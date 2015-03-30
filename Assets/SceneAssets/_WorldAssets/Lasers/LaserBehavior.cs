using UnityEngine;
using System.Collections;

public class LaserBehavior : QInteractable {
	public Vector3 directionStart = Vector3.up;
	public Vector3 directionEnd = Vector3.up;
	public Vector3 directionCurrent = Vector3.up;
	public float movementDuration = 1f;
	public float movementTimer = 0f;
	
	int layerMask;
	LineRenderer laser;
	
	public bool alertTimerSet = false;
	public float alertWait = 4f;
	public float alertTimer = 0f;
	Vector3 alertPosition;
	
	public override void Start() {
		Color color = Color.red;
		color.a = 0.8f;
		laser = GetComponent<LineRenderer>();
		laser.material.color = color;
		layerMask = (1 << Layerdefs.wall) + (1 << Layerdefs.stan) + (1 << Layerdefs.foe)
				+ (1 << Layerdefs.floor) + (1 << Layerdefs.prop);
		base.Start();
	}
	
	void Update() {
		if (alertTimerSet) {
			alertTimer += Time.deltaTime;
			if (alertTimer >= alertWait) {
				FoeAlertSystem.Alert(alertPosition);
				alertTimer = 0;
				alertTimerSet = false;
			}
		}
		movementTimer += Time.deltaTime;
		if (movementTimer > movementDuration * 2f) {
			movementTimer -= movementDuration * 2f;
		}
		float ratio = Mathf.Abs (movementTimer - movementDuration) / movementDuration;
		directionCurrent = (ratio * directionStart + (1 - ratio) * directionEnd);
	
		transform.rotation = Quaternion.LookRotation(directionCurrent);
		
		RaycastHit hitInfo;
		if (Physics.Raycast(transform.position, directionCurrent, out hitInfo, 100f, layerMask)) {
			if (hitInfo.collider.gameObject.layer == Layerdefs.stan) {
				GetComponentInParent<LaserRoomAlertSystem>().SignalAlarm();
				alertTimerSet = true;
				alertPosition = new Vector3(hitInfo.point.x, 0, hitInfo.point.z);
			}
			laser.SetPosition(0, transform.position);
			laser.SetPosition(1, hitInfo.point);
			float distance = hitInfo.distance;
			
			GetComponentInChildren<ParticleSystem>().startLifetime = distance / 100f;
			GetComponentInChildren<ParticleSystem>().maxParticles = (int) distance * 10;
		} else {
			laser.SetPosition(0, transform.position);
			laser.SetPosition(1, transform.position + directionCurrent * 100f);
		}
	}
	
	public override void Trigger() {
		alertTimer = 0;
		alertTimerSet = false;
	}
	
	public override Sprite GetSprite() {
		return ButtonSpriteDefinitions.main.laser;
	}
}
