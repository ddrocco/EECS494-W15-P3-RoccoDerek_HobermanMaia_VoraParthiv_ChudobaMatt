using UnityEngine;
using System.Collections;

public class AlertHub : QInteractable {
	public bool isActive = true;
	public static bool isSounding = false;
	public static bool heardALaser = false;
	public bool wasSounding = false;
	public int lockdownGroup = 1;
	public QCameraControl camControl;
	
	public void Signal(Vector3 detectionLocation, GameObject sourceObject,
	                   ExternalAlertSystem extSystem = null) {
		//print ("received alert: " + sourceObject.name);
		//if (isActive) {
			print ("received alert: " + sourceObject.name);
			if (extSystem) {
				extSystem.RemoveAllActiveSignals();
			}
			FoeAlertSystem.Alert(detectionLocation, isPlayer: true);
			sourceObject.GetComponent<QInteractable>().QInteractionButton.GetComponent<QInteractionUI>().AlertOn(); //find right one!
			if (sourceObject.GetComponent<LaserBehavior>()) {
				heardALaser = true;
			}
			if (sourceObject.GetComponent<CameraControl>()) {
				sourceObject.GetComponent<CameraControl>().Offline = true;
			}
			camControl.AlertOn();
			SetLockdownState(true);
		//}
	}
	
	void Update() {
		if (camControl == null) {
			camControl = FindObjectOfType<QCameraControl>();
		}
		if (!isSounding) {
			if (wasSounding) {
				SetLockdownState(false);
				//camControl.AlertOff();
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
		return ButtonSpriteDefinitions.main.Alarm;
	}
	
	void SetLockdownState(bool newLockdownState) {
		foreach(DoorControl door in FindObjectsOfType<DoorControl>()) {
			door.SetLockState(lockdownGroup, newLockdownState);
		}
	}
	
	public override GameObject GetStanVisibleTag() {
		return null;
	}
	
	public override void Tag() {
		return;
	}
	
	public override void UnTag() {
		return;
	}
}
