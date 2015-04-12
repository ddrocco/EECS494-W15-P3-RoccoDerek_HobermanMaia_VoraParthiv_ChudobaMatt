using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Foe_Movement_Handler : MonoBehaviour {
	public enum alertState{
		patrolling,	//Nominal, walking around
		investigating, //Moves to a location, then goes back to patrolling.
	};	
	public alertState state = alertState.patrolling;
	public Vector3 currentDestination;
	
	float minNodeDistance = 0.5f;
	
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
	
	public float baseSpeed = 1f;
	public bool stayFrozenOnLook = false;
	
	[HideInInspector]
	public NavMeshAgent navigator;
	[HideInInspector]
	public bool canMove = true;
	
	public float rotationSpeed = 1f;
	
	void Start() {
		foeGlanceCommand = GetComponentInChildren<Foe_Glance_Command>();
		foeDetectionHandler = GetComponentInChildren<Foe_Detection_Handler>();
		navigator = Instantiate(ObjectPrefabDefinitions.main.FoeNavigator).GetComponent<NavMeshAgent>();
		
		navigator.enabled = false;
		navigator.transform.position = transform.position;
		navigator.enabled = true;
		
		navigator.speed = baseSpeed;
		if (state == alertState.patrolling){
			UpdateDestination();
		} else if (state == alertState.investigating) {
			StartInvestigation(PlayerController.player.transform.position);
		}
	}

	void Update() {
		if (!canMove) {
			return;	
		}
		
		if (stayFrozenOnLook) {
			navigator.Stop ();
			if (!foeGlanceCommand.isLookingAround) {
				stayFrozenOnLook = false;
			}
		} else {	
			navigator.Resume ();
		}
		
		if (state == alertState.investigating && isReturning == false && foeDetectionHandler.isAggressive) {
			navigator.speed = baseSpeed * foeDetectionHandler.sprintMultiplier;
		} else {
			navigator.speed = baseSpeed;
		}
		
		if (Vector3.Distance(new Vector3(transform.position.x, 0, transform.position.z), 
				new Vector3(currentDestination.x, 0, currentDestination.z)) < minNodeDistance) {
		     //If close to current destination
			UpdateDestination();
		}
		
		float maxNavDistance = 0.1f;
		if (Vector3.Distance(navigator.transform.position, transform.position) > maxNavDistance) {
			navigator.Stop ();
		} else {
			navigator.Resume();
		}
		
		RotateOrMove();
	}
	
	void RotateOrMove() {
		var lookPos = navigator.transform.position - transform.position;
		if (lookPos.magnitude < 0.001f) {
			return;
		}
		lookPos.y = 0;
		var rotation = Quaternion.LookRotation(lookPos);
		transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * rotationSpeed);
		float maxAngle = 10f;
		if (Quaternion.Angle (transform.rotation, Quaternion.LookRotation(lookPos, Vector3.up)) < maxAngle) {
			transform.position = navigator.transform.position;
			if ((transform.position - navigator.transform.position).magnitude > navigator.speed * Time.deltaTime) {
				transform.position += (navigator.transform.position - transform.position).normalized * navigator.speed * Time.deltaTime;
			} else {
				transform.position = navigator.transform.position;
			}
			navigator.Resume();
		} else {
			navigator.Stop ();
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
		navigator.destination = currentDestination;
	}
	
	public void StartInvestigation(Vector3 destination) {
		if (!enabled) {
			return;
		}
		
		if (!originIsValid) {
			originLocation = transform.position;
		}
		stayFrozenOnLook = false;
		originIsValid = true;
		currentDestination = destination;
		navigator.destination = destination;
		isReturning = false;
		state = alertState.investigating;
		
		foeGlanceCommand.OverrideGlanceCommand();
	}
	
	public void Interact() {
		GetComponentInChildren<Foe_Detection_Handler>().Interact();
	}
}
