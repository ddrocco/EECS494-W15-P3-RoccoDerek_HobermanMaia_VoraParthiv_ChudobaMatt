using UnityEngine;
using System.Collections;

public class Laser_Deadly : MonoBehaviour {
	public Vector3 directionStart = Vector3.up;
	public Vector3 directionEnd = Vector3.up;
	public Vector3 directionCurrent = Vector3.up;
	public float movementDuration = 1f;
	public float movementTimer = 0f;
	
	int layerMask;
	LineRenderer laser;
	
	void Start() {
		Color color = Color.red;
		color.a = 0.8f;
		laser = GetComponent<LineRenderer>();
		laser.material.color = color;
		layerMask = (1 << Layerdefs.wall)+ (1 << Layerdefs.stan)
				+ (1 << Layerdefs.floor) + (1 << Layerdefs.prop);
	}
	
	void Update() {
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
				GameController.PlayerDead = true;
				string restartControl = "A";
				if (PlayerController.debugControls) restartControl = "Left Click";
				GameController.GameOverMessage = "You were killed by a laser!\nPress " + restartControl + " to restart the level";
			} else {
				laser.SetPosition(0, transform.position);
				laser.SetPosition(1, hitInfo.point);
				float distance = hitInfo.distance;
				
				GetComponentInChildren<ParticleSystem>().startLifetime = distance / 100f;
				GetComponentInChildren<ParticleSystem>().maxParticles = (int) distance * 10;
			}
		} else {
			laser.SetPosition(0, transform.position);
			laser.SetPosition(1, transform.position + directionCurrent * 100f);
		}
	}
}
