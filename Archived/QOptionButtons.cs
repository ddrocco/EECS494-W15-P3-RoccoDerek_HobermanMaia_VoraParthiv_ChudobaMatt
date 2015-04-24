using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class QOptionButtons : MonoBehaviour {

	public QInteractionUI qiui;
	public QInteractable controlledObject;
	public int optionnumber = -1;

	// Use this for initialization
	void Start () {
	
	}
	
	/*// Update is called once per frame
	void Update () {
		Debug.Log("Stiff");
	}

	public void performAction(){
		if (Event.current.button == 0) {
			Debug.Log ("MyButton was clicked with left mouse button.");
			qiui.ToggleFunction();
		} else if (Event.current.button == 1) {
			Debug.Log ("MyButton was clicked with right mouse button.");
			qiui.ToggleDisplay();
		}	
	}*/
}
