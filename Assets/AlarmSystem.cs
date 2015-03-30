using UnityEngine;
using System.Collections;

public class AlarmSystem : QInteractable {
	public bool isActive = true;
	
	public void Signal() {
		//Raise alarm!
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
