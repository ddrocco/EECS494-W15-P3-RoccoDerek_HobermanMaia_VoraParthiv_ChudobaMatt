using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Foe_Detection_Handler : MonoBehaviour {
	public int currentRoom;
	public GameObject taser;
	
	//For haphazard use:
	private Vector3 displacement;
	private bool playerSpotted = false;
	public float visionWidth = 75f;
	
	//Exclamation points:
	public bool isAttentive = false;
	
	public Foe_Movement_Handler movementHandler;
	
	//Player spotted:
	public bool isAggressive = false;
	[HideInInspector]
	public float timeSincePlayerSpotted = 10f;
	[HideInInspector]
	public float timeUntilPlayerLost = 1f;
	float baseSpeed;
	public float sprintMultiplier = 5f;
	
	public int jurisdictionZone;

	int cullingMask;
	
	float shoveDisorientationTime = 1f;
	float timeUntilOriented;
	
	public bool isDeaf = false;

	void Start () {	
		taser = Instantiate(ObjectPrefabDefinitions.main.FoeTaser) as GameObject;
		taser.transform.parent = transform;
		taser.transform.localPosition = new Vector3(-0.6645966f, -2f, 0.02145767f);
		taser.transform.localEulerAngles = new Vector3(0, 10f, 0); //290.9929
		taser.SetActive(false);
		
		baseSpeed = GetComponentInParent<NavMeshAgent>().speed;
		movementHandler = GetComponentInParent<Foe_Movement_Handler>();
		
		cullingMask = (1 << Layerdefs.floor) + (1 << Layerdefs.wall) + (1 << Layerdefs.stan) + (1 << Layerdefs.prop)
				+ (1 << Layerdefs.foe);
		
		timeUntilOriented = shoveDisorientationTime;
	}
	
	void Update () {
		if (timeUntilOriented < shoveDisorientationTime) {
			timeUntilOriented -= Time.deltaTime;
			if (timeUntilOriented <= 0f) {
				timeUntilOriented = shoveDisorientationTime;
				GetComponentInParent<Rigidbody>().isKinematic = true;
				GetComponentInParent<NavMeshAgent>().enabled = true;
				GetComponentInParent<Rigidbody>().useGravity = false;
				GetComponentInParent<Rigidbody>().freezeRotation = false;
				timeSincePlayerSpotted = 0f;
				if (GetComponentInParent<Foe_Movement_Handler>().queuedMovement) {
					GetComponentInParent<NavMeshAgent>().destination =
							GetComponentInParent<Foe_Movement_Handler>().currentDestination;	
				}
			}
		}
	
		if (movementHandler == null) {
			movementHandler = GetComponentInParent<Foe_Movement_Handler>();
		}
		
		displacement = PlayerController.player.transform.position - transform.position;
		CalculateVisualDetection();
		React();
	}
	
	void CalculateVisualDetection() {
		Camera myCam = GetComponent<Camera>();
		Plane[] planes = GeometryUtility.CalculateFrustumPlanes(myCam);
		bool detected;

		if (GeometryUtility.TestPlanesAABB(planes, PlayerController.player.GetComponent<Collider>().bounds)) {
			RaycastHit hit;
			Vector3 heading = PlayerController.player.transform.position - transform.position;
			float distance = heading.magnitude;
			Vector3 direction = heading.normalized;
			if (Physics.Raycast(transform.position, direction, out hit, distance, cullingMask)) {
				if (hit.collider.CompareTag("Player") == true) {
					detected = true;
				} else {
					detected = false;
				}
			} else {
				detected = false;
			}
		} else {
			detected = false;
		}
		
		if (detected) {
			playerSpotted = true;
		} else {
			playerSpotted = false;
		}
	}
	
	bool CanHearPlayer() {
		if (isDeaf || FindObjectOfType<PlayerController>().isStationary) {
			return false;
		} else if (FindObjectOfType<PlayerController>().state == PlayerController.State.sprinting) {
			if (displacement.magnitude < 15f) {
				return true;
			} else {
				return false;
			}
		} else {
			if (displacement.magnitude < 3f) {
				return true;
			} else {
				return false;
			}
		}
	}
	
	void React() {
		Debug_Foe_Alert_Status.playerSpotted = playerSpotted;
		
		timeSincePlayerSpotted += Time.deltaTime;

		if (playerSpotted) {
			PlayerSpotted();
			MoveToPlayer();
		} else if (CanHearPlayer()) {
			if (!GetComponentInParent<Foe_Movement_Handler>().isTrackingPlayer) {
				GetComponent<AudioSource>().clip = SelectRandomClip(AudioDefinitions.main.GuardHearsPlayer);
				GetComponent<AudioSource>().Play ();
			}
			MoveToPlayer();
		} else if (isAggressive) {
			GetComponentInParent<NavMeshAgent>().speed = baseSpeed * sprintMultiplier;
			if (movementHandler.state == Foe_Movement_Handler.alertState.returning) {
				isAggressive = false;
				taser.SetActive(false);
			}
		}
	}
	
	void PlayerSpotted() {
		isAggressive = true;
		isDeaf = false;
		if (timeSincePlayerSpotted >= timeUntilPlayerLost) {
			GetComponent<AudioSource>().clip = SelectRandomClip(AudioDefinitions.main.GuardSpotsPlayer);
			GetComponent<AudioSource>().Play();
		}
		taser.gameObject.SetActive(true);
		timeSincePlayerSpotted = 0f;
	}
	
	public void MoveToPlayer() {
		isAttentive = true;
		if (movementHandler != null){
			movementHandler.StartInvestigation(PlayerController.player.transform.position, isPlayer: true);
		}
	}
	
	//Player kills guard
	public void Interact() {
		if (!enabled || timeUntilOriented != shoveDisorientationTime) {
			return;
		}
		if (timeSincePlayerSpotted > timeUntilPlayerLost) {
			AudioSource.PlayClipAtPoint(AudioDefinitions.main.WilhelmScream, transform.position);
			
			taser.SetActive(false);
			GetComponentInParent<Rigidbody>().isKinematic = false;
			GetComponentInParent<Rigidbody>().useGravity = true;
			Vector2 randRotation = 2500f * Random.insideUnitCircle.normalized;
			GetComponentInParent<Rigidbody>().AddTorque(new Vector3(randRotation.x, 0, randRotation.y));
			GetComponentInParent<Rigidbody>().mass = 0.1f;
			GetComponentInParent<NavMeshAgent>().enabled = false;
			GetComponentInParent<Foe_Movement_Handler>().enabled = false;
			GetComponentInChildren<Light>().enabled = false;
			GetComponent<Foe_Glance_Command>().enabled = false;
			GetComponentInParent<Foe_Movement_Handler>().tag = "Untagged";
			tag = "Untagged";
			enabled = false;
		} else {
			GetComponentInParent<Rigidbody>().isKinematic = false;
			GetComponentInParent<Rigidbody>().mass = 0.1f;
			GetComponentInParent<Rigidbody>().useGravity = true;
			GetComponentInParent<Rigidbody>().freezeRotation = true;
			GetComponentInParent<NavMeshAgent>().enabled = false;

			float magnitude = 50f;
			Vector3 direction = transform.position - FindObjectOfType<PlayerController>().transform.position;
			Vector3 horizontalShove = magnitude * new Vector3(direction.x, 0, direction.z).normalized;
			Vector3 verticalShove = new Vector3 (0, magnitude / 2, 0);
			GetComponentInParent<Rigidbody>().AddForce(horizontalShove + verticalShove);
			
			timeUntilOriented -= Time.deltaTime;
		}
	}
	
	public static AudioClip SelectRandomClip(List<AudioClip> clips) {
		int i = Mathf.FloorToInt(Random.Range(0, clips.Count));
		return clips[i];
	}
}