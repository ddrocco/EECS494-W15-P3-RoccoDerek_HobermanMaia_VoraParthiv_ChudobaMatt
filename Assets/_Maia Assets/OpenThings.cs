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
		
	void Awake () {
		anim = GetComponent<Animator>();
		if (foeDoorOpenerPrefab != null) {
			GameObject foeDoorOpener = Instantiate(foeDoorOpenerPrefab,
           			transform.position, Quaternion.identity) as GameObject;
			foeDoorOpener.GetComponent<Foe_Door_Opener>().parentDoorAnimator = anim;
		}
	}
		
	public void Interact () {
		if (willKill) {
			GameController.PlayerDead = true;
			GameController.GameOverMessage =
				"You opened a box with a bomb in it - your partner should be watching out for that stuff!";
			QCamera.GetComponent<QUI>().showCamera(false);
			QUI.setText("GAME OVER\nYour partner just opened a bomb. LEARN TO DO YOUR JOB.");
		}
		if (needsPasscard && !playerGotPasscard) {
			gameObject.GetComponent<AudioSource>().Play();
			return;
		} else if (needsPasscard && playerGotPasscard) {
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
		anim.SetBool("isOpen", !anim.GetBool("isOpen"));
	}
}
