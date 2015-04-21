﻿using UnityEngine;
using System.Collections;

public class ElevatorControl : QInteractable {
	public Animator anim;
	public static bool playerGotPasscard = false;
	public bool needsPasscard;
	
	void Awake () {
		anim = GetComponent<Animator>();
		if (anim == null) {
			anim = GetComponentInParent<Animator>();
		}
	}
	
	public void Interact () {
		if (needsPasscard && !playerGotPasscard) {
			GameController.SendPlayerMessage("Locked:\nFind the key", 5);
			//gameObject.GetComponent<AudioSource>().Play();
			return;
		} else {
			anim.SetBool("isOpen", !anim.GetBool("isOpen"));
			return;
		}
	}
	public override void Trigger () {
	
	}
	
	public override Sprite GetSprite () {
		return ButtonSpriteDefinitions.main.Elevator;
	}
}
