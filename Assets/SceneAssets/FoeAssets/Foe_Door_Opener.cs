using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Foe_Door_Opener : MonoBehaviour {
	public Animator parentDoorAnimator;
	public GameObject parentDoor;
	int objectsColliding = 0;
	
	bool unlockInProgress = false;
	float timeSpentUnlocking = 0;
	float unlockTime = 5f;
	Vector3 unlockerPosition;
	
	void Start() {
		parentDoorAnimator = parentDoor.GetComponentInParent<Animator>();
	}
	
	void Update() {
		if (unlockInProgress) {
			timeSpentUnlocking += Time.deltaTime * objectsColliding;
			if (timeSpentUnlocking >= unlockTime) {
				OpenDoor(unlockerPosition);
				parentDoor.GetComponent<DoorControl>().isLocked = parentDoor.GetComponent<DoorControl>().expectState;
				parentDoor.GetComponent<DoorControl>().QInteractionButton.GetComponent<QInteractionUI>().AlertOff();
			}
		} else {
			timeSpentUnlocking = 0;
		}
	}
	
	void OnTriggerEnter(Collider other) {
		//DoorControl obj = parentDoor.GetComponent<DoorControl>();
		if (other.tag == "FoeBody") {
			++objectsColliding;
			/*if (obj.isLocked != obj.expectState) { //test this
				FoeAlertSystem.Alert(transform.position);
			}*/
			if (!parentDoor.GetComponent<DoorControl>().isLocked) {
				OpenDoor(other.transform.position);
			}
			else {
				unlockInProgress = true;
				unlockerPosition = other.transform.position;
				parentDoor.GetComponent<DoorControl>().QInteractionButton.GetComponent<QInteractionUI>().InUseOn();
			}
		}
	}
	
	void OnTriggerExit(Collider other) {
		if (other.tag == "FoeBody") {
			--objectsColliding;
			if (objectsColliding == 0) {
				parentDoorAnimator.SetBool("isOpen", false);
				unlockInProgress = false;
			}
		}
	}
	
	void OpenDoor(Vector3 guardPosition) {
		if (parentDoor.GetComponent<DoorControl>().direction == DoorDirection.z) {//parentDoor.transform.rotation.y == 0) { //zDoor
			if (guardPosition.x < transform.position.x) { //Open south
				parentDoorAnimator.SetBool("openSouth", true);
			} else { //Open North
				parentDoorAnimator.SetBool("openSouth", false);
			}
		} else if (parentDoor.GetComponent<DoorControl>().direction == DoorDirection.x) { //xDoor
			if (guardPosition.z < transform.position.z) { //Open east
				parentDoorAnimator.SetBool("openEast", true);
			} else { //Open west
				parentDoorAnimator.SetBool("openEast", false);
			}
		} else {
			print ("Door hinge dimension invalid");
		}
		parentDoorAnimator.SetBool("isOpen", true);
	}
}
