using UnityEngine;
using System.Collections;

public class TaserKill : MonoBehaviour {

	void OnTriggerEnter(Collider other) {
		if (other.GetComponent<PlayerController>() != null) {
			GameController.PlayerDead = true;
			GameController.GameOverMessage = "You were spotted and tasered by a guard!\nPress A to restart the level";
			QUI.setText("Your agent was spotted and killed by a guard!\nPress A to restart the level");
		}
	}
}
