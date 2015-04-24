using UnityEngine;
using System.Collections;

public class CeilingControl : MonoBehaviour {
	//Turns ceiling meshrenderers off for debugging when game isn't playing

	void Start() {
		transform.GetComponent<MeshRenderer>().enabled = true;
	}
	
	void OnDestroy() {
		transform.GetComponent<MeshRenderer>().enabled = false;
	}
}
