using UnityEngine;
using System.Collections;

public class ElevatorControl : QInteractable {
	public Animator anim;
	public static bool playerGotPasscard;
	public bool needsPasscard;
	
	void Awake () {
		playerGotPasscard = false;
		anim = GetComponent<Animator>();
		if (anim == null) {
			anim = GetComponentInParent<Animator>();
		}
	}
	
	void Update() {
		qHasFunctionAccess = false;
	}
	
	public void Interact () {
		if (anim.GetBool("isOpen")) {
			return;
		}
		if (needsPasscard && !playerGotPasscard) {
			GameController.SendPlayerMessage("Locked\nFind the key", 5);
			return;
		} else {
			anim.SetBool("isOpen", true);
			AudioSource.PlayClipAtPoint(AudioDefinitions.main.ElevatorDoorOpen, transform.position);
		}
	}
	
	public override void Trigger () {
	}
	
	public override Sprite GetSprite () {
		return ButtonSpriteDefinitions.main.Elevator;
	}
}
