using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class ComputerConsole : MonoBehaviour {
	static List<MapGroup> usedMapValues = new List<MapGroup>();
	public int mapValue;
	public bool debugComputer = false;
	public CameraControl nearestCam;
	public CameraControl[] allCams;
	
	void Start() {
		print ("finding cameras");
		allCams = FindObjectsOfType<CameraControl>();
		float minDist = 1000000f;
		foreach (CameraControl cam in allCams) {
			float tempDist = (cam.transform.position-transform.position).sqrMagnitude;
			if (tempDist < minDist) {
				minDist = tempDist;
				nearestCam = cam;
			}
		}
	}
	
	public void Interact() {
		OtherAction(mapValue);
		EnableRenderers(mapValue);
	}
	
	void OtherAction(int value) {
		if (value == 1) {
			FindObjectOfType<QUI>().showCamera(true);
		} else if (value == 2) {
			GameController.SendPlayerMessage("Additional access granted", 5);
		} else if (value == 3) {
			GameController.SendPlayerMessage("Full system access granted:\nGet to the elevator", 5);
		} else if (value == 4) {
			//hack this camera!
			TakeCameraControl(nearestCam);
		}
	}
	
	public static void TakeCameraControl(CameraControl camControl) {
		camControl.QIsWatching = true;
		QCameraControl Qcontrol = FindObjectOfType<QCameraControl>();
		QCameraLocation loc = camControl.GetComponentInParent<QCameraLocation>();
		Qcontrol.ToggleCamera(loc.cameraNumber, true);
	}
	
	void EnableRenderers(int value) {
		int enabledValue = value-1;
		MapGroup group;
		if (value == 1) {
			group = MapGroup.One;
		} else if (value == 2) {
			group = MapGroup.Two;
		} else if (value == 3) {
			group = MapGroup.Three;
		} else {
			group = MapGroup.One;
		}
		
		if (usedMapValues.Contains(group)) {
			return;
		} else {
			usedMapValues.Add(group);
		}
		
		foreach (GenerateQRenderer renderObject in FindObjectsOfType<GenerateQRenderer>()) {
			renderObject.Activate(enabledValue);
		}
		foreach (QInteractable interactableObj in FindObjectsOfType<QInteractable>()) {
			if (interactableObj.group == group) {
				interactableObj.enableButtonView();
				interactableObj.QInteractionButton.GetComponent<QInteractionUI>().SetEnabled(true);
				interactableObj.qHasFunctionAccess = true;
				if (interactableObj.objectIsTaggable) {
					interactableObj.qHasDisplayAccess = true;
				}
			} else if (interactableObj.unusableGroup == group && !usedMapValues.Contains(interactableObj.group)) {
				interactableObj.enableButtonView();
				interactableObj.qHasFunctionAccess = false;
				interactableObj.QInteractionButton.GetComponent<QInteractionUI>().SetEnabled(false);
				if (interactableObj.objectIsTaggable) {
					interactableObj.qHasDisplayAccess = true;
				}
			}
		}
	}
}
