using UnityEngine;
using System.Collections;

public class BoxControl : QInteractable {
	public Animator anim;
	public bool isBomb;	//Differentiates between documents and bombs
	public string message; //will only contain a snippet, if anything
	public string QMessage; //notifies Q if Stan found something like a map piece
	
	public bool holdsPasscard;
	
	//Bomb:
	bool isArmed = true;
	public bool timerSet = false;
	public float timeToDetonation = 5f;
	public float killDistance = 3f;
	
	public GameObject elevatorDoor;
	
	void Awake () {
		anim = GetComponentInChildren<Animator>();
	}
	
	public override void Start() {
		base.Start();
	}
	
	public void Interact () {
		if (anim.GetBool("isOpen")) {
			anim.SetBool("isOpen",false);
			AudioSource.PlayClipAtPoint(AudioDefinitions.main.BoxClosing,transform.position);
		} else {
			anim.SetBool("isOpen",true);
			AudioSource.PlayClipAtPoint(AudioDefinitions.main.BoxOpening,transform.position);
		}
		
		if (anim.GetBool("isOpen") == true) {
			if (isBomb) {
				if (!isArmed) {
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
	
	public void Update() {
		if (!isBomb || !isArmed) {
			return;
		}
		if (timerSet) {
			//play countdown noise
			int previousCountdown = Mathf.CeilToInt(timeToDetonation);
			timeToDetonation -= Time.deltaTime;
			if (previousCountdown == 5 && Mathf.CeilToInt(timeToDetonation) == 4) {
				gameObject.GetComponent<AudioSource>().clip = AudioDefinitions.main.TickTock;
				gameObject.GetComponent<AudioSource>().loop = true;
				gameObject.GetComponent<AudioSource>().Play();
			} else if (previousCountdown == 2 && Mathf.CeilToInt(timeToDetonation) == 1) {
				gameObject.GetComponent<AudioSource>().loop = false;
			}
		}
		if (timeToDetonation <= 0) {
		
			gameObject.GetComponent<AudioSource>().clip = AudioDefinitions.main.Explosion;
			gameObject.GetComponent<AudioSource>().loop = false;
			gameObject.GetComponent<AudioSource>().Play();
			
			if (Vector3.Distance(transform.position, PlayerController.player.transform.position) < killDistance) {
				GameController.PlayerDead = true;
				GameController.GameOverMessage =
					"You set off a bomb";
				FindObjectOfType<QUI>().showCamera(false);
				QUI.setText("Game Over\nThe agent set off a bomb");
			}
			else {
				FoeAlertSystem.Alert(transform.position);
			}
			isArmed = false;
			timerSet = false;
			timeToDetonation = 0;
		}
	}
	
	public override void Trigger() {
		if (isBomb && isArmed) {
			if (!timerSet) {
				//GameController.SendPlayerMessage("Fire in the hole--your partner set off a bomb!", 5);
				timerSet = true;
			} else {
				//GameController.SendPlayerMessage("Your partner has defused the bomb; you're safe--for now.", 5);
				timerSet = false;
				isArmed = false;
				
				gameObject.GetComponent<AudioSource>().loop = false;
			}
		}
	}
	
	public override Sprite GetSprite() {
		if (isBomb) {
			if (!isArmed) {
				return ButtonSpriteDefinitions.main.bombDefused;
			}
			if (timerSet) {
				switch (Mathf.CeilToInt(timeToDetonation)) {
					case 1:	return ButtonSpriteDefinitions.main.bomb1;
					case 2:	return ButtonSpriteDefinitions.main.bomb2;
					case 3:	return ButtonSpriteDefinitions.main.bomb3;
					case 4:	return ButtonSpriteDefinitions.main.bomb4;
					case 5:	return ButtonSpriteDefinitions.main.bomb5;
				}
			} else {
				return ButtonSpriteDefinitions.main.bombDefault;
			}
		}
		return ButtonSpriteDefinitions.main.files;
	}
}
