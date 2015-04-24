using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class LaserBehavior : QInteractable {
	public Vector3 directionStart = Vector3.up;
	public Vector3 directionEnd = Vector3.up;
	public Vector3 directionCurrent = Vector3.up;
	public float movementDuration = 1f;
	public float movementTimer = 0f;
	public bool moving = false;
	
	int layerMask;
	LineRenderer laser;
	
	public override void Start() {
		Color color = Color.red;
		color.a = 0.8f;
		laser = GetComponent<LineRenderer>();
		laser.material.color = color;
		layerMask = (1 << Layerdefs.wall) + (1 << Layerdefs.stan) + (1 << Layerdefs.foe)
				+ (1 << Layerdefs.floor) + (1 << Layerdefs.prop);
		float ratio = Mathf.Abs (movementTimer - movementDuration) / movementDuration;
		directionCurrent = (ratio * directionStart + (1 - ratio) * directionEnd);
		transform.rotation = Quaternion.LookRotation(directionCurrent);
		base.Start();
	}
	
	void Update() {
		qHasFunctionAccess = false;
		if (!qHasDisplayAccess) {
			gameObject.layer = Layerdefs.invisible;
		} else {
			gameObject.layer = Layerdefs.laser;
		}
		if (moving) {
			movementTimer += Time.deltaTime;
			if (movementTimer > movementDuration * 2f) {
				movementTimer -= movementDuration * 2f;
			}
			float ratio = Mathf.Abs (movementTimer - movementDuration) / movementDuration;
			directionCurrent = (ratio * directionStart + (1 - ratio) * directionEnd);
		
			transform.rotation = Quaternion.LookRotation(directionCurrent);
		}
		RaycastHit hitInfo;
		if (Physics.Raycast(transform.position, directionCurrent, out hitInfo, 100f, layerMask)) {
			if (hitInfo.collider.gameObject.layer == Layerdefs.stan) {
				GetComponentInParent<LaserAlertSystem>().SignalAlarm(new Vector3(hitInfo.point.x, 0, hitInfo.point.z),
				                                                     this.gameObject);
			}
			laser.SetPosition(0, transform.position);
			laser.SetPosition(1, hitInfo.point);
			float distance = hitInfo.distance;
			
			if (GetComponentInChildren<ParticleSystem>() != null) {
				GetComponentInChildren<ParticleSystem>().startLifetime = distance / 100f;
				GetComponentInChildren<ParticleSystem>().maxParticles = (int) distance * 10;
			}
		} else {
			laser.SetPosition(0, transform.position);
			laser.SetPosition(1, transform.position + directionCurrent * 100f);
		}
	}
	
	public override void Trigger() {
		return;
	}
	
	public override Sprite GetSprite() {
		return ButtonSpriteDefinitions.main.Laser;
	}
	
	public override GameObject GetStanVisibleTag() {
		return ObjectPrefabDefinitions.main.TagViewLaser;
	}
	
	public override void enableButtonView() {
		QInteractionButton.GetComponent<Image>().enabled = true;
		GetComponentInChildren<LineRenderer>().enabled = true;
	}
}
