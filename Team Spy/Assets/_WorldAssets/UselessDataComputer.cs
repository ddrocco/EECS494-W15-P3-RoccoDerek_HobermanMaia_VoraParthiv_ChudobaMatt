using UnityEngine;
using System.Collections;

public class UselessDataComputer : QInteractable {
	static int uselessDataCollected = 0;

	public override void Start() {
		base.Start();
	}
	
	void Update() {
		qHasFunctionAccess = false;
	}
	
	public override void Trigger() {
		return;
	}
	
	public override Sprite GetSprite() {
		return ButtonSpriteDefinitions.main.Computer;
	}

	public void Interact() {
		gameObject.tag = "Untagged";
		++uselessDataCollected;
		string message = "";
		switch(uselessDataCollected) {
			case 1:
				message = "You got some data!  Nice work!";
				break;
			case 2:
				message = "You got some more data!";
				break;
			case 3:
				message = "More data!  You're really cleaning up!  You deserve a medal!";
				break;
			case 4:
				message = "SO much data!";
				break;
			case 5:
				message = "No one has more data than you!!!";
				break;
			case 6:
				message = "...You know all those monitors were connected to the same computer, right?";
				break;
		}
		GameController.SendPlayerMessage(message, 7);
		QUI.setText(message, objective: false);
	}
}
