using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public enum DoorDirection {
	x,
	z
};

public class DoorControl : QInteractable {
	public bool isLocked;
	public bool expectState;
	public DoorDirection direction;
	
	//Lockdown (during alerts)
	public int lockGroup = 0;
	bool lockGroupActive = false;
	
	//Opening (guards) and closing (guards and Stan) detection:
	Vector3 basePosition, baseForward, baseRight;
	int cullGuards, cullStan;
	float openDistance = 0.4f;
	float closeDistance = 3f;
	
	RaycastHit hitInfo;
	List<Ray> raysRight = new List<Ray>();
	List<Ray> raysLeft = new List<Ray>();
	List<Ray> raysClose = new List<Ray>();
	
	//Guards unlocking
	float timeToUnlock = 5f;
	float timeUntilUnlocked = 5f;
	
	void Awake () {		
		basePosition = transform.position + new Vector3(0, -0.5f, 0);
		baseForward = transform.forward / 1.5f;
		baseRight = transform.right;
		cullGuards = (1 << Layerdefs.foe);
		cullStan = (1 << Layerdefs.stan);
		
		raysRight.Add(new Ray(basePosition,					baseRight * openDistance));
		raysRight.Add(new Ray(basePosition + baseForward,	baseRight * openDistance));
		raysRight.Add(new Ray(basePosition - baseForward,	baseRight * openDistance));
		raysLeft.Add(new Ray(basePosition,					-baseRight * openDistance));
		raysLeft.Add(new Ray(basePosition + baseForward,	-baseRight * openDistance));
		raysLeft.Add(new Ray(basePosition - baseForward,	-baseRight * openDistance));
		
		raysClose.Add(new Ray(basePosition - baseRight * closeDistance,						baseRight * closeDistance * 2));
		raysClose.Add(new Ray(basePosition - baseRight * closeDistance + baseForward,		baseRight * closeDistance * 2));
		raysClose.Add(new Ray(basePosition - baseRight * closeDistance - baseForward,		baseRight * closeDistance * 2));
		raysClose.Add(new Ray(basePosition - baseRight * closeDistance + 2 * baseForward,	baseRight * closeDistance * 2));
		raysClose.Add(new Ray(basePosition - baseRight * closeDistance - 2 * baseForward,	baseRight * closeDistance * 2));
		raysClose.Add(new Ray(basePosition - baseRight * closeDistance + 3 * baseForward,	baseRight * closeDistance * 2));
		raysClose.Add(new Ray(basePosition - baseRight * closeDistance - 3 * baseForward,	baseRight * closeDistance * 2));
		raysClose.Add(new Ray(basePosition + baseRight * closeDistance,						-baseRight * closeDistance * 2));
		raysClose.Add(new Ray(basePosition + baseRight * closeDistance + baseForward,		-baseRight * closeDistance * 2));
		raysClose.Add(new Ray(basePosition + baseRight * closeDistance - baseForward,		-baseRight * closeDistance * 2));
		raysClose.Add(new Ray(basePosition + baseRight * closeDistance + 2 * baseForward,	-baseRight * closeDistance * 2));
		raysClose.Add(new Ray(basePosition + baseRight * closeDistance - 2 * baseForward,	-baseRight * closeDistance * 2));
		raysClose.Add(new Ray(basePosition + baseRight * closeDistance + 3 * baseForward,	-baseRight * closeDistance * 2));
		raysClose.Add(new Ray(basePosition + baseRight * closeDistance - 3 * baseForward,	-baseRight * closeDistance * 2));
	}
	
	public override void Start() {
		expectState = isLocked;
		base.Start();
	}
	
	public void Interact () {
		if (isLocked) {
			GameController.SendPlayerMessage("Locked", 2);
			GetComponent<AudioSource>().clip = AudioDefinitions.main.DoorLocked;
			GetComponent<AudioSource>().Play();
			return;
		}
		
		if (!GetComponentInParent<Animator>().GetBool("isOpen")) {
			if (transform.rotation.y == 0) { //zDoor
				if (FindObjectOfType<PlayerController>().transform.position.x < transform.position.x) { //Open south
					GetComponentInParent<Animator>().SetBool("openSouth", true);
				} else { //Open north
					GetComponentInParent<Animator>().SetBool("openSouth", false);
				}
			} else { //xDoor
				if (FindObjectOfType<PlayerController>().transform.position.z < transform.position.z) { //Open east
					GetComponentInParent<Animator>().SetBool("openEast", true);
				} else { //Open west
					GetComponentInParent<Animator>().SetBool("openEast", false);
				}
			}
			GetComponentInParent<Animator>().SetBool("isOpen", true);
			GetComponent<NavMeshObstacle>().enabled = true;
			GetComponent<AudioSource>().clip = AudioDefinitions.main.DoorOpen;
			GetComponent<AudioSource>().Play();
			return;
		} else {
			GetComponentInParent<Animator>().SetBool("isOpen", false);
			GetComponent<AudioSource>().clip = AudioDefinitions.main.DoorClose;
			GetComponent<AudioSource>().Play();
			return;
		}
	}
	
	void Update() {
		foreach (Ray ray in raysClose) {
			Debug.DrawRay(ray.origin, ray.direction * closeDistance);
		}
		if (GetComponentInParent<Animator>().GetBool("isOpen")) {
			CloseForGuardsAndStan();
		} else {	
			OpenForGuards();
		}
	}
	
	void OpenForGuards() {
		foreach(Ray ray in raysRight) {
			if (Physics.Raycast(ray, out hitInfo, openDistance, cullGuards)) {
				OpenDoor (openRight: true, guard: hitInfo.collider.gameObject);
				return;
			}
		}
		
		foreach(Ray ray in raysLeft) {
			if (Physics.Raycast(ray, out hitInfo, openDistance, cullGuards)) {
				OpenDoor (openRight: false, guard: hitInfo.collider.gameObject);
				return;
			}
		}
		
		//No guards found:
		if (timeUntilUnlocked < timeToUnlock) {
			QInteractionButton.GetComponent<QInteractionUI>().AlertOff();
		}
		timeUntilUnlocked = timeToUnlock;
	}
	
	void OpenDoor(bool openRight, GameObject guard) {
		if (!isLocked) {
			if (direction == DoorDirection.x) {
				GetComponentInParent<Animator>().SetBool("openEast", openRight);
			} else {	
				GetComponentInParent<Animator>().SetBool("openSouth", !openRight);
			}
			GetComponentInParent<Animator>().SetBool("isOpen", true);
			GetComponent<AudioSource>().clip = AudioDefinitions.main.DoorOpen;
			GetComponent<AudioSource>().Play();
			GetComponent<NavMeshObstacle>().enabled = true;
		} else {
			if (timeUntilUnlocked <= 0) {
				QInteractionButton.GetComponent<QInteractionUI>().AlertOff();
				isLocked = false;
				expectState = false;
				guard.GetComponentInParent<Foe_Movement_Handler>().navigator.Resume ();
				guard.GetComponentInParent<Foe_Movement_Handler>().canMove = true;
			} else {
				guard.GetComponentInParent<Foe_Movement_Handler>().navigator.Stop ();
				guard.GetComponentInParent<Foe_Movement_Handler>().canMove = false;
			}
			if (timeUntilUnlocked == timeToUnlock) {
				QInteractionButton.GetComponent<QInteractionUI>().InUseOn();
			}
			timeUntilUnlocked -= Time.deltaTime;
		}
	}
	
	void CloseForGuardsAndStan() {
		foreach(Ray ray in raysClose) {
			if (Physics.Raycast(ray, out hitInfo, closeDistance, cullGuards + cullStan)) {
				if (hitInfo.collider.GetComponentInParent<NavMeshAgent>()) {
					hitInfo.collider.GetComponentInParent<NavMeshAgent>().speed =
							hitInfo.collider.GetComponentInParent<Foe_Movement_Handler>().baseSpeed;
				}
				return;
			}
		}
		//No guards or stans detected
		GetComponentInParent<Animator>().SetBool("isOpen", false);
		GetComponent<AudioSource>().clip = AudioDefinitions.main.DoorClose;
		GetComponent<AudioSource>().Play();
		GetComponent<NavMeshObstacle>().enabled = false;
	}
	
	public override void Trigger() {
		if (lockGroupActive) {
			return;
		}
		isLocked = !isLocked;
	}
	
	public override Sprite GetSprite() {
		if (isLocked) {
			return ButtonSpriteDefinitions.main.DoorLocked;
		} else {
			return ButtonSpriteDefinitions.main.DoorUnlocked;
		}
	}
	
	public void SetLockState(int lockGroupValue, bool lockGroupState) {
		if (lockGroup != lockGroupValue) {
			return;
		}
		lockGroupActive = lockGroupState;
		if (lockGroupActive) {
			isLocked = true;
			QInteractionButton.GetComponent<QInteractionUI>().AlertOn();
		} else {
			QInteractionButton.GetComponent<QInteractionUI>().AlertOff();
		}
	}
}
