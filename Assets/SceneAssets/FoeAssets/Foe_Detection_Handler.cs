 using UnityEngine;
using System.Collections;

public class Foe_Detection_Handler : QInteractable {
	public int currentRoom;
	public GameObject taser;
	
	//For haphazard use:
	private Vector3 displacement;
	private float visualDetectionValue, audialDetectionValue;
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
	
	public bool isDead = false;
	bool playerDisabled = false;

	int cullingMask;

	public override void Start () {	
		taser = Instantiate(ObjectPrefabDefinitions.main.FoeTaser) as GameObject;
		taser.transform.parent = transform;
		taser.transform.localPosition = new Vector3(-0.7f, -0.5f, 0.5f);
		taser.transform.localEulerAngles = Vector3.zero;
		taser.SetActive(false);
		
		baseSpeed = GetComponentInParent<NavMeshAgent>().speed;
		
		/*cullingMask = (1 << Layerdefs.wall) + (1 << Layerdefs.floor)
				+ (1 << Layerdefs.q_display) + (1 << Layerdefs.prop);*/
		
		movementHandler = GetComponentInParent<Foe_Movement_Handler>();
		
		cullingMask = (1 << Layerdefs.floor) + (1 << Layerdefs.wall) + (1 << Layerdefs.stan) + (1 << Layerdefs.prop)
				+ (1 << Layerdefs.foe);
		
		base.Start();
	}
	
	void Update () {
		if (movementHandler == null) {
			movementHandler = GetComponentInParent<Foe_Movement_Handler>();
		}
		//GetCurrentRoom();
		if (!isDead) {
			displacement = PlayerController.player.transform.position - transform.position;
			CalculateVisualDetection();
			CalculateAudialDetection();
			React();
		} else {
			if (canCommunicate) {
				HeartbeatMonitor();
			}
		}
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
				} else detected = false;
			} else detected = false;
		} else detected = false;
		
		if (detected) {
			visualDetectionValue = 2f;
		} else {
			visualDetectionValue = 0;
		}
	}
	
	void CalculateAudialDetection() {
		audialDetectionValue = audioMultiplier / Mathf.Pow (displacement.magnitude, 2);
	}
	
	void React() {
		Debug_Foe_Alert_Status.visualDetectionValue = visualDetectionValue;
		Debug_Foe_Alert_Status.audialDetectionValue = audialDetectionValue;
		
		timeSincePlayerSpotted += Time.deltaTime;

		if (visualDetectionValue >= 2f) {
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
		if (!isDead && movementHandler != null){
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
	
	void HeartbeatMonitor() {
		timeAttemptingCommunication += Time.deltaTime;
		if (timeAttemptingCommunication > timeToCommunicate) {
			FindObjectOfType<HeartbeatMonitor>().ReceiveDistressCall(transform.position);
			canCommunicate = false;
		}
	}	
	
	//Player kills guard
	public void Interact() {
		if (timeSincePlayerSpotted > 1f) {
			isDead = true;
			taser.SetActive(false);
			GetComponentInParent<NavMeshAgent>().enabled = false;
			GetComponent<Foe_Glance_Command>().enabled = false;
			timeAttemptingCommunication = 0f;
			GetComponentInParent<Rigidbody>().isKinematic = false;
			GetComponentInParent<Rigidbody>().useGravity = true;
			GetComponentInChildren<Light>().enabled = false;
		}
	}
	
	public override void Trigger() {
		if (!isDead) {
			if (canCommunicate) {
				canCommunicate = false;
				GameController.SendPlayerMessage("", 5);
				return;
			} else {
				canCommunicate = true;
				GameController.SendPlayerMessage("", 5);
				return;
			}
		} else {
			if (canCommunicate) {
				timeToCommunicate = float.MaxValue;
				canCommunicate = false;
				playerDisabled = true;
				GameController.SendPlayerMessage("Your partner has disabled the guard's heartbeat monitor!", 5);
			} else if (playerDisabled) {
				GameController.SendPlayerMessage("", 5);
			} else {
				GameController.SendPlayerMessage("", 5);
			}
		}
	}
	
	public override Sprite GetSprite() {
		if (canCommunicate) {
			return ButtonSpriteDefinitions.main.guardSounding;
		} else {
			return ButtonSpriteDefinitions.main.guardSilent;
		}
	}
}
