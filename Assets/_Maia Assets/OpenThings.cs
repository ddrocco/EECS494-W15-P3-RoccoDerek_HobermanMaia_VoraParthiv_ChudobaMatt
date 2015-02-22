using UnityEngine;
using System.Collections;

public class OpenThings : MonoBehaviour {
	public Animator anim;
	public bool willKill;
	public bool holdsPasscard;
	public bool needsPasscard;
	public bool playerGotPasscard = false;
	
	void Awake () {
		anim = GetComponent<Animator>();
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
