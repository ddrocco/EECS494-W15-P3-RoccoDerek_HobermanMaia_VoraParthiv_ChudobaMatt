using UnityEngine;
using System.Collections;

public class TaserKill : MonoBehaviour {
	float killRange = 0.25f;
	
	void Update() {
		if (Vector3.Distance(transform.position, FindObjectOfType<PlayerController>().transform.position) < killRange) {
			KillPlayer();
		}
	}
	
	void OnTriggerEnter(Collider other) {
		if (other.GetComponent<PlayerController>() != null) {
			KillPlayer();
		}
	}
	
	void KillPlayer() {
		GameController.PlayerDead = true;
		GameController.GameOverMessage = "You were spotted and tasered by a guard!\nPress A to restart the level";
		QUI.setText("Your agent was spotted and killed by a guard!\nPress A to restart the level");
	}
}
