﻿using UnityEngine;
using System.Collections;

public class BoxControl : QInteractable {
	public Animator anim;
	public bool willKill;
	bool hasDetonated;
	public bool holdsPasscard;
	public int timeTillExplode = 100;
	public float distFromBomb = 3;
	public string message; //will only contain a snippet, if anything
	public string QMessage; //notifies Q if Stan found something like a map piece
	public int bombTimer = 0;
	public bool timerSet = false;
	public GameObject elevatorDoor;
	private Transform player;
	
	void Awake () {
		anim = GetComponent<Animator>();
	}
	
	void Start() {
		base.Start();
		player = PlayerController.player.transform;
	}
	
	public void Interact () {
		anim.SetBool("isOpen", !anim.GetBool("isOpen"));
		if (anim.GetBool("isOpen") == true) {
			if (willKill) {
				if (hasDetonated) {
					return;
				}
				timerSet = true;
				return;
			}
			else if (holdsPasscard) {
				ElevatorControl.playerGotPasscard = true;
				//Change Q's tasks
			}
			GameController.SendPlayerMessage(message, 5);
			QUI.setText(QMessage);
		}
	}
	
	public void FixedUpdate() {
		if (timerSet) {
			//play countdown noise
			++bombTimer;
		}
		if (bombTimer >= timeTillExplode) {
			gameObject.GetComponent<AudioSource>().Play();
			if (Vector3.Distance(transform.position, player.transform.position) < distFromBomb) {
				GameController.PlayerDead = true;
				GameController.GameOverMessage =
					"You opened a box with a bomb in it - your partner should be watching out for that stuff!";
				QCamera.GetComponent<QUI>().showCamera(false);
				QUI.setText("GAME OVER\nYour partner just opened a bomb. LEARN TO DO YOUR JOB.");
			}
			else {
				FoeAlertSystem.Alert(transform.position);
			}
			hasDetonated = true;
			timerSet = false;
			bombTimer = 0;
		}
	}
	
	public override void Trigger() {
		if (willKill) {
			if (!timerSet) {
				GameController.SendPlayerMessage("Fire in the hole--your partner set off a bomb!", 5);
				timerSet = true;
				bombTimer = 0;
			} else {
				GameController.SendPlayerMessage("Your partner has defused the bomb; you're safe--for now.", 5);
				timerSet = false;
				bombTimer = 0;
				hasDetonated = true;
			}
		}
	}
	
	public override Sprite GetSprite() {
		if (willKill && timerSet) {
			return ButtonSpriteDefinitions.main.bombLit;
		} else if (willKill && hasDetonated) {
			return ButtonSpriteDefinitions.main.bombDefused;
		} else if (willKill && !timerSet) {
			return ButtonSpriteDefinitions.main.bombDefault;
		} else {
			return ButtonSpriteDefinitions.main.files;
		}
	}
}
