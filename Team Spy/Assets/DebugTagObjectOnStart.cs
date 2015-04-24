using UnityEngine;
using System.Collections;

public class DebugTagObjectOnStart : MonoBehaviour {
	QInteractable obj;
	
	void Start() {
		obj = GetComponent<QInteractable>();
	}

	void Update () {
		if (!obj.displayIsActive) {
			obj.Tag ();
		}
	}
}
