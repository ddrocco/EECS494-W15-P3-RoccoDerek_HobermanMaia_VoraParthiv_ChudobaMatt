using UnityEngine;
using System.Collections;

public class InformationForPlayer : MonoBehaviour {
	static int numCollected = 0;
	public string message;
	public string QMessage;
	
	public bool read = false;
	
	public void Interact() {
		GameController.SendPlayerMessage(message, 2);
		if (!read) {
			read = true;
			++numCollected;
			QUI.setText("Partner found document!  Total: " + numCollected);
		}
		//QUI.setText(QMessage);
	}
}
