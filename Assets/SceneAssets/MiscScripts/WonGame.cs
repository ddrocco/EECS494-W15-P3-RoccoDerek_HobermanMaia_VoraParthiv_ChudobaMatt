using UnityEngine;
using System.Collections;

public class WonGame : MonoBehaviour {
	private bool gameOver;

	void OnTriggerEnter(Collider other) {
		if (other.CompareTag("Player")) {
			GameController.SendPlayerMessage("Mission Complete! Press A to restart", 10);
			QUI.setText("Mission Complete! Press spacebar to restart");
			gameOver = true;
		}
	}
	
	void Update() {
		if (gameOver && Input.GetKeyDown(KeyCode.Space))
		{
			GameController.PlayerDead = true;
		}
	}

}
