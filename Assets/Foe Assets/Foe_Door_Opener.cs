using UnityEngine;
using System.Collections;

public class Foe_Door_Opener : MonoBehaviour {
	public Animator parentDoorAnimator;
	int objectsColliding = 0;
	
	void OnTriggerEnter(Collider other) {
		if (other.gameObject.layer == Layerdefs.foe) {
			++objectsColliding;
			if (other.transform.position.z < transform.position.z) { //Open east
				parentDoorAnimator.SetBool("openEast", true);
			} else { //Open west
				parentDoorAnimator.SetBool("openEast", false);
			}
			parentDoorAnimator.SetBool("isOpen", true);
		}
	}
	
	void OnTriggerExit(Collider other) {
		if (other.gameObject.layer == Layerdefs.foe) {
			--objectsColliding;
			if (objectsColliding == 0) {
				parentDoorAnimator.SetBool("isOpen", false);
			}
		}
	}
}
