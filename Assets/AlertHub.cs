using UnityEngine;
using System.Collections;

public class AlertHub : QInteractable {
	public bool isActive = true;
	public bool isSounding = false;
	
	//Juice:
	/*float soundingTimer = 0f;
	float animationSpeed = 0.5f;*/
	
	public void Signal(Vector3 detectionLocation) {
		if (isActive) {
			print ("INTRUDER DETECTED at " + detectionLocation + "!");
			FoeAlertSystem.Alert(detectionLocation);
			QInteractionButton.GetComponent<QInteractionUI>().AlertOn();
			isSounding = true;
		}
		//Raise alarm!
	}
	
	void Update() {
		/*if (isSounding) {
			SoundAnimation();
		}*/
	}
	
	void SoundAnimation() {
		/*if (soundingTimer > animationSpeed) {
			soundingTimer -= animationSpeed;
			if (QInteractionButton.transform.localEulerAngles != new Vector3(0, 45, 0)) {
				QInteractionButton.transform.localEulerAngles = new Vector3(0, 45, 0);
				if (QInteractionButton.transform.localScale != new Vector3(1, 2, 1)) {
					QInteractionButton.transform.localScale = new Vector3(1, 2, 1);
				} else {
					QInteractionButton.transform.localScale = new Vector3(2, 1, 1);
				}
			} else {
				QInteractionButton.transform.localEulerAngles = new Vector3(0, -45, 0);
			}
		}*/
	}
	
	public override void Trigger() {
		isActive = !isActive;
		isSounding = false;
	}
	
	public override Sprite GetSprite() {
		if (isSounding) {
			return ButtonSpriteDefinitions.main.alarmSounding;
		} else if (isActive) {
			return ButtonSpriteDefinitions.main.alarmEnabled;
		} else {
			return ButtonSpriteDefinitions.main.alarmSilent;
		}
	}
}
