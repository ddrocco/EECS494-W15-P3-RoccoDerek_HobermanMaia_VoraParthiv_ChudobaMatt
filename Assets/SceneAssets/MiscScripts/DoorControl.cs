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
	float openDistance = 1.5f;
	float closeDistance = 2.4f;
	
	RaycastHit hitInfo;
	List<Ray> raysRight = new List<Ray>();
	List<Ray> raysLeft = new List<Ray>();
	List<Ray> raysClose = new List<Ray>();
	
	//Guards unlocking
	float timeToUnlock = 5f;
	float timeUntilUnlocked = 5f;
	
	void Awake () {
		basePosition = transform.position;
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
		
		raysClose.Add(new Ray(basePosition - baseRight * closeDistance,					baseRight * closeDistance * 2));
		raysClose.Add(new Ray(basePosition - baseRight * closeDistance + baseForward,	baseRight * closeDistance * 2));
		raysClose.Add(new Ray(basePosition - baseRight * closeDistance - baseForward,	baseRight * closeDistance * 2));
		raysClose.Add(new Ray(basePosition + baseRight * closeDistance,					-baseRight * closeDistance * 2));
		raysClose.Add(new Ray(basePosition + baseRight * closeDistance + baseForward,	-baseRight * closeDistance * 2));
		raysClose.Add(new Ray(basePosition + baseRight * closeDistance - baseForward,	-baseRight * closeDistance * 2));
	}
	
	public override void Start() {
		expectState = isLocked;
		base.Start();
	}
	
	public void Interact () {
		if (isLocked) {
			GameController.SendPlayerMessage("Locked", 2);
			gameObject.GetComponent<AudioSource>().Play();
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
			return;
		} else {
			GetComponentInParent<Animator>().SetBool("isOpen", false);
			return;
		}
	}
	
	void Update() {
		if (GetComponentInParent<Animator>().GetBool("isOpen")) {
			CloseForGuardsAndStan();
		} else {	
			OpenForGuards();
		}
	}
	
	void OpenForGuards() {
		foreach(Ray ray in raysRight) {
			if (Physics.Raycast(ray, out hitInfo, openDistance, cullGuards)) {
				OpenDoor (openRight: true, guardNavAgent: hitInfo.collider.GetComponentInParent<NavMeshAgent>());
				return;
			}
		}
		
		foreach(Ray ray in raysLeft) {
			if (Physics.Raycast(ray, out hitInfo, openDistance, cullGuards)) {
				OpenDoor (openRight: false, guardNavAgent: hitInfo.collider.GetComponentInParent<NavMeshAgent>());
				return;
			}
		}
		
		//No guards found:
		if (timeUntilUnlocked < timeToUnlock) {
			QInteractionButton.GetComponent<QInteractionUI>().AlertOff();
		}
		timeUntilUnlocked = timeToUnlock;
	}
	
	void OpenDoor(bool openRight, NavMeshAgent guardNavAgent) {
		if (!isLocked) {
			if (direction == DoorDirection.x) {
				GetComponentInParent<Animator>().SetBool("openEast", openRight);
			} else {	
				GetComponentInParent<Animator>().SetBool("openSouth", !openRight);
			}
			GetComponentInParent<Animator>().SetBool("isOpen", true);
			GetComponent<NavMeshObstacle>().enabled = true;
		} else {
			if (timeUntilUnlocked <= 0) {
				QInteractionButton.GetComponent<QInteractionUI>().AlertOff();
				isLocked = false;
				expectState = false;
				guardNavAgent.speed = guardNavAgent.GetComponent<Foe_Movement_Handler>().speed;
			} else {
				guardNavAgent.speed = 0;
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
							hitInfo.collider.GetComponentInParent<Foe_Movement_Handler>().speed;
				}
				return;
			}
		}
		
		//No guards or stans detected
		GetComponentInParent<Animator>().SetBool("isOpen", false);
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
			return ButtonSpriteDefinitions.main.doorLocked;
		} else {
			return ButtonSpriteDefinitions.main.doorUnlocked;
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
