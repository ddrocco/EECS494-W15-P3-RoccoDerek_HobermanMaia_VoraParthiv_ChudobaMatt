using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class World_Foe_Coordinator : MonoBehaviour {
	public List<GameObject> foesInCollision = new List<GameObject>();
	public float numRequired = 2;
	public float speed;
	
	void Start() {
		
	}
	
	void OnTriggerEnter(Collider other) {
		if (other.gameObject.tag == "FoeBody") {
			foesInCollision.Add(other.gameObject);
			if (foesInCollision.Count < numRequired && 
					other.GetComponent<Foe_Movement_Handler>().state
			    	== Foe_Movement_Handler.alertState.patrolling) {
				other.GetComponent<NavMeshAgent>().speed = 0;
				other.GetComponentInChildren<Foe_Glance_Command>().ReceiveGlanceCommand(10, 3f, -135f, 0);
			} else {
				foreach (GameObject foe in foesInCollision) {
					foe.GetComponent<NavMeshAgent>().speed = foe.GetComponent<Foe_Movement_Handler>().speed;
				}
			}
		}
	}
	
	public void ReleaseCommunicatingGuards() {
		foreach (GameObject foe in foesInCollision) {
			foe.GetComponent<NavMeshAgent>().speed = foe.GetComponent<Foe_Movement_Handler>().speed;
		}
	}
	
	void OnTriggerExit(Collider other) {
		if (other.gameObject.tag == "FoeBody") {
			foesInCollision.Remove(other.gameObject);
		}
	}
}
