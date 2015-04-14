using UnityEngine;
using System.Collections;

public class EndGameControl : MonoBehaviour {
	public GameObject QCamera;


	void Start(){
		QCamera = GameObject.Find ("QCamera");
	}
	void OnTriggerEnter(Collider other) {
		if (other.tag == "Player") {
			GameController.PlayerWon = true;
			GameController.GameOverMessage =
				"Mission Success!";
			QCamera.GetComponent<QUI>().showCamera(false);
			QUI.setText("Mission Success\nWell done!");
			Time.timeScale = 0;
			return;
		}
	}
}
