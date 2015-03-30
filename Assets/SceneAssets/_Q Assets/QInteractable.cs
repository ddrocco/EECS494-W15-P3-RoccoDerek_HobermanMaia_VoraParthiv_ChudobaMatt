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
	
	Color activeColor = new Color(1f, 0.4f, 0.4f);
	
	public GameObject tagViewPrefab;
	public GameObject tagView;
	
	public virtual void Start () {
		powerSystem = FindObjectOfType<QPowerSystem>();
		InteractionCanvas = GameObject.Find ("InteractionCanvas");

		QIntButton = Instantiate (InteractionCanvas.GetComponent<InteractionCanvasSetup> ().QInteractiveButton);
		QIntButton.GetComponent<RectTransform> ().localPosition =
			new Vector3 (transform.position.x + buttonoffset, InteractionCanvas.GetComponent<RectTransform> ().localPosition.y, transform.position.z + buttonoffset);
		QIntButton.GetComponent<RectTransform> ().localRotation = InteractionCanvas.GetComponent<RectTransform> ().localRotation;
		QIntButton.GetComponent<QInteractionUI> ().controlledObject = this;
		QIntButton.transform.SetParent (InteractionCanvas.transform);
		QIntButton.GetComponent<Image>().sprite = GetSprite();
		
		tagView = Instantiate(tagViewPrefab, transform.position, Quaternion.identity) as GameObject;
		tagView.transform.parent = transform;
		tagView.transform.localScale = Vector3.one;
		tagView.transform.localEulerAngles = Vector3.zero;
		if (GetComponent<MeshFilter>() != null && tagView.GetComponent<MeshFilter>() != null) {
			tagView.GetComponent<MeshFilter>().mesh = GetComponent<MeshFilter>().mesh;
		}
	}
	
	public void Toggle (bool toggleDisplay) {
		if (toggleDisplay) {
			if (!displayIsActive && powerSystem.AddObject(this, false)) {
				Tag();
				displayIsActive = true;
				//Eyecon stuff
			} else if (displayIsActive && powerSystem.DropObject(this, false)) {
				UnTag();
				displayIsActive = false;
				//Eyecon stuff
			}
		} else {
			if ((!functionIsActive && powerSystem.AddObject(this, true))
			    || (functionIsActive && powerSystem.DropObject(this, true))) {
				functionIsActive = !functionIsActive;
				Trigger();
				QIntButton.GetComponent<Image>().sprite = GetSprite();
				if (functionIsActive) {
					QIntButton.GetComponent<Image>().color = activeColor;
				} else {
					QIntButton.GetComponent<Image>().color = Color.white;
				}
			}
		}
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
		
	public virtual void Tag() {
		if (tagView.GetComponent<MeshRenderer>() != null) {
			tagView.GetComponent<MeshRenderer>().enabled = true;
		}
		tagView.GetComponent<ParticleSystemRenderer>().enabled = true;
	}
	public virtual void UnTag() {
		if (tagView.GetComponent<MeshRenderer>() != null) {
			tagView.GetComponent<MeshRenderer>().enabled = false;
		}
		tagView.GetComponent<ParticleSystemRenderer>().enabled = false;
	}
}
