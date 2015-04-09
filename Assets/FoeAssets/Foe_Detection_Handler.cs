 using UnityEngine;
using System.Collections;

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
	public bool canCommunicate = true;
	float timeAttemptingCommunication = 0f;
	float timeToCommunicate = 5f;
	
	public int jurisdictionZone;

	int cullingMask;

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
	}
	
	void Update () {
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
			if (canCommunicate) {
				Communicate();
			}
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
	
	void Communicate() {
		if (isAggressive) {
			timeAttemptingCommunication += Time.deltaTime;
			if (timeAttemptingCommunication >= timeToCommunicate) {
				FoeAlertSystem.Alert(PlayerController.player.transform.position);
			}
		}
	}
	
	//Player kills guard
	public void Interact() {
		if (timeSincePlayerSpotted > 1f) {
			AudioSource.PlayClipAtPoint(AudioDefinitions.main.WilhelmScream, transform.position);
			
			taser.SetActive(false);
			GetComponentInParent<Rigidbody>().isKinematic = false;
			GetComponentInParent<Rigidbody>().useGravity = true;
			Vector2 randRotation = 2500f * Random.insideUnitCircle.normalized;
			GetComponentInParent<Rigidbody>().AddTorque(new Vector3(randRotation.x, 0, randRotation.y));
			GetComponentInParent<NavMeshAgent>().enabled = false;
			GetComponentInParent<Foe_Movement_Handler>().enabled = false;
			GetComponentInChildren<Light>().enabled = false;
			GetComponent<Foe_Glance_Command>().enabled = false;
			enabled = false;
		}
	}
}
