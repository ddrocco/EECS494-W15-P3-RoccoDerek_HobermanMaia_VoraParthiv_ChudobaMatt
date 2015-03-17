using UnityEngine;
using System.Collections;

public class BoxControl : MonoBehaviour {
	public Animator anim;
	public bool willKill;
	public bool holdsPasscard;
	public GameObject QCamera;
	public int timeTillDeath = 100;
	public float distFromBomb = 3;
	public string message;
	private int deathTimer = 0;
	private bool timerSet = false;
//	private Transform player;
	
	void Awake () {
		anim = GetComponent<Animator>();
	}
	
	void Start() {
//		player = PlayerController.player.transform;
	}
	
	public void Interact () {
		anim.SetBool("isOpen", !anim.GetBool("isOpen"));
		if (anim.GetBool("isOpen") == true) {
			if (willKill) {
				timerSet = true;
				GameController.SendPlayerMessage("Uh oh--you set off a bomb! Quick, tag it so your partner can diffuse it!", 5);
				return;
			}
			else if (holdsPasscard) {
				//playerGotPasscard = true;
				GameController.SendPlayerMessage("You found the passcard! Ask your partner for your next objective.", 5);
				QUI.appendText("Next Objective: Get to the Elevator.");
				return;
			}
			GameController.SendPlayerMessage(message, 5);
			//QUI.appendText("Next Objective: Get to the Elevator.");
		}
	}
	
	public void FixedUpdate() {
		if (timerSet) {
			++deathTimer;
			/*if (Vector3.Distance(transform.position, player.transform.position) > distFromBomb) {
				timerSet = false;
				deathTimer = 0;
			}*/
		}
		if (deathTimer >= timeTillDeath) {
			GameController.PlayerDead = true;
			GameController.GameOverMessage =
				"You opened a box with a bomb in it - your partner should be watching out for that stuff!";
			QCamera.GetComponent<QUI>().showCamera(false);
			QUI.setText("GAME OVER\nYour partner just opened a bomb. LEARN TO DO YOUR JOB.");
		}
	}
}
