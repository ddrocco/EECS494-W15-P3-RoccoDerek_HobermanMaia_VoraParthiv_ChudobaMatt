using UnityEngine;
using System.Collections;

public class ElevatorControl : QInteractable {
	public Animator anim;
	public static bool playerGotPasscard = false;
	public bool dispPasscard;
	//private Transform player;
	
	void Awake () {
		anim = GetComponent<Animator>();
	}
	void Update() {
		dispPasscard = playerGotPasscard;
	}
	
	/*void Start() {
		base.Start();
		//player = PlayerController.player.transform;
	}
	*/
	
	public void Interact () {
		if (!playerGotPasscard) {
			GameController.SendPlayerMessage("Locked:\nFind the key", 5);
			gameObject.GetComponent<AudioSource>().Play();
			return;
		} else {
			anim.SetBool("isOpen", !anim.GetBool("isOpen"));
			return;
		}
	}
	public override void Trigger () {
	
	}
	
	public override Sprite GetSprite () {
		return ButtonSpriteDefinitions.main.doorUnlocked;
	}
}
