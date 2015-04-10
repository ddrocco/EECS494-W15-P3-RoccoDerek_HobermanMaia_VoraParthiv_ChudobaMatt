using UnityEngine;
using System.Collections;

public class WonGame : MonoBehaviour {
	private bool gameOver;

	void OnTriggerEnter(Collider other) {
		if (other.CompareTag("Player")) {
			GameController.PlayerWon = true;
			QUI.setText("Mission Complete! Press spacebar to continue");
			gameOver = true;
		}
	}

}
