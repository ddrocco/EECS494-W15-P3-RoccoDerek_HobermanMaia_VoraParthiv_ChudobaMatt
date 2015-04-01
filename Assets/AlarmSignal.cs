using UnityEngine;
using System.Collections;

public class AlarmSignal : MonoBehaviour {
	public float timeAlive = 0;
	public bool shouldDestroy = false;
	
	public Vector3 detectionLocation;
	
	void Update() {
		timeAlive += 3*Time.deltaTime/4f;
	}
}
