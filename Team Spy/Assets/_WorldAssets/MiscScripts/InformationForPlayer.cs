using UnityEngine;
using System.Collections;

public class InformationForPlayer : QInteractable {
	public string message;
	public string QMessage;
	public bool read = false;
	
	public override void Start() {
		base.Start();
	}
	
	void Update() {
		qHasFunctionAccess = false;
	}
	
	public void Interact() {
		GameController.SendPlayerMessage(message, 2);
		if (!read) {
			read = true;
		}
		QUI.setText(message, objective: false);
	}
	
	public override void Trigger() {
		return;
	}
	
	public override Sprite GetSprite() {
		return ButtonSpriteDefinitions.main.Files;
	}
}
