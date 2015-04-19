using UnityEngine;
using System.Collections;

public class DoorSoundOnOpen : MonoBehaviour {
	bool soundHasPlayed = false;
	
	void Update() {
		if (soundHasPlayed) {
			return;
		}
		if (GetComponent<Animator>().GetBool("isOpen")) {
			AudioSource.PlayClipAtPoint(AudioDefinitions.main.Level5Script, transform.position);
			soundHasPlayed = true;
		}
	}

}
