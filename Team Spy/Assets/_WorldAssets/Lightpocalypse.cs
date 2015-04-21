using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Lightpocalypse : MonoBehaviour {
	public Light stanLight;

	void Interact() {
		FindObjectOfType<BrokenLightParent>().Lightpocalypse();
		foreach (GameObject text in GameObject.FindGameObjectsWithTag("EnableOnConnectText")) {
			text.GetComponent<Text>().enabled = true;
		}
		stanLight.enabled = true;
	}
}
