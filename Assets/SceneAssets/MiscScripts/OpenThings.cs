using UnityEngine;
using System.Collections;

public class OpenThings : MonoBehaviour {
	public Animator anim;
	public bool willKill;
	public bool holdsPasscard;
	public bool needsPasscard;
	public static bool playerGotPasscard = false;
	public GameObject QCamera;
	public int timeTillDeath = 100;
	public float distFromBomb = 3;
//	private bool isOpen = false;
	private int deathTimer = 0;
	private bool timerSet = false;
	private Transform player;
		
	void Awake () {
		anim = GetComponent<Animator>();
	}
	
	void Start() {
		player = PlayerController.player.transform;
	}
	
	public void Interact () {
		if (willKill) {
			timerSet = true;
		}
		if (needsPasscard && !playerGotPasscard) {
			gameObject.GetComponent<AudioSource>().Play();
			return;
		} else if (needsPasscard && playerGotPasscard) { //This needs reworking
			GameController.PlayerDead = true;
			GameController.GameOverMessage =
				"You won!  Congratulations!";
			QCamera.GetComponent<QUI>().showCamera(false);
			QUI.setText("WELL DONE\nYour partner survived. EXCELLENT WORK.");
			return;
		}
		if (holdsPasscard) {
			playerGotPasscard = true;
			GameController.SendPlayerMessage("You found the passcard! Ask your partner for your next objective.", 5);
			QUI.appendText("Next Objective: Get to the Elevator.");
		}
	}
	
	public void FixedUpdate() {
		if (timerSet) {
			++deathTimer;
			if (Vector3.Distance(transform.position, player.transform.position) > distFromBomb) {
				timerSet = false;
				deathTimer = 0;
			}
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
