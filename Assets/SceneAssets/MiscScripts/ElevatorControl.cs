using UnityEngine;
using System.Collections;

public class ElevatorControl : MonoBehaviour {
	public Animator anim;
	public static bool playerGotPasscard = false;
	public bool dispPasscard;
	public GameObject QCamera;
	//private Transform player;
	
	void Awake () {
		anim = GetComponent<Animator>();
	}
	void Update() {
		dispPasscard = playerGotPasscard;
	}
	
	void Start() {
		//player = PlayerController.player.transform;
	}
	
	public void Interact () {
		if (!playerGotPasscard) {
			gameObject.GetComponent<AudioSource>().Play();
			return;
		} else {
			anim.SetBool("isOpen", !anim.GetBool("isOpen"));
			return;
		}
	}
}
