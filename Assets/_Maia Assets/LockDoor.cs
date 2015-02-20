using UnityEngine;
using System.Collections;

public class LockDoor : MonoBehaviour {
	public Animator anim;
	
	void OnMouseDown () {
		print ("doorknob clicked");
		anim = GetComponentInParent<Animator>();
		anim.SetBool("isLocked", !anim.GetBool("isLocked"));
	}
}
