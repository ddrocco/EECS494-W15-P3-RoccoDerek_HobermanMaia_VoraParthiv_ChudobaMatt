﻿using UnityEngine;
using System.Collections;

public class AlertHub : QInteractable {
	public bool isActive = true;
	public static bool isSounding = false;
	public bool wasSounding = false;
	public int lockdownGroup = 1;
	
	public void Signal(Vector3 detectionLocation) {
		if (isActive) {
			//print ("INTRUDER DETECTED at " + detectionLocation + "!");
			FoeAlertSystem.Alert(detectionLocation);
			QInteractionButton.GetComponent<QInteractionUI>().AlertOn();
			FindObjectOfType<QCameraControl>().AlertOn();
			SetLockdownState(true);
		}
	}
	
	void Update() {
		if (!isSounding) {
			if (wasSounding) {
				SetLockdownState(false);
				FindObjectOfType<QCameraControl>().AlertOff();
				wasSounding = false;
			}
		} else {
			if (!wasSounding) {
				wasSounding = true;
			}
		}
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
	
	void SetLockdownState(bool newLockdownState) {
		foreach(DoorControl door in FindObjectsOfType<DoorControl>()) {
			door.SetLockState(lockdownGroup, newLockdownState);
		}
	}
}