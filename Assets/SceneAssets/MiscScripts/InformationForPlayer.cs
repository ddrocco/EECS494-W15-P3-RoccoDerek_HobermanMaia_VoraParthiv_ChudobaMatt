using UnityEngine;
using System.Collections;

public class InformationForPlayer : MonoBehaviour {
	public string message;
	public string QMessage;
	
	public void Interact() {
		GameController.SendPlayerMessage(message, 2);
		//QUI.setText(QMessage);
	}
}
