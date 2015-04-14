using UnityEngine;
using System.Collections;

public class EndGameControl : MonoBehaviour {	
	void OnTriggerEnter(Collider other) {
		if (other.tag == "Player") {
			GameController.PlayerWon = true;
			GameController.GameOverMessage =
				"Mission Success!";
			FindObjectOfType<QUI>().showCamera(false);
			QUI.setText("Mission Success\nWell done!");
			Time.timeScale = 0;
			return;
		}
	}
}
