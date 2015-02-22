﻿using UnityEngine;
using System.Collections;

public class OpenThings : MonoBehaviour {
	public Animator anim;
	public GameObject foeDoorOpenerPrefab;	public bool willKill;
	public bool holdsPasscard;
	public bool needsPasscard;
	public bool playerGotPasscard = false;	
	void Awake () {
		anim = GetComponent<Animator>();
		if (foeDoorOpenerPrefab != null) {
			GameObject foeDoorOpener = Instantiate(foeDoorOpenerPrefab,
           			transform.position, Quaternion.identity) as GameObject;
			foeDoorOpener.GetComponent<Foe_Door_Opener>().parentDoorAnimator = anim;
		}
	}
		
	public void Interact () {
		if (willKill) {/*do stuff*/}
		if (needsPasscard && !playerGotPasscard) {
			//play sound
			return;
		}
		if (holdsPasscard) {
			playerGotPasscard = true;
			//UI stuff??
		}
		anim.SetBool("isOpen", !anim.GetBool("isOpen"));
	}
}
