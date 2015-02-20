using UnityEngine;
using System.Collections;

public class OpenThings : MonoBehaviour {
	public Animator anim;
	
	void Awake () {
		anim = GetComponent<Animator>();
	}
		
	void OnMouseDown () {
		anim.SetBool("isOpen", !anim.GetBool("isOpen"));
	}
}
