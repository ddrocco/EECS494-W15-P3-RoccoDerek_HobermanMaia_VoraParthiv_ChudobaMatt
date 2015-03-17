using UnityEngine;
using System.Collections;

public class DoorControl : MonoBehaviour {
	public Animator anim;
	public GameObject foeDoorOpenerPrefab;
	public GameObject QCamera;
	public bool locked;
	public bool expectState;
	private Transform player;
	private float closeDistance;
	private bool isOpen = false;
	
	void Awake () {
		anim = GetComponent<Animator>();
		if (foeDoorOpenerPrefab != null) {
			Vector3 offset;
			if (transform.rotation.y == 0) {
				offset = new Vector3(0, 0, .75f);
			}
			else {
				offset = new Vector3(.75f, 0, 0);
			}
			GameObject foeDoorOpener = Instantiate(foeDoorOpenerPrefab,
			                                       transform.position + offset, Quaternion.identity) as GameObject;
			foeDoorOpener.GetComponent<Foe_Door_Opener>().parentDoorAnimator = anim;
		}
	}
	
	void Start() {
		player = PlayerController.player.transform;
		expectState = isOpen;
	}
	
	public void Interact () {
		if (!isOpen) {
			if (transform.rotation.y == 0) {
				if (player.position.x < transform.position.x) { //Open south
					anim.SetBool("openSouth", true);
				} else { //Open north
					anim.SetBool("openSouth", false);
				}
			} else {
				if (player.position.z < transform.position.z) { //Open east
					anim.SetBool("openEast", true);
				} else { //Open west
					anim.SetBool("openEast", false);
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
