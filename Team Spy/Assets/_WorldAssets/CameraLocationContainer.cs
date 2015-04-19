using UnityEngine;
using System.Collections;

public class CameraLocationContainer : MonoBehaviour {
	void Awake() {
		transform.parent = FindObjectOfType<QCameraControl>().transform.parent;
	}
}
