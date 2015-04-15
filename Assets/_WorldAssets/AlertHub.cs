using UnityEngine;
using System.Collections;

public class AlertHub : QInteractable {
	public bool isActive = true;
	public static bool isSounding = false;
	public bool wasSounding = false;
	public int lockdownGroup = 1;
	private QCameraControl camControl;
	
	public override void Start() {
		camControl = FindObjectOfType<QCameraControl>();
	}
	
	public void Signal(Vector3 detectionLocation) {
		if (isActive) {
			FoeAlertSystem.Alert(detectionLocation, isPlayer: true);
			QInteractionButton.GetComponent<QInteractionUI>().AlertOn();
			camControl.AlertOn();
			SetLockdownState(true);
		}
	}
	
	void Update() {
		if (!isSounding) {
			if (wasSounding) {
				SetLockdownState(false);
				camControl.AlertOff();
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
