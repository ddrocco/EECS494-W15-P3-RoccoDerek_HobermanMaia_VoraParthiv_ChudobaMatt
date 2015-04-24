using UnityEngine;
using System.Collections;

public class AlarmRelay : MonoBehaviour {
	public float timeAlive = 0;
	public bool shouldDestroy = false;
	
	public void Update() {
		timeAlive += Time.deltaTime;
	}
}
