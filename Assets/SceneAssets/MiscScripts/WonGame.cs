using UnityEngine;
using System.Collections;

public class WonGame : MonoBehaviour {
	private bool gameOver;

	void OnTriggerEnter(Collider other) {
		if (other.CompareTag("Player")) {
			GameController.SendPlayerMessage("YOU WON! Press spacebar to restart", 5);
			QUI.setText("YOU WON! Press spacebar to restart");
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
