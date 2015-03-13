using UnityEngine;
using System.Collections;

public class OpenThings : MonoBehaviour {
	public Animator anim;
	public GameObject foeDoorOpenerPrefab;
	public bool willKill;
	public bool holdsPasscard;
	public bool needsPasscard;
	public static bool playerGotPasscard = false;
	public GameObject QCamera;
	
	//Close behind player, etc
	private Transform player;
	private float closeDistance;
	private bool isOpen = false;
		
	void Awake () {
		anim = GetComponent<Animator>();
		if (foeDoorOpenerPrefab != null) {
			GameObject foeDoorOpener = Instantiate(foeDoorOpenerPrefab,
           			transform.position, Quaternion.identity) as GameObject;
			foeDoorOpener.GetComponent<Foe_Door_Opener>().parentDoorAnimator = anim;
		}
	}
	
	void Start() {
		player = PlayerController.player.transform;
	}
		
	public void Interact () {
		if (willKill) { //no insta-death
			GameController.PlayerDead = true;
			GameController.GameOverMessage =
				"You opened a box with a bomb in it - your partner should be watching out for that stuff!";
			QCamera.GetComponent<QUI>().showCamera(false);
			QUI.setText("GAME OVER\nYour partner just opened a bomb. LEARN TO DO YOUR JOB.");
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
		if (!isOpen) {
			if (gameObject.layer == Layerdefs.door) {
				if (transform.rotation.y == 0) {
					if (player.position.x < transform.position.x) { //Open south
						anim.SetBool("openSouth", true);
					} else { //Open north
						anim.SetBool("openSouth", false);
					}
				} else {
					if (player.position.z < transform.position.z) { //Open south
						anim.SetBool("openEast", true);
					} else { //Open north
						anim.SetBool("openEast", false);
					}
				}
			}
			anim.SetBool("isOpen", true);
			isOpen = true;
			closeDistance = Vector3.Distance(transform.position, player.position) + 1f;
		} else {
			anim.SetBool("isOpen", false);
			isOpen = false;
		}
	}
	
	void Update() {
		if (isOpen) {
			if (Vector3.Distance(transform.position, player.position) > closeDistance) {
				isOpen = false;
				anim.SetBool("isOpen", false);
			}
		}
	}
}
