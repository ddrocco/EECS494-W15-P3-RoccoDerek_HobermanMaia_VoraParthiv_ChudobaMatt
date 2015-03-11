using UnityEngine;
using System.Collections;

public class CeilingControl : MonoBehaviour {

	void Start() {
		transform.GetComponent<MeshRenderer>().enabled = true;
	}
	
	void OnDestroy() {
		transform.GetComponent<MeshRenderer>().enabled = false;
	}
}
