using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PolyLaserParent : QInteractable {
	public GameObject laserChildPrefab;
	public List<Vector3> origins, directionStarts, directionEnds;
	
	[HideInInspector]
	public List<Vector3> directionCurrents;
	[HideInInspector]
	public List<LineRenderer> lasers;
	
	public float movementDuration = 1f;
	[HideInInspector]
	public float movementTimer = 0f;
	
	[HideInInspector]
	public int layerMask;
	
	public bool alertTimerSet = false;
	public float alertWait = 4f;
	[HideInInspector]
	public float alertTimer = 0f;
	Vector3 alertPosition;
	
	void Awake() {
		Color color = Color.red;
		color.a = 0.8f;
		lasers = new List<LineRenderer>();
		foreach (Vector3 origin in origins) {
			GameObject laser = Instantiate(laserChildPrefab) as GameObject;
			laser.GetComponent<LineRenderer>().material.color = color;
			laser.transform.parent = transform;
			laser.transform.localPosition = origin;
			lasers.Add (laser.GetComponent<LineRenderer>());
			
			directionCurrents.Add (Vector3.zero);
		}
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
		//transform.rotation = Quaternion.LookRotation(directionCurrent);
		for (int i = 0; i < lasers.Count; ++i) {
			directionCurrents[i] = (ratio * directionStarts[i] + (1 - ratio) * directionEnds[i]);
			RaycastHit hitInfo;
			if (Physics.Raycast(origins[i] + transform.position, directionCurrents[i], out hitInfo, 100f, layerMask)) {
				if (hitInfo.collider.gameObject.layer == Layerdefs.stan) {
					GetComponentInParent<LaserRoomAlertSystem>().Activate(this);
					alertTimerSet = true;
					alertPosition = new Vector3(hitInfo.point.x, 0, hitInfo.point.z);
				}
				lasers[i].SetPosition(0, origins[i] + transform.position);
				lasers[i].SetPosition(1, hitInfo.point);
				float distance = hitInfo.distance;
				
				GetComponentInChildren<ParticleSystem>().startLifetime = distance / 100f;
				GetComponentInChildren<ParticleSystem>().maxParticles = (int) distance * 10;
				
				lasers[i].transform.rotation = Quaternion.LookRotation(hitInfo.point - origins[i] - transform.position);
			} else {
				lasers[i].SetPosition(0, origins[i] + transform.position);
				lasers[i].SetPosition(1, origins[i] + transform.position + directionCurrents[i] * 100f);
				lasers[i].transform.rotation = Quaternion.LookRotation(directionCurrents[i]);
			}
		}
	}
	
	public override void Trigger() {
		alertTimer = 0;
		alertTimerSet = false;
	}
	
	public override Sprite GetSprite() {
		return ButtonSpriteDefinitions.main.poly_laser;
	}
	
	public override void Tag() {
		ParticleSystemRenderer[] systems = GetComponentsInChildren<ParticleSystemRenderer>();
		foreach (ParticleSystemRenderer system in systems) {
			system.enabled = true;
		}
	}
	
	public override void UnTag() {
		ParticleSystemRenderer[] systems = GetComponentsInChildren<ParticleSystemRenderer>();
		foreach (ParticleSystemRenderer system in systems) {
			system.enabled = false;
		}
	}
}
