using UnityEngine;
using System.Collections;
using InControl;

public class Buttons : MonoBehaviour {

	public void Play(){
		Application.LoadLevel ("_AlphaScene");
	}

	void Update(){
		if (InputManager.MenuWasPressed) Play ();
	}

}
