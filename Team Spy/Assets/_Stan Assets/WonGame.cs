using UnityEngine;
using System.Collections;

public class WonGame : MonoBehaviour {
	void OnTriggerEnter(Collider other) {
		if (other.CompareTag("Player")) {
			GameController.PlayerWon = true;
			QUI.setText("Mission Complete! Press spacebar to continue", objective: true);
		}
	}
}
