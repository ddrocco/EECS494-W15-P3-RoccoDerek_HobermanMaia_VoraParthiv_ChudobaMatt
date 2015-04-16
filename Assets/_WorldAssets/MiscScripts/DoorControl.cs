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
	
	Animator anim;
	AudioSource audioSource;
		
	//Lockdown (during alerts)
	public int lockGroup = 0;
	bool lockGroupActive = false;
	
	//Opening (guards) and closing (guards and Stan) detection:
	Vector3 basePosition, baseForward, baseRight;
	int cullGuards, cullStan;
	float openDistance = 1.5f;
	float closeDistance = 3f;
	
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
		anim = GetComponentInParent<Animator>();
		audioSource = GetComponent<AudioSource>();
		base.Start();
	}
	
	public void Interact () {
		if (isLocked) {
			GameController.SendPlayerMessage("Locked", 2);
			audioSource.clip = AudioDefinitions.main.DoorLocked;
			audioSource.Play();
			return;
		}
		
		if (!anim.GetBool("isOpen")) {
			if (transform.rotation.y == 0) { //zDoor
				if (FindObjectOfType<PlayerController>().transform.position.x < transform.position.x) { //Open south
					anim.SetBool("openSouth", true);
				} else { //Open north
					anim.SetBool("openSouth", false);
				}
			} else { //xDoor
				if (FindObjectOfType<PlayerController>().transform.position.z < transform.position.z) { //Open east
					anim.SetBool("openEast", true);
				} else { //Open west
					anim.SetBool("openEast", false);
				}
			}
			anim.SetBool("isOpen", true);
			audioSource.clip = AudioDefinitions.main.DoorOpen;
			audioSource.Play();
			return;
		} else {
			anim.SetBool("isOpen", false);
			audioSource.clip = AudioDefinitions.main.DoorClose;
			audioSource.Play();
			return;
		}
	}
	
	void Update() {			
		foreach (Ray ray in raysClose) {
			Debug.DrawRay(ray.origin, ray.direction * closeDistance);
		}
		if (anim.GetBool("isOpen")) {
			CloseForGuardsAndStan();
		} else {	
			OpenForGuards();
		}
	}
	
	void OpenForGuards() {
		bool valid = false;
		foreach(Ray ray in raysRight) {
			RaycastHit[] hits = Physics.RaycastAll(ray, openDistance, cullGuards);
			foreach (RaycastHit hit in hits) {
				OpenDoor (openRight: true, guardNavAgent: hit.collider.GetComponentInParent<NavMeshAgent>());
				valid = true;
			}
		}
		
		foreach(Ray ray in raysLeft) {
			RaycastHit[] hits = Physics.RaycastAll(ray, openDistance, cullGuards);
			foreach (RaycastHit hit in hits) {
				OpenDoor (openRight: false, guardNavAgent: hit.collider.GetComponentInParent<NavMeshAgent>());
				valid = true;
			}
		}
		
		if (valid) {
			return;
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
				anim.SetBool("openEast", openRight);
			} else {	
				anim.SetBool("openSouth", !openRight);
			}
			anim.SetBool("isOpen", true);
			audioSource.clip = AudioDefinitions.main.DoorOpen;
			audioSource.Play();
		} else {
			if (timeUntilUnlocked <= 0) {
				QInteractionButton.GetComponent<QInteractionUI>().AlertOff();
				isLocked = false;
				expectState = false;
				guardNavAgent.Resume ();
			} else {
				if (guardNavAgent.GetComponentInChildren<Foe_Detection_Handler>().timeSincePlayerSpotted >= 
				    	guardNavAgent.GetComponentInChildren<Foe_Detection_Handler>().timeUntilPlayerLost) {
					guardNavAgent.Stop ();    	
				}
			}
			if (timeUntilUnlocked == timeToUnlock) {
				QInteractionButton.GetComponent<QInteractionUI>().InUseOn();
			}
			timeUntilUnlocked -= Time.deltaTime;
		}
	}
	
	void CloseForGuardsAndStan() {
		bool valid = false;		
		foreach(Ray ray in raysClose) {
			RaycastHit[] hits = Physics.RaycastAll(ray, closeDistance, cullGuards + cullStan);
			foreach (RaycastHit hit in hits) {
				if (hit.collider.GetComponentInParent<NavMeshAgent>() && hit.collider.GetComponentInParent<NavMeshAgent>().enabled) {
					hit.collider.GetComponentInParent<NavMeshAgent>().Resume ();
				}
				valid = true;
			}
		}
		
		if (valid) {
			return;
		} else {
			anim.SetBool("isOpen", false);
			audioSource.clip = AudioDefinitions.main.DoorClose;
			audioSource.Play();
		}
	}
	
	public override void Trigger() {
		if (lockGroupActive) {
			return;
		}
		isLocked = !isLocked;
		
		if (!anim.GetBool("isOpen")) {
			if (isLocked) {
				audioSource.clip = AudioDefinitions.main.DoorLocking;
			} else {
				audioSource.clip = AudioDefinitions.main.DoorUnlocking;
			}
			audioSource.Play();
		}
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
