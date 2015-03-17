using UnityEngine;
using System.Collections;

public class Foe_Detection_Handler : MonoBehaviour {
	public GameObject player;
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
	bool isAggressive = false;
	float timeSincePlayerSpotted = 0f;
	float timeUntilPlayerLost = 1f;
	float baseSpeed;
	public float sprintMultiplier = 5f;
	
	//Communicate findings:
	bool hasSeenPlayer = false;
	public bool canCommunicate = true;
	float timeAttemptingCommunication = 0f;
	float timeToCommunicate = 5f;

	void Start () {
		FoeAlertSystem.foeList.Add(this);
	
		taser = Instantiate(taserPrefab) as GameObject;
		taser.SetActive(false);
		taser.transform.parent = transform;
		taser.transform.localPosition = new Vector3(-0.7f, -0.5f, 0.5f);
		
		baseSpeed = GetComponentInParent<NavMeshAgent>().speed;
		player = PlayerController.player.gameObject;
		
		cullingMask = (1 << Layerdefs.wall) + (1 << Layerdefs.floor)
				+ (1 << Layerdefs.q_interactable) + (1 << Layerdefs.door);
		alertObject1.GetComponent<Renderer>().enabled = false;
		alertObject2.GetComponent<Renderer>().enabled = false;
		movementHandler = GetComponentInParent<Foe_Movement_Handler>();
	}
	
	void Update () {
		//GetCurrentRoom();
		displacement = player.transform.position - transform.position;
		CalculateVisualDetection();
		CalculateAudialDetection();
		React();
		Communicate();
	}
	
	void CalculateVisualDetection() {
		Debug.DrawRay (transform.position, transform.rotation * Vector3.forward * displacement.magnitude, Color.red);
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
			GetComponentInParent<NavMeshAgent>().speed = baseSpeed;
			isAggressive = false;
			taser.SetActive(false);
		}
	}
	
	int GetPlayerRaycasts() {
		Vector3[] playerVertices = player.GetComponent<Player_Vertices>().GetVertices();
		
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
			taser.gameObject.SetActive(true);
		}
		timeSincePlayerSpotted = 0f;
		hasSeenPlayer = true;
		GetComponentInParent<NavMeshAgent>().speed = baseSpeed * sprintMultiplier;
	}
	
	public void MoveToPlayer() {
		isAttentive = true;
		alertObject1.GetComponent<Renderer>().enabled = true;
		alertObject2.GetComponent<Renderer>().enabled = true;
		movementHandler.StartInvestigation(player.transform.position);
	}
	
	void Communicate() {
		if (hasSeenPlayer) {
			timeAttemptingCommunication += Time.deltaTime;
			if (timeAttemptingCommunication >= timeToCommunicate) {
				//Communicate with other guards.
			}
		}
	}
	
	/*void GetCurrentRoom() {
		currentRoom = Room_Floor_Designation.GetCurrentRoom(transform.position);
		Debug_Foe_Alert_Status.currentRoom = currentRoom;
	}*/
}
