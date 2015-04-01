using UnityEngine;
using System.Collections;

public class AlertHub : QInteractable {
	public bool isActive = true;
	public GameObject mapCover1;
	public GameObject mapCover2;
	
	//Juice:
	/*bool isSounding = false;
	float soundingTimer = 0f;
	float animationSpeed = 0.5f;*/
	
	public void Signal() {
		if (isActive) {
			//isSounding = true;
			print ("Signal!");
			QCamera.GetComponent<QCameraControl>().AlertOn();
		}
		//Raise alarm!
	}
	
	void Update() {
		if (!isActive) {
			QCamera.GetComponent<QCameraControl>().AlertOff();
		}
		
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
	}
	
	public override Sprite GetSprite() {
		if (isActive) {
			return ButtonSpriteDefinitions.main.alarmSounding;
		} else {
			return ButtonSpriteDefinitions.main.alarmSilent;
		}
	}
	
	
}
