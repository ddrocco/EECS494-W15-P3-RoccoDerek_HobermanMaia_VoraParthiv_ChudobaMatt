using UnityEngine;
using System.Collections;

public class BoxControl : QInteractable {
	public Animator anim;
	public bool isBomb;	//Differentiates between documents and bombs
	public string message; //will only contain a snippet, if anything
	public string QMessage; //notifies Q if Stan found something like a map piece
	
	public bool holdsPasscard;
	
	//Bomb:
	public bool isArmed = true;
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
			Instantiate(ObjectPrefabDefinitions.main.ExplosionSmoke, transform.position, Quaternion.identity);
			Instantiate(ObjectPrefabDefinitions.main.Explosion, transform.position, Quaternion.identity);
			
			if (Vector3.Distance(transform.position, PlayerController.player.transform.position) < killDistance) {
				GameController.PlayerDead = true;
				GameController.GameOverMessage =
					"You set off a bomb";
				FindObjectOfType<QUI>().showCamera(false);
				QUI.setText("Game Over\nThe agent set off a bomb");
			}
			else {
				FoeAlertSystem.Alert(transform.position, isPlayer: false);
			}
			isArmed = false;
			timerSet = false;
			timeToDetonation = 0;
			Destroy(this.gameObject, .5f);
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
				return ButtonSpriteDefinitions.main.BombDefused;
			}
			if (timerSet) {
				return ButtonSpriteDefinitions.main.BombDetonationCountdown[Mathf.FloorToInt(timeToDetonation)];
			} else {
				return ButtonSpriteDefinitions.main.BombDefault;
			}
		}
		return ButtonSpriteDefinitions.main.Files;
	}
}
