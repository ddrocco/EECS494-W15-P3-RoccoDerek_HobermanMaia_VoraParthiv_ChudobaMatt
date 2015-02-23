using UnityEngine;
using System.Collections;

public class Laser_Deadly : MonoBehaviour {
	Vector3 direction;
	
	public Vector3 startDirection = Vector3.forward;
	public Vector3 endDirection = Vector3.forward;
	public float fullCycleTime = 1f;
	float timer;
	
	void Start() {
		Color color = Color.red;
		color.a = 0.8f;
		GetComponent<MeshRenderer>().material.color = color;
		
		startDirection.Normalize();
		endDirection.Normalize();
		direction = startDirection;
		timer = 0;
	}
	
	void Update () {		
		direction.Normalize();
		
		RaycastHit hitInfo;
		if (Physics.Raycast(transform.parent.position,
		                    direction, out hitInfo, 100f, (1 << Layerdefs.wall)
		                    + (1 << Layerdefs.floor))) {
			if (hitInfo.transform.gameObject.layer == Layerdefs.player) {
				print ("Game over!");
			}
//			print (hitInfo.distance);
//			print (hitInfo.collider.gameObject.layer);
			transform.localScale = new Vector3(
				transform.localScale.x,
				hitInfo.distance / 2f,
				transform.localScale.z);
			transform.localPosition = (hitInfo.distance / 2) * direction;
		} else {
			transform.localScale = new Vector3(
				transform.localScale.x,
				100f,
				transform.localScale.z);
			transform.localPosition = 100f * direction;
		}
		transform.LookAt(transform.position + direction, Vector3.up);
		transform.Rotate (90f, 0, 0);
		
		if (startDirection != endDirection) {
			UpdateDirection();
		}
	}
	
	void UpdateDirection() {
		timer += Time.deltaTime;
		if (timer >= fullCycleTime) {
			timer -= fullCycleTime;
		}
		
		float ratio = timer / fullCycleTime;
		
		direction = startDirection * (0.5f + 0.5f * Mathf.Cos (ratio * 2 * Mathf.PI))
				+ endDirection * (0.5f - 0.5f * Mathf.Cos (ratio * 2 * Mathf.PI));
	}

	void OnTriggerEnter(Collider other) {
		if (other.gameObject.layer == Layerdefs.player && other.tag == "Player") {
			GameController.PlayerDead = true;
			string restartControl = "A";
			if (PlayerController.debugControls) restartControl = "Left Click";
			GameController.GameOverMessage = "You were killed by a laser!\nPress " + restartControl + " to restart the level";
		}
	}
}
