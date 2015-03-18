using UnityEngine;
using System.Collections;

public class ElevatorControl : MonoBehaviour {
	public Animator anim;
	public bool playerGotPasscard = false;
	public GameObject QCamera;
	//private Transform player;
	
	void Awake () {
		anim = GetComponent<Animator>();
	}
	
	void Start() {
		//player = PlayerController.player.transform;
	}
	
	public void Interact () {
		if (!playerGotPasscard) {
			gameObject.GetComponent<AudioSource>().Play();
			return;
		} else {
			anim.SetBool("isOpen", true);
			return;
		}
	}
}
