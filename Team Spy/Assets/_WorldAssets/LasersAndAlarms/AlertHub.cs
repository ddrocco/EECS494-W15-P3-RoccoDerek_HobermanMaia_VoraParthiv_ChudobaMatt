using UnityEngine;
using System.Collections;

public class AlertHub : QInteractable {
	public bool isActive = true;
	public static bool isSounding = false;
	public bool wasSounding = false;
	public int lockdownGroup = 1;
	public QCameraControl camControl;
	public static bool guardOnAlert = false;
	
	public override void Start() {
		guardOnAlert = false;
		isSounding = false;
		base.Start();
	}
	
	public void Signal(Vector3 detectionLocation, GameObject sourceObject,
	                   LaserAlertSystem lasers = null,
	                   ExternalAlertSystem extSystem = null) {
	    if (guardOnAlert) {
			return;
	    }
		if (extSystem) {
			extSystem.RemoveAllActiveSignals();
		}
		FoeAlertSystem.Alert(detectionLocation, isPlayer: true);
		guardOnAlert = true;
		sourceObject.GetComponent<QInteractable>().QInteractionButton.GetComponent<QInteractionUI>().AlertOn(); //find right one!
		if (sourceObject.GetComponent<LaserBehavior>()) {
			lasers.heardALaser = true;
		}
		if (sourceObject.GetComponent<CameraControl>()) {
			sourceObject.GetComponent<CameraControl>().Offline = true;
		}
		camControl.AlertOn();
		SetLockdownState(true);
	}
	
	void Update() {
		if (camControl == null) {
			camControl = FindObjectOfType<QCameraControl>();
		}
		if (!isSounding) {
			if (wasSounding) {
				SetLockdownState(false);
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
