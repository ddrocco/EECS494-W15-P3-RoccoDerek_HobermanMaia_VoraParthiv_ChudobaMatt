using UnityEngine;
using System.Collections;

public class Foe_Door_Opener : MonoBehaviour {
	public Animator parentDoorAnimator;
	int objectsColliding = 0;
	
	void OnTriggerEnter(Collider other) {
		++objectsColliding;
		parentDoorAnimator.SetBool("isOpen", true);
	}
	
	void OnTriggerExit(Collider other) {
		--objectsColliding;
		if (objectsColliding == 0) {
			parentDoorAnimator.SetBool("isOpen", false);
		}
	}
}
