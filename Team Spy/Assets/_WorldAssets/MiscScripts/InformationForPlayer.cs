using UnityEngine;
using System.Collections;

public class InformationForPlayer : QInteractable {
	static int numCollected = 0;
	public string message;
	public string QMessage;
	
	public bool read = false;
	
	public override void Start() {
		base.Start();
	}
	
	public void Interact() {
		GameController.SendPlayerMessage(message, 2);
		if (!read) {
			read = true;
			++numCollected;
			QUI.setText("Partner found document!  Total: " + numCollected);
		}
		QUI.setText(message);
	}
	
	public override void Trigger() {
		return;
	}
	
	public override Sprite GetSprite() {
		return ButtonSpriteDefinitions.main.Files;
	}
}
