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
	Vector3 locationToInvestigate;
	public GameObject objectToInvestigate;
	
	void FixedUpdate() {
		if (state == alertState.patrolling) {
			Patrol ();
		} else if (state == alertState.investigating) {
			locationToInvestigate = objectToInvestigate.transform.position;
			Investigate();
		}
	}
	
	
	void Patrol() {
		Vector3 nodePos = Foe_Route_Node.routeNodeList[defaultPath[currentPathNode]].transform.position;
		nodePos += Vector3.up * (transform.position.y - nodePos.y);
		
		if ((nodePos - transform.position).magnitude < .1f) {
			isRotating = true;
			currentPathNode += 1;
			if (currentPathNode >= defaultPath.Count) {
				currentPathNode = 0;
			}
		}
		
		Vector3 horizVelocity = new Vector3(rigidbody.velocity.x, 0, rigidbody.velocity.z);
		Quaternion targetRotation = Quaternion.LookRotation(nodePos - transform.position);
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
	
	void Investigate() {
		Quaternion targetRotation = Quaternion.LookRotation(locationToInvestigate - transform.position);
		transform.rotation = targetRotation;//Quaternion.Slerp(transform.rotation, targetRotation, 0.1f);
		Vector3 sideDisplacement = transform.rotation * Vector3.right * transform.lossyScale.x / 2;
		
		Vector3 investigationDisplacement = locationToInvestigate - transform.position;
		
		Debug.DrawRay(transform.position + sideDisplacement,
				transform.rotation * Vector3.forward * investigationDisplacement.magnitude, Color.white);
		Debug.DrawRay(transform.position - sideDisplacement,
				transform.rotation * Vector3.forward * investigationDisplacement.magnitude, Color.white);
		Debug.DrawRay(transform.position, investigationDisplacement, Color.blue);
	}
}
