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

	private NavMeshAgent navAgent;
	
	void Start() {
		foeGlanceCommand = GetComponentInChildren<Foe_Glance_Command>();
		foeDetectionHandler = GetComponentInChildren<Foe_Detection_Handler>();
		navAgent = GetComponent<NavMeshAgent>();
		speed = navAgent.speed;
		camControl = FindObjectOfType<QCameraControl>();
		state = alertState.patrolling;
		UpdateDestination();
	}

	void FixedUpdate() {	
		if (navAgent == null) {
			return;
		}
					
		if (stayFrozenOnLook) {
			if (foeGlanceCommand.isLookingAround) {
				navAgent.Stop ();
			} else {
				stayFrozenOnLook = false;
				navAgent.Resume ();
			}
		} else if (state == alertState.investigating && foeDetectionHandler.isAggressive) {
			navAgent.speed = speed * foeDetectionHandler.sprintMultiplier;
		} else {
			navAgent.speed = speed;
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
		if (navAgent.enabled) {
			navAgent.destination = currentDestination;	
		} else {
			queuedMovement = true;
		}
	}
	
	public void StartInvestigation(Vector3 destination, bool isPlayer) {
		isTrackingPlayer = isPlayer;
		if (!navAgent.enabled) {
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
		navAgent.destination = destination;
		state = alertState.investigating;
		
		foeGlanceCommand.OverrideGlanceCommand();
	}
	
	public void EndInvestigation() {
		AlertHub.guardOnAlert = false;
		MusicPlayer.Escaped(foeDetectionHandler);
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
