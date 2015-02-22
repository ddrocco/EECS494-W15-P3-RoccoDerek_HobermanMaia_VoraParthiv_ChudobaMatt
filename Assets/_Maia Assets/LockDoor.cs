using UnityEngine;
using System.Collections;

public class LockDoor : MonoBehaviour {
	public Animator anim;
	
	public void toggleLock () {
		//print ("doorknob clicked");
		anim = GetComponentInParent<Animator>();
		anim.SetBool("isLocked", !anim.GetBool("isLocked"));
	}
}
