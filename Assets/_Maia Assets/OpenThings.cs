﻿using UnityEngine;
using System.Collections;

public class OpenThings : MonoBehaviour {
	public Animator anim;
	
	void Awake () {
		anim = GetComponent<Animator>();
	}
		
	public void openThing () {
		//print("registered click");
		anim.SetBool("isOpen", !anim.GetBool("isOpen"));
	}
}
