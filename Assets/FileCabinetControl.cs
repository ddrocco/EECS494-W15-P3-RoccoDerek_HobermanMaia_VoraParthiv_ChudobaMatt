using UnityEngine;
using System.Collections;

public class FileCabinetControl : MonoBehaviour {
	public Animator anim;
	public GameObject QCamera;
	public string message;
	
	void Awake () {
		anim = GetComponent<Animator>();
	}
	
	public void Interact () {
		anim.SetBool("isOpen", !anim.GetBool("isOpen"));
		if (anim.GetBool("isOpen") == true) {
			GameController.SendPlayerMessage(message, 5);
			//QUI.appendText("Next Objective: Get to the Elevator.");
		}
	}
}
