using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class QOptionButtons : MonoBehaviour {

	public QInteractionUI qiui;
	public GameObject controlledObject;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void performAction(){
		if (gameObject.GetComponentInChildren<Text> ().text != "Cancel") {
			Debug.Log (controlledObject.name + " " + gameObject.GetComponentInChildren<Text> ().text);
			//In future, all interactive objects for Q need functions within that allow setting necessary properties
			//Set up a series of if statements here that are based on the button's text and will call the necessary function
			//in the controlledObject 
		}

		qiui.toggleOptions();
	}
}
