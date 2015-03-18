using UnityEngine;
using System.Collections;

public class Laser_Deadly_Static : MonoBehaviour {
	public bool alertTimerSet;
	public int alertTimer = 0;
	public int timeTillAlert = 100;
	
	void Start() {
		Color color = Color.red;
		color.a = 0.8f;
		GetComponent<MeshRenderer>().material.color = color;
	}
	
	void FixedUpdate () {
		if (alertTimerSet) {
			alertTimer++;
		}
		if (alertTimer > timeTillAlert) {
			FoeAlertSystem.Alert(transform.position);
			alertTimer = 0;
			alertTimerSet = false;
		}
	}
	
	void OnTriggerEnter(Collider other) {
		if (other.gameObject.layer == Layerdefs.stan) {
			GameController.PlayerDead = true;
			string restartControl = "A";
			if (PlayerController.debugControls) restartControl = "Left Click";
			GameController.GameOverMessage = "You were killed by a laser!\nPress " + restartControl + " to restart the level";
		}
	}
}
