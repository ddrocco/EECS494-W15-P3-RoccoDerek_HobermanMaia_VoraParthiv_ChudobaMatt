using UnityEngine;
using System.Collections;

public class EndGameControl : MonoBehaviour {
	public GameObject QCamera;
	
	void OnTriggerEnter(Collider other) {
		if (other.CompareTag("Player") == true) {
			GameController.PlayerDead = true;
			GameController.GameOverMessage =
				"You won!  Congratulations!";
			QCamera.GetComponent<QUI>().showCamera(false);
			QUI.setText("WELL DONE\nYour partner survived. EXCELLENT WORK.");
			return;
		}
	}
}
