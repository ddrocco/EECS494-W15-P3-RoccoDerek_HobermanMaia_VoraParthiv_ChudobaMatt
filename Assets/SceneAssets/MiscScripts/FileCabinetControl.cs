using UnityEngine;
using System.Collections;

public class FileCabinetControl : MonoBehaviour {
	public Animator anim;
	public GameObject QCamera;
	private string message; //Will only contain a snippet of a message, if anything
	public string QMessage; //Can send a message to Q if it contains something like a map piece
	
	void Awake () {
		anim = GetComponent<Animator>();
	}

	void Start()
	{
		/*string control = "pressing (X)";
		if (PlayerController.debugControls)
			control = "right-clicking";
		
		message = "You can also tag objects if you want your " +
			"partner to help you with them. Try tagging the " +
				"camera in the corner by " + control + " when your targetter turns blue.";*/
	}
	
	public void Interact () {
		anim.SetBool("isOpen", !anim.GetBool("isOpen"));
		if (anim.GetBool("isOpen") == true) {
			GameController.SendPlayerMessage(message, 5);
			QUI.setText(QMessage); //only if there's something important in there
		}
	}
}
