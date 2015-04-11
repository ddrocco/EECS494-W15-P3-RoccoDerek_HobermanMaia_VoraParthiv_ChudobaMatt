using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Foe_Detection_Handler : MonoBehaviour {
	public int currentRoom;
	public GameObject taser;
	
	//For haphazard use:
	private Vector3 displacement;
	private float audialDetectionValue;
	private bool playerSpotted = false;
	public static float audioMultiplier = 0f;
	public float visionWidth = 75f;
	
	//Exclamation points:
	public bool isAttentive = false;
	
	public Foe_Movement_Handler movementHandler;
	
	//Player spotted:
	public bool isAggressive = false;
	float timeSincePlayerSpotted = 10f;
	float timeUntilPlayerLost = 1f;
	float baseSpeed;
	public float sprintMultiplier = 5f;
	
	//Communicate findings:
	bool hasSeenPlayer = false;
	
	public int jurisdictionZone;

	int cullingMask;
	
	float shoveDisorientationTime = 1f;
	float timeUntilOriented;

	void Start () {	
		taser = Instantiate(ObjectPrefabDefinitions.main.FoeTaser) as GameObject;
		taser.transform.parent = transform;
		taser.transform.localPosition = new Vector3(-0.7f, -0.5f, 0.5f);
		taser.transform.localEulerAngles = Vector3.zero;
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
			}
		}
	
		if (movementHandler == null) {
			movementHandler = GetComponentInParent<Foe_Movement_Handler>();
		}
		
		displacement = PlayerController.player.transform.position - transform.position;
		CalculateVisualDetection();
		CalculateAudialDetection();
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
	
	void CalculateAudialDetection() {
		audialDetectionValue = audioMultiplier / Mathf.Pow (displacement.magnitude, 2);
	}
	
	void React() {
		Debug_Foe_Alert_Status.playerSpotted = playerSpotted;
		Debug_Foe_Alert_Status.audialDetectionValue = audialDetectionValue;
		
		timeSincePlayerSpotted += Time.deltaTime;

		if (playerSpotted) {
			PlayerSpotted();
			MoveToPlayer();
		} else if (audialDetectionValue >= 0.5f ||
				(timeSincePlayerSpotted < timeUntilPlayerLost && hasSeenPlayer)) {
			MoveToPlayer();
		} else if (isAggressive) {
			GetComponentInParent<NavMeshAgent>().speed = baseSpeed * sprintMultiplier;
			if (movementHandler.isReturning) {
				isAggressive = false;
				taser.SetActive(false);
			}
		}
	}
	
	void PlayerSpotted() { //No insta-death--chase player down
		if (!isAggressive) {
			isAggressive = true;
		}
		if (timeSincePlayerSpotted >= timeUntilPlayerLost) {
			List<AudioClip> soundClips = AudioDefinitions.main.GuardSpotsPlayer;
			int i = Mathf.FloorToInt(Random.Range(0, soundClips.Count));
			AudioSource.PlayClipAtPoint(soundClips[i], transform.position);
		}
		taser.gameObject.SetActive(true);
		timeSincePlayerSpotted = 0f;
		hasSeenPlayer = true;
	}
	
	public void MoveToPlayer() {
		isAttentive = true;
		if (movementHandler != null){
			movementHandler.StartInvestigation(PlayerController.player.transform.position);
		}
	}
	
	//Player kills guard
	public void Interact() {
		if (!enabled || timeUntilOriented != shoveDisorientationTime) {
			return;
		}
		if (timeSincePlayerSpotted > timeUntilPlayerLost) {
			print ("Guard Slain!");
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
}
