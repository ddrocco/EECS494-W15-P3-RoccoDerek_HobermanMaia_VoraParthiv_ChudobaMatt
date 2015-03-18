using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class QInteractable : MonoBehaviour {
	public float cost;
	public GameObject QCamera;

	float time = 0f;

	public float buttonoffset = 1f;
	
	QPowerSystem bar;
	GameObject InteractionCanvas;
	
	public enum Type {
		goodBox,
		badBox,
		door,
		elevatorDoor,
		laser,
		guard,
		camera
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
				Tag();
				break;
			case 1:
				UnTag();
				break;
			case 2:
				DoorLock();
				break;
			case 3:
				DoorUnlock();
				break;
			default:
				break;
			}
			break;
		case Type.laser:
			switch(option) {
			case 0:
				Tag();
				break;
			case 1:
				UnTag();
				break;
			case 2:
				SetOffLasers();
				break;
			}
			break;
		case Type.guard:
			switch(option) {
			case 0:
				SubdueGuardAlert();
				break;
			}
			break;
		case Type.goodBox:
			switch(option) {
			case 0:
				Tag();
				break;
			case 1:
				UnTag();
				break;
			}
			break;
		case Type.badBox:
			switch(option) {
			case 0:
				Tag();
				break;
			case 1:
				UnTag();
				break;
			case 2:
				ExplodeBox();
				break;
			}
			break;
		case Type.camera:
			switch(option) {
			case 0:
				SubdueCameraAlert();
				break;
			case 1:
				HackCamera();
				break;
			}
			break;
		case Type.elevatorDoor:
			switch(option) {
			case 0:
				Tag();
				break;
			case 1:
				UnTag();
				break;
			}
			break;
		}
	}

	public void PopulateOptions(){
		options = new List<string> ();

		switch(type) {
		case Type.door:
			options.Add ("Tag");
			options.Add ("Untag");
			options.Add("Lock");
			options.Add("Unlock");
			break;
		case Type.laser:
			options.Add("Tag");
			options.Add ("Untag");
			options.Add("Set Off");
			break;
		case Type.guard:
			options.Add("Subdue Alert");
			break;
		case Type.goodBox:
			options.Add("Tag");
			options.Add ("Untag");
			break;
		case Type.badBox:
			options.Add("Tag");
			options.Add ("Untag");
			options.Add("Set off");
			break;
		case Type.camera:
			options.Add("Subdue Alert");
			options.Add("Hack");
			break;
		case Type.elevatorDoor:
			options.Add("Tag");
			options.Add ("Untag");
			break;
		}
	}

	public void ToggleEnabled(){
		activated = !activated;
		QIntButton.SetActive(activated);
	}
	
	void Tag() {
		GenerateTagVisibility tagScript = GetComponent<GenerateTagVisibility>();
		tagScript.Tag();
		QPowerSystem.main.UseObject(this);
	}
	
	void UnTag() {
		GenerateTagVisibility tagScript = GetComponent<GenerateTagVisibility>();
		tagScript.UnTag();
		QPowerSystem.main.DropObject(this);
	}
		
	void SetOffLasers() {
		LaserBehavior obj = GetComponent<LaserBehavior>();
		obj.alertTimerSet = true;
		obj.alertTimer = 0;
	}
	
	void SubdueCameraAlert() {
		GameController.SendPlayerMessage("Your partner has successfully turned off the camera alert...I just hope it was in time!", 5);
		CameraControl obj = GetComponent<CameraControl>();
		obj.alertTimerSet = false;
		obj.alertTimer = 0;
	}
	
	void SubdueGuardAlert() {
		
	}
	
	void ExplodeBox() {
		GameController.SendPlayerMessage("Fire in the hole--your partner set of a bomb!", 5);
		BoxControl obj = GetComponent<BoxControl>();
		obj.timerSet = true;
		obj.bombTimer = 0;
	}
	
	void HackCamera() {
		CameraControl obj = GetComponent<CameraControl>();
		obj.QIsWatching = true;
		QCameraControl camObj = QCamera.GetComponent<QCameraControl>();
		camObj.ToggleCamera(obj.ID, true);
	}
	
	void DoorLock() {
		//Debug.Log ("Door Locked");
		DoorControl obj = GetComponent<DoorControl>();
		if (obj.isLocked == false) {
			obj.isLocked = true;
		}
	}
	void DoorUnlock() {
		//Debug.Log ("Door Unlocked");
		DoorControl obj = GetComponent<DoorControl>();
		if (obj.isLocked == true) {
			obj.isLocked = false;
		}
	}


}
