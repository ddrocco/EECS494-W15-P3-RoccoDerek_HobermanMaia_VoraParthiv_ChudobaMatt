using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class QInteractable : MonoBehaviour {
	public float functionCost;
	public float displayCost;
	public GameObject QCamera;

	float buttonoffset = 0;
	
	QPowerSystem powerSystem;
	GameObject InteractionCanvas;
	
	public GameObject QIntButton;
	
	public bool functionIsActive = false;
	public bool displayIsActive = false;
	public float cost = 0;
	
	public void Start () {
		powerSystem = FindObjectOfType<QPowerSystem>();
		InteractionCanvas = GameObject.Find ("InteractionCanvas");

		QIntButton = Instantiate (InteractionCanvas.GetComponent<InteractionCanvasSetup> ().QInteractiveButton);
		QIntButton.GetComponent<RectTransform> ().localPosition =
			new Vector3 (transform.position.x + buttonoffset, InteractionCanvas.GetComponent<RectTransform> ().localPosition.y, transform.position.z + buttonoffset);
		QIntButton.GetComponent<RectTransform> ().localRotation = InteractionCanvas.GetComponent<RectTransform> ().localRotation;
		QIntButton.GetComponent<QInteractionUI> ().controlledObject = this;
		QIntButton.transform.SetParent (InteractionCanvas.transform);
		QIntButton.GetComponent<Image>().sprite = GetSprite();
	}
	
	public void Toggle (bool toggleDisplay) {
		if (toggleDisplay) {
			if (!displayIsActive && powerSystem.AddObject(this, false)) {
				Tag();
				print ("Tagging");
				//Eyecon stuff
			} else if (displayIsActive && powerSystem.DropObject(this, false)) {
				UnTag();
				print ("Untagging");
				//Eyecon stuff
			}
		} else {
			if ((functionIsActive && powerSystem.AddObject(this, true))
			    || (!functionIsActive && powerSystem.DropObject(this, true))) {
				functionIsActive = !functionIsActive;
				print ("Triggerl function");
				Trigger();
				QIntButton.GetComponent<Image>().sprite = GetSprite();
			}
		}
	}
	
	void Tag() {
		GenerateTagVisibility tagScript = GetComponent<GenerateTagVisibility>();
		tagScript.Tag();
	}
	
	void UnTag() {
		GenerateTagVisibility tagScript = GetComponent<GenerateTagVisibility>();
		tagScript.UnTag();
	}
	
	void SubdueCameraAlert() {
		GameController.SendPlayerMessage("Your partner has successfully turned off the camera alert...I just hope it was in time!", 5);
		CameraControl obj = GetComponent<CameraControl>();
		obj.alertTimerSet = false;
		obj.alertTimer = 0;
	}
	
	void HackCamera() {
		CameraControl obj = GetComponent<CameraControl>();
		obj.QIsWatching = true;
		QCameraControl camObj = QCamera.GetComponent<QCameraControl>();
		camObj.ToggleCamera(obj.ID, true);
	}

	public virtual void Trigger() {
		
	}
	public virtual Sprite GetSprite() {
		return null;
	}
}
