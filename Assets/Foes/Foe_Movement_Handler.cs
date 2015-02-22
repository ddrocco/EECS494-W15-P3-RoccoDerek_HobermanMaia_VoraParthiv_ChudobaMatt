using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Foe_Movement_Handler : MonoBehaviour {
	public List<int> defaultPath;
	public int currentPathNode = 0;
	
	public enum alertState{
		patrolling,	//Nominal, walking around
		investigating,	//Moves to a location, then goes back to patrolling.
	};	
	public alertState state = alertState.patrolling;
	
	public Vector3 currentDestination;
	
	//Patrolling variables
	bool isRotating = true;
	
	//Investigating variables
	public GameObject player;
	//Vector3 soundLocation;
	public Vector3 originLocation;
	bool originIsValid = false;
	bool isReturning;
	
	Foe_Glance_Command foeGlanceCommand;
	Foe_Detection_Handler foeDetectionHandler;
	
	void Start() {
		foeGlanceCommand = GetComponentInChildren<Foe_Glance_Command>();
		foeDetectionHandler = GetComponentInChildren<Foe_Detection_Handler>();
	
		if (state == alertState.investigating) {
			StartInvestigation();
		} else {
			UpdateDestination();
		}
	}

	void FixedUpdate() {
		if ((new Vector3(transform.position.x, 0, transform.position.z)
				- new Vector3(currentDestination.x, 0, currentDestination.z)).magnitude < 0.1f) {
			UpdateDestination();
		}
	}
	
	void UpdateDestination() {
		if (state == alertState.patrolling) {
			currentDestination = Foe_Route_Node.routeNodeList[defaultPath[currentPathNode]].transform.position;
			
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
				currentDestination = Foe_Route_Node.routeNodeList[defaultPath[currentPathNode]].transform.position;
				originIsValid = false;
				
			}
		}
		currentDestination.y = transform.position.y;
		GetComponent<NavMeshAgent>().destination = currentDestination;
	}
	
	public void StartInvestigation() {
		if (!originIsValid) {
			originLocation = transform.position;
		}
		originIsValid = true;
		currentDestination = player.transform.position;
		GetComponent<NavMeshAgent>().destination = currentDestination;
		isReturning = false;
		//transform.LookAt(new Vector3(currentDestination.x, transform.position.y, currentDestination.z));
		state = alertState.investigating;
	}
}
