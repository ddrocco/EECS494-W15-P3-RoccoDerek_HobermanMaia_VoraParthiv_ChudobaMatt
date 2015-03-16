﻿using UnityEngine;
using System.Collections;

public class Foe_Detection_Handler : MonoBehaviour {
	public GameObject player;
	public int currentRoom;
	
	//For haphazard use:
	private Vector3 displacement;
	private float visualDetectionValue, audialDetectionValue;
	public static float audioMultiplier = 0f;
	public float visionWidth = 75f;
	
	//Exclamation points:
	public bool isAttentive = false;
	public GameObject alertObject1, alertObject2;
	
	Foe_Movement_Handler movementHandler;
	
	int cullingMask;

	void Start () {
		cullingMask = (1 << Layerdefs.wall) + (1 << Layerdefs.floor)
				+ (1 << Layerdefs.interactable) + (1 << Layerdefs.door);
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
		
		if (audialDetectionValue >= 0.5f) {
			isAttentive = true;
			alertObject1.GetComponent<Renderer>().enabled = true;
			alertObject2.GetComponent<Renderer>().enabled = true;
			movementHandler.StartInvestigation();
		}
		
		if (visualDetectionValue >= 2f) {
			PlayerDetected();
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
	
	void PlayerDetected() { //No insta-death--chase player down
		GameController.PlayerDead = true;
		GameController.GameOverMessage = "You were spotted by a guard!\nPress A to restart the level";
	}
	
	/*void GetCurrentRoom() {
		currentRoom = Room_Floor_Designation.GetCurrentRoom(transform.position);
		Debug_Foe_Alert_Status.currentRoom = currentRoom;
	}*/
}
