using UnityEngine;
using System.Collections;

public class WonGame : MonoBehaviour {

	void OnTriggerEnter(Collider other) {
		if (other.CompareTag("Player")) {
			GameController.SendPlayerMessage("YOU WON! Press spacebar to restart", 5);
			QUI.setText("YOU WON! Press spacebar to restart");
		}
	}
	
	void Update() {
		if (Input.GetKeyDown(KeyCode.Space))
		{
			GameController.PlayerDead = true;
		}
	}

}
