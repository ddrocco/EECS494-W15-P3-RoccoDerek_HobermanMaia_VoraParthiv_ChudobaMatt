using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Foe_Movement_Handler : MonoBehaviour {
	public List<int> defaultPath;
	public List<int> alertNodes; //Const?
	
	public int currentPathNode = 0;
	public int currentPathAlertNode;
	
	float speed = 1.0f;
	
	public enum alertState{
		patrolling,	//Nominal, walking around
		attentive,	//Still walking around; glances about more often.
		investigating,	//Moves to a location, then goes back to patrolling.
		attacking
	};	
	public alertState state = alertState.patrolling;
	
	//Patrolling variables
	bool isRotating = true;
	
	//Investigating variables
	public GameObject objectToInvestigate;
	Vector3 destinationLocation;
	Vector3 originLocation;
	bool isReturning;
	
	void Start() {
		if (state == alertState.investigating) {
			StartInvestigation();
		}
	}

	void FixedUpdate() {
		if (state == alertState.patrolling) {
			Patrol ();
		} else if (state == alertState.investigating) {
			CheckInvestigationState();
		}
	}
	
	void StartInvestigation() {
		originLocation = transform.position;
		destinationLocation = objectToInvestigate.transform.position;
		GetComponent<NavMeshAgent>().destination = destinationLocation;
		GetComponent<NavMeshAgent>().enabled = true;
		isReturning = false;
		rigidbody.isKinematic = true;
		rigidbody.useGravity = false;
	}
	
	void CheckInvestigationState() {
		print (destinationLocation);
		if (!isReturning) {
			if ((new Vector3(transform.position.x, 0, transform.position.z)
					- new Vector3(destinationLocation.x, 0, destinationLocation.z)).magnitude < 0.1f) {
				isReturning = true;
				GetComponent<NavMeshAgent>().destination = originLocation;	
			}
		} else {
			if ((new Vector3(transform.position.x, 0, transform.position.z)
			     - new Vector3(originLocation.x, 0, originLocation.z)).magnitude < 0.1f) {
				GetComponent<NavMeshAgent>().enabled = false;
				rigidbody.isKinematic = false;
				rigidbody.useGravity = true;
			}
		}
	}
	
	void Patrol() {
		Vector3 destination = Foe_Route_Node.routeNodeList[defaultPath[currentPathNode]].transform.position;
		destination += Vector3.up * (transform.position.y - destination.y);
		
		if ((destination - transform.position).magnitude < .1f) {
			isRotating = true;
			currentPathNode += 1;
			if (currentPathNode >= defaultPath.Count) {
				currentPathNode = 0;
			}
		}
		
		Vector3 horizVelocity = new Vector3(rigidbody.velocity.x, 0, rigidbody.velocity.z);
		
		Quaternion targetRotation = Quaternion.LookRotation(destination - transform.position);
		transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 0.1f);
		
		if (isRotating) {
			horizVelocity -= horizVelocity.normalized * speed;
			if (Quaternion.Angle(transform.rotation, targetRotation) < 1f) {
				isRotating = false;
			}
		} else {
			horizVelocity += transform.rotation * Vector3.forward * speed;
		}
		
		if (horizVelocity.magnitude > speed) {
			horizVelocity = horizVelocity.normalized * speed;
		}
		rigidbody.velocity = new Vector3(horizVelocity.x, rigidbody.velocity.y, horizVelocity.z);
	}
}
