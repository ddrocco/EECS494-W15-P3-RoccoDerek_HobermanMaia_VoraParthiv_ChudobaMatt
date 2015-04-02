﻿using UnityEngine;
using System.Collections;

public class Foe_Door_Opener : MonoBehaviour {
	public Animator parentDoorAnimator;
	public GameObject parentDoor;
	int objectsColliding = 0;
	
	void Start() {
		parentDoorAnimator = parentDoor.GetComponentInParent<Animator>();
	}
	
	void OnTriggerEnter(Collider other) {
		//DoorControl obj = parentDoor.GetComponent<DoorControl>();
		if (other.tag == "FoeBody") {
			++objectsColliding;
			/*if (obj.isLocked != obj.expectState) { //test this
				FoeAlertSystem.Alert(transform.position);
			}*/
			if (!parentDoor.GetComponent<DoorControl>().isLocked) {
				if (parentDoor.name == "zDoorHinge") {//parentDoor.transform.rotation.y == 0) { //zDoor
					if (other.transform.position.x < transform.position.x) { //Open south
						parentDoorAnimator.SetBool("openSouth", true);
					} else { //Open North
						parentDoorAnimator.SetBool("openSouth", false);
					}
				} else if (parentDoor.name == "xDoorHinge") { //xDoor
					if (other.transform.position.z < transform.position.z) { //Open east
						parentDoorAnimator.SetBool("openEast", true);
					} else { //Open west
						parentDoorAnimator.SetBool("openEast", false);
					}
				}
				parentDoorAnimator.SetBool("isOpen", true);
			}
			else {
				//other.gameObject.GetComponent.
			}
		}
	}
	
	void OnTriggerExit(Collider other) {
		if (other.tag == "FoeBody") {
			--objectsColliding;
			if (objectsColliding == 0) {
				parentDoorAnimator.SetBool("isOpen", false);
			}
		}
	}
}
