using UnityEngine;
using System.Collections;

public class AlarmSignal : MonoBehaviour {
	public float timeAlive = 0;
	public bool shouldDestroy = false;
	
	void Update() {
		timeAlive += Time.deltaTime;
	}
}
