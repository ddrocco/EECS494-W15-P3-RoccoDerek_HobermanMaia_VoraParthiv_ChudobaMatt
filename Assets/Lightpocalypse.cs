using UnityEngine;
using System.Collections;

public class Lightpocalypse : MonoBehaviour {
	void Interact() {
		FindObjectOfType<BrokenLightParent>().Lightpocalypse();
	}
}
