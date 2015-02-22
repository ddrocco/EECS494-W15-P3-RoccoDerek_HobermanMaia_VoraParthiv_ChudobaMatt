using UnityEngine;
using System.Collections;

public class OpenThings : MonoBehaviour {
	public Animator anim;
	public GameObject foeDoorOpenerPrefab;
	
	void Awake () {
		anim = GetComponent<Animator>();
		if (foeDoorOpenerPrefab != null) {
			GameObject foeDoorOpener = Instantiate(foeDoorOpenerPrefab,
           			transform.position, Quaternion.identity) as GameObject;
			foeDoorOpener.GetComponent<Foe_Door_Opener>().parentDoorAnimator = anim;
		}
	}
		
	public void Interact () {
		//print("registered click");
		anim.SetBool("isOpen", !anim.GetBool("isOpen"));
	}
}
