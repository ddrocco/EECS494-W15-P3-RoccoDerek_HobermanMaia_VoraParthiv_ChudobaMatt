using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Foe_Movement_Handler : MonoBehaviour {
	public enum alertState{
		patrolling,	//Nominal, walking around
		investigating,	//Moves to a location, then goes back to patrolling.
	};	
	public alertState state = alertState.patrolling;
	
	public GameObject player;
	public Vector3 currentDestination;
	
	//Patrolling variables:
	public List<int> defaultPath;
	public int currentPathNode = 0;
	
	//Investigating variables:
	public Vector3 originLocation;
	bool originIsValid = false;
	public bool isReturning;
	
	//Child classes:
	Foe_Glance_Command foeGlanceCommand;
	Foe_Detection_Handler foeDetectionHandler;
	
	public float speed;
	
	public bool stayFrozenOnLook = false;
	
	void Start() {
		foeGlanceCommand = GetComponentInChildren<Foe_Glance_Command>();
		foeDetectionHandler = GetComponentInChildren<Foe_Detection_Handler>();
		speed = GetComponent<NavMeshAgent>().speed;
	
		if (state == alertState.patrolling){
			UpdateDestination();
		} else if (state == alertState.investigating) {
			StartInvestigation(player.transform.position);
		}
	}

	void FixedUpdate() {
		if (stayFrozenOnLook) {
			GetComponent<NavMeshAgent>().speed = 0;
			if (!foeGlanceCommand.isLookingAround) {
				stayFrozenOnLook = false;
				GetComponent<NavMeshAgent>().speed = speed;
			}
		} else if (state == alertState.investigating && isReturning == false) {
			if (foeDetectionHandler.isAggressive) {
				GetComponent<NavMeshAgent>().speed = speed * foeDetectionHandler.sprintMultiplier;
			} else {
				GetComponent<NavMeshAgent>().speed = speed;
			}
		}
		if ((new Vector3(transform.position.x, 0, transform.position.z)
		     - new Vector3(currentDestination.x, 0, currentDestination.z)).magnitude < 0.1f) {
			UpdateDestination();
		}
	}
	
	void UpdateDestination() {
		if (state == alertState.patrolling) {
			currentDestination = World_Foe_Route_Node.routeNodeList[defaultPath[currentPathNode]].transform.position;
			
			currentPathNode += 1;
			if (currentPathNode >= defaultPath.Count) {
				currentPathNode = 0;
			}
			
			if (foeDetectionHandler.isAttentive) {
				foeGlanceCommand.prepareToLook = true;
				foeGlanceCommand.waitToLook = 1f;
			}
		} else if (state == alertState.investigating) {
			if (!isReturning) {
				if (!foeGlanceCommand.lookIsStationary) {
					foeGlanceCommand.ReceiveGlanceCommand(3f, 0.25f, -90f, 90f);
					foeGlanceCommand.lookIsStationary = true;
				}
				if (!foeGlanceCommand.isLookingAround) {
					foeGlanceCommand.lookIsStationary = false;
					isReturning = true;
					currentDestination = originLocation;
					
					foeGlanceCommand.prepareToLook = true;
					foeGlanceCommand.waitToLook = 1f;
				}
			} else if (isReturning) {
				state = alertState.patrolling;
				currentDestination = World_Foe_Route_Node.routeNodeList[defaultPath[currentPathNode]].transform.position;
				originIsValid = false;
			}
		}
		currentDestination.y = transform.position.y;
		GetComponent<NavMeshAgent>().destination = currentDestination;
	}
	
	public void StartInvestigation(Vector3 destination) {
		if (!originIsValid) {
			originLocation = transform.position;
		}
		stayFrozenOnLook = false;
		originIsValid = true;
		currentDestination = destination;
		GetComponent<NavMeshAgent>().destination = destination;
		isReturning = false;
		state = alertState.investigating;
		
		foeGlanceCommand.OverrideGlanceCommand();
	}
}
