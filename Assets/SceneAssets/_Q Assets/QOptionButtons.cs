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
	
	// Update is called once per frame
	void Update () {
	
	}

	public void performAction(){
		if (gameObject.GetComponentInChildren<Text> ().text != "Cancel") {
			controlledObject.QSelect (optionnumber);
		}

		qiui.toggleOptions();
	}
}
