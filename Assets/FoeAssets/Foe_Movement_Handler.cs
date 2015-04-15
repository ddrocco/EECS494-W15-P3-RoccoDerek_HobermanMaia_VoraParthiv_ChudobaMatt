using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Foe_Movement_Handler : MonoBehaviour {
	public QCameraControl camControl;
	
	public enum alertState{
		patrolling,	//Nominal, walking around
		investigating, //Moves to a location
		returning //Goes back to patrolling.
	};	
	public alertState state = alertState.patrolling;
	public Vector3 currentDestination;
	[HideInInspector]
	public bool isTrackingPlayer = false;
	
	float minNodeDistance = 0.5f;
	public float minInvestigationDistance = 1f;
	
	//Patrolling variables:
	public List<int> defaultPath;
	public int currentPathNode = 0;
	
	//Investigating IN / Returning OUT variables:
	public Vector3 originLocation;
	bool originIsValid = false;
	
	//Child classes:
	Foe_Glance_Command foeGlanceCommand;
	Foe_Detection_Handler foeDetectionHandler;
	
	[HideInInspector]
	public float speed;
	public bool stayFrozenOnLook = false;
	
	[HideInInspector]
	public bool queuedMovement;
	
	void Start() {
		foeGlanceCommand = GetComponentInChildren<Foe_Glance_Command>();
		foeDetectionHandler = GetComponentInChildren<Foe_Detection_Handler>();
		speed = GetComponent<NavMeshAgent>().speed;
		camControl = FindObjectOfType<QCameraControl>();
		state = alertState.patrolling;
		UpdateDestination();
	}

	void FixedUpdate() {	
		if (GetComponent<NavMeshAgent>() == null) {
			return;
		}
		
		/*if (!GetComponent<NavMeshAgent>().hasPath) {
			timeWithoutPath += Time.deltaTime;
			if (timeWithoutPath > 2f) {
				switch(state) {
				case alertState.patrolling:
					UpdateDestination();
					break;
				case alertState.investigating:
					EndInvestigation();
					break;
				case alertState.returning:
					UpdateDestination();
					break;
				}
				GetComponent<NavMeshAgent>().enabled = false;
				GetComponent<NavMeshAgent>().enabled = true;
				timeWithoutPath = 0f;
			}
		} else {
			timeWithoutPath = 0f;
		}*/
					
		if (stayFrozenOnLook) {
			if (foeGlanceCommand.isLookingAround) {
				GetComponent<NavMeshAgent>().Stop ();
			} else {
				stayFrozenOnLook = false;
				GetComponent<NavMeshAgent>().Resume ();
			}
		} else if (state == alertState.investigating && foeDetectionHandler.isAggressive) {
			GetComponent<NavMeshAgent>().speed = speed * foeDetectionHandler.sprintMultiplier;
		} else {
			GetComponent<NavMeshAgent>().speed = speed;
		}
		
		float minDist;
		if (state == alertState.patrolling) {
			minDist = minNodeDistance;
		} else if (state == alertState.investigating) {
			minDist = minInvestigationDistance;
		} else {
			minDist = minNodeDistance;
		}
		
		if (Vector3.Distance(new Vector3(transform.position.x, 0, transform.position.z), 
				new Vector3(currentDestination.x, 0, currentDestination.z)) < minDist) {
		     //If close to current destination
			UpdateDestination();
		}
	}
	
	void UpdateDestination() {
		if (state == alertState.patrolling) {
			if (defaultPath.Count == 0) {
				currentDestination = transform.position;
				GetComponentInChildren<Light>().enabled = false;
			} else {
				currentDestination = World_Foe_Route_Node.routeNodeList[defaultPath[currentPathNode]].transform.position;
				currentPathNode += 1;
				if (currentPathNode >= defaultPath.Count) {
					currentPathNode = 0;
				}
				if (foeDetectionHandler.isAttentive) {
					foeGlanceCommand.prepareToLook = true;
					foeGlanceCommand.waitToLook = 1f;
				}
			}
		} else if (state == alertState.investigating) {
			GetComponentInChildren<Light>().enabled = true;
			if (!foeGlanceCommand.lookIsStationary) {
				foeGlanceCommand.ReceiveGlanceCommand(3f, 0.25f, -90f, 90f);
				foeGlanceCommand.lookIsStationary = true;
			}
			if (!foeGlanceCommand.isLookingAround) {
				foeGlanceCommand.lookIsStationary = false;
				foeGlanceCommand.prepareToLook = true;
				foeGlanceCommand.waitToLook = 1f;
				EndInvestigation();
			}
		} else if (state == alertState.returning) {
			state = alertState.patrolling;
			originIsValid = false;
			if (defaultPath.Count == 0) {
				currentDestination = transform.position;
			} else {
				currentDestination = World_Foe_Route_Node.routeNodeList[defaultPath[currentPathNode]].transform.position;
			}
		}
		
		currentDestination.y = transform.position.y;
		if (GetComponent<NavMeshAgent>().enabled) {
			GetComponent<NavMeshAgent>().destination = currentDestination;	
		} else {
			queuedMovement = true;
		}
	}
	
	public void StartInvestigation(Vector3 destination, bool isPlayer) {
		isTrackingPlayer = isPlayer;
		if (!GetComponent<NavMeshAgent>().enabled) {
			return;
		}
		
		if (!originIsValid) {
			originLocation = transform.position;
		}
		stayFrozenOnLook = false;
		originIsValid = true;
		currentDestination = destination;
		if (isPlayer) {
			minInvestigationDistance = FoeAlertSystem.playerRange;
		} else {
			minInvestigationDistance = FoeAlertSystem.bombRange;
		}
		GetComponent<NavMeshAgent>().destination = destination;
		state = alertState.investigating;
		
		foeGlanceCommand.OverrideGlanceCommand();
	}
	
	public void EndInvestigation() {
		isTrackingPlayer = false;
		state = alertState.returning;
		currentDestination = originLocation;
		//turn off alarms
		camControl.AlertOff();
	}
	
	public void Interact() {
		GetComponentInChildren<Foe_Detection_Handler>().Interact();
	}
}
