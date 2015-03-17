using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class QInteractable : MonoBehaviour {
	public float cost;

	float time = 0f;

	public float buttonoffset = 1f;
	
	QPowerSystem bar;
	GameObject InteractionCanvas;
	
	public enum Type {
		box,
		door,
		laser,
		guard
	};

	public Type type;
	public bool activated = true;
	public GameObject QIntButton;
	List<string> options; //list of options to display
	
	// Use this for initialization
	void Start () {
		bar = FindObjectOfType<QPowerSystem>();
		InteractionCanvas = GameObject.Find ("InteractionCanvas");

		QIntButton = Instantiate (InteractionCanvas.GetComponent<InteractionCanvasSetup> ().QInteractiveButton);
		QIntButton.GetComponent<RectTransform> ().localPosition =
			new Vector3 (transform.position.x + buttonoffset, InteractionCanvas.GetComponent<RectTransform> ().localPosition.y, transform.position.z + buttonoffset);
		QIntButton.GetComponent<RectTransform> ().localRotation = InteractionCanvas.GetComponent<RectTransform> ().localRotation;
		PopulateOptions();
		QIntButton.GetComponent<QInteractionUI> ().options = options;
		QIntButton.GetComponent<QInteractionUI> ().controlledObject = this;
		QIntButton.transform.SetParent (InteractionCanvas.transform);

	}
	
	// Update is called once per frame
	void Update () {
		time += Time.deltaTime;
		if (time > 4f) {
			time = 0f;
			bar.DropObject(this);
		}
		if (time > 2f) {
			bar.UseObject(this);
		}
	}
	
	public void QSelect (int option) {
		switch(type) {
		case Type.door:
			switch(option){
			case 0:
				DoorLock();
				break;
			case 1:
				DoorUnlock();
				break;
			default:
				break;
			}
			break;
		case Type.laser:
			//Do stuff
			break;
		case Type.guard:
			//Do stuff
			break;
		case Type.box:
			//do stuff;
			break;
		}
	}

	public void PopulateOptions(){
		options = new List<string> ();

		switch(type) {
		case Type.door:
			options.Add("Lock");
			options.Add("Unlock");
			break;
		case Type.laser:
			//Do stuff
			break;
		case Type.guard:
			//Do stuff
			break;
		case Type.box:
			//do stuff;
			break;
		}
	}

	public void ToggleEnabled(){
		activated = !activated;
		QIntButton.SetActive(activated);
	}
	
	void BoxDisplay() {
	
	}
	void DoorLock() {
		Debug.Log ("Door Locked");
	}
	void DoorUnlock() {
		Debug.Log ("Door Unlocked");
	}


}
