using UnityEngine;
using System.Collections;
using InControl;

public class ReturnToMainMenu : MonoBehaviour {
	private InputDevice device;
	
	void Start() {
		device = InputManager.ActiveDevice;
	}

	void Update () {
		if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)
    	|| device.Action1.WasPressed) {
		    Application.LoadLevel("MainMenu");
    	}
	}
}
