using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Lightpocalypse : MonoBehaviour {
	void Interact() {
		FindObjectOfType<BrokenLightParent>().Lightpocalypse();
		foreach (GameObject text in GameObject.FindGameObjectsWithTag("EnableOnConnectText")) {
			text.GetComponent<Text>().enabled = true;
		}
	}
}
