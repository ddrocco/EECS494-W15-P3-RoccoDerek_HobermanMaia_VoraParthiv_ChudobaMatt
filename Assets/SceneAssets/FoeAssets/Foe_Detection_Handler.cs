using UnityEngine;
using System.Collections;

public class Foe_Detection_Handler : QInteractable {
	public int currentRoom;
	public GameObject taserPrefab;
	GameObject taser;
	
	//For haphazard use:
	private Vector3 displacement;
	private float visualDetectionValue, audialDetectionValue;
	public static float audioMultiplier = 0f;
	public float visionWidth = 75f;
	
	//Exclamation points:
	public bool isAttentive = false;
	public GameObject alertObject1, alertObject2;
	
	public Foe_Movement_Handler movementHandler;
	
	int cullingMask;
	
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

	void Start () {
		base.Start();
		
		FoeAlertSystem.foeList.Add(this);
	
		taser = Instantiate(taserPrefab) as GameObject;
		taser.SetActive(false);
		taser.transform.parent = transform;
		taser.transform.localPosition = new Vector3(-0.7f, -0.5f, 0.5f);
		
		baseSpeed = GetComponentInParent<NavMeshAgent>().speed;
		
		cullingMask = (1 << Layerdefs.wall) + (1 << Layerdefs.floor)
				+ (1 << Layerdefs.q_display) + (1 << Layerdefs.prop);
		alertObject1.GetComponent<Renderer>().enabled = false;
		alertObject2.GetComponent<Renderer>().enabled = false;
		
		movementHandler = GetComponentInParent<Foe_Movement_Handler>();
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
			if (canCommunicate) {
				Communicate();
			}
		} else {
			HeartbeatMonitor();
		}
	}
	
	void CalculateVisualDetection() {
		Debug.DrawRay (transform.position, transform.rotation * Vector3.forward
				* displacement.magnitude, Color.red);
		Debug.DrawRay (transform.position, displacement, Color.blue);
		float visualAngle = Vector3.Angle(transform.rotation * Vector3.forward, displacement);
		
		int visualMultiplier = GetPlayerRaycasts();
		
		if (visualMultiplier == 0 || visualAngle > visionWidth) {
			visualDetectionValue = 0;
		} else {
			float lightFactor = 0.25f; //When it's brightly lit.
										//When it's dark, this should be closer to 2f.
			visualDetectionValue = visualMultiplier
					* Mathf.Cos (visualAngle * (Mathf.PI / 180f ))
					/ Mathf.Pow (displacement.magnitude, lightFactor);
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
	
	int GetPlayerRaycasts() {
		Vector3[] playerVertices = PlayerController.player.GetComponent<Player_Vertices>().GetVertices();
		
		int visibleVertices = 0;
		foreach (Vector3 vertex in playerVertices) {
			RaycastHit hitInfo;
			bool raycastHit = Physics.Raycast(
					transform.position,
					(vertex - transform.position),
					out hitInfo,
					(vertex - transform.position).magnitude,
					cullingMask);
			if (!raycastHit) {
				++visibleVertices;
				//Debug.DrawRay (transform.position, (vertex - transform.position), Color.green);
			} else {
				//Debug.DrawRay (transform.position, (vertex - transform.position), Color.magenta);
			}
		}
		return visibleVertices;
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
		//alertObject1.GetComponent<Renderer>().enabled = true;
		//alertObject2.GetComponent<Renderer>().enabled = true;
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
	
	void HeartbeatMonitor() {
		timeAttemptingCommunication += Time.deltaTime;
		if (timeAttemptingCommunication > timeToCommunicate) {
			FoeAlertSystem.Alert(transform.position);
			this.enabled = false;
		}
		
	}
	
	/*void GetCurrentRoom() {
		currentRoom = Room_Floor_Designation.GetCurrentRoom(transform.position);
		Debug_Foe_Alert_Status.currentRoom = currentRoom;
	}*/
	
	
	//Player kills guard
	public void Interact() {
		if (timeSincePlayerSpotted > 1f) {
			isDead = true;
			GetComponentInParent<NavMeshAgent>().enabled = false;
			timeAttemptingCommunication = 0f;
			FoeAlertSystem.foeList.Remove(this);
		}
	}
	
	public override void Trigger() {
		if (!isDead) {
			canCommunicate = false;
			GameController.SendPlayerMessage("Your partner has disabled enemy communications.", 5);
		} else {
			timeToCommunicate = float.MaxValue;
			canCommunicate = false;
			GameController.SendPlayerMessage("Heartbeat monitor alert has been disabled... Hopefully before it sounded...", 5);
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
