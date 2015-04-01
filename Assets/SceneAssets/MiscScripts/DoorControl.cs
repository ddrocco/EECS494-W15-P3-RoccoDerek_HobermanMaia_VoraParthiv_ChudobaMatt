using UnityEngine;
using System.Collections;

public class DoorControl : QInteractable {
	public Animator anim;
	public GameObject foeDoorOpenerPrefab;
	public bool isLocked;
	public bool expectState;
	private float closeDistance;
	private bool isOpen = false;
	
	void Awake () {
		anim = GetComponentInParent<Animator>();
		if (foeDoorOpenerPrefab != null) {
			/*Vector3 offset;
			if (transform.rotation.y == 0) {
				offset = new Vector3(0, 0, .75f);
			}
			else {
				offset = new Vector3(.75f, 0, 0);
			}*/
			GameObject foeDoorOpener = Instantiate(foeDoorOpenerPrefab,
			                                       transform.position /*+ offset*/, Quaternion.identity) as GameObject;
			foeDoorOpener.GetComponent<Foe_Door_Opener>().parentDoor = gameObject;
		}
	}
	
	public override void Start() {
		expectState = isOpen;
		base.Start();
	}
	
	public void Interact () {
		if (isLocked) {
			GameController.SendPlayerMessage("Locked", 2);
			gameObject.GetComponent<AudioSource>().Play();
			return;
		}
		if (!isOpen) {
			if (transform.rotation.y == 0) { //zDoor
				if (FindObjectOfType<PlayerController>().transform.position.x < transform.position.x) { //Open south
					anim.SetBool("openSouth", true);
				} else { //Open north
					anim.SetBool("openSouth", false);
				}
			} else { //xDoor
				if (FindObjectOfType<PlayerController>().transform.position.z < transform.position.z) { //Open east
					anim.SetBool("openEast", true);
				} else { //Open west
					anim.SetBool("openEast", false);
				}
			}
			anim.SetBool("isOpen", true);
			isOpen = true;
			closeDistance = Vector3.Distance(transform.position, FindObjectOfType<PlayerController>().transform.position) + 1f;
			return;
		} else {
			anim.SetBool("isOpen", false);
			isOpen = false;
			return;
		}
	}
	
	void Update() {
		if (isOpen) {
			if (Vector3.Distance(transform.position, FindObjectOfType<PlayerController>().transform.position) > closeDistance) {
				isOpen = false;
				anim.SetBool("isOpen", false);
			}
		}
	}
	
	public override void Trigger() {
		isLocked = !isLocked;
	}
	
	public override Sprite GetSprite() {
		if (isLocked) {
			return ButtonSpriteDefinitions.main.doorLocked;
		} else {
			return ButtonSpriteDefinitions.main.doorUnlocked;
		}
	}
}
