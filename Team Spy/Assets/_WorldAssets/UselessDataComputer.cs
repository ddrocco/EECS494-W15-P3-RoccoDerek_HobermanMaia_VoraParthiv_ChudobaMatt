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
		switch(uselessDataCollected) {
			case 1:
				GameController.SendPlayerMessage("You got some data!  Nice work!", 7);
				break;
			case 2:
				GameController.SendPlayerMessage("You got some more data!", 7);
				break;
			case 3:
				GameController.SendPlayerMessage("More data!  You're really cleaning up!  You deserve a medal!", 7);
				break;
			case 4:
				GameController.SendPlayerMessage("SO much data!", 7);
				break;
			case 5:
				GameController.SendPlayerMessage("No one has more data than you!!!", 7);
				break;
			case 6:
				GameController.SendPlayerMessage("...You know all those monitors were connected to the same computer, right?", 7);
				break;
		}
	}
}
