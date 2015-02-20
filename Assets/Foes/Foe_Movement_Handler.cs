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
		patroling,
		attentive,
		investigating,
		attacking
	};	
	public alertState state = alertState.patroling;
	
	bool isRotating = true;
	
	void FixedUpdate() {
		if (state == alertState.patroling) {
			Patrol ();
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
}
