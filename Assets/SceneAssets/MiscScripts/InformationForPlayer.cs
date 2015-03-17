using UnityEngine;
using System.Collections;

public class InformationForPlayer : MonoBehaviour {
	public string message;
	
	public void Interact() {
		GameController.SendPlayerMessage(message, 5);
	}
}
