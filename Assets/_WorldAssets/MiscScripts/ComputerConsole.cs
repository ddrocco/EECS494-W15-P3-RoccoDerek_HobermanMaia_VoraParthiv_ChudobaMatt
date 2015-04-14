using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Text.RegularExpressions;

public class ComputerConsole : MonoBehaviour {
	public int mapValue;
	public bool debugComputer = false;
	
	public void Interact() {
		if (!debugComputer) {
			MapCoverControl.ToggleMapGroup(mapValue, true);
		}
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
			GameObject Parent = transform.parent.gameObject;
			CameraControl camControl = Parent.GetComponentInChildren<CameraControl>();
			TakeCameraControl(camControl);
		}
	}
	
	public static void TakeCameraControl(CameraControl camControl) {
		camControl.QIsWatching = true;
		QCameraControl Qcontrol = FindObjectOfType<QCameraControl>();
		QCameraLocation loc = camControl.GetComponentInParent<QCameraLocation>();
		Qcontrol.ToggleCamera(loc.cameraNumber, true);
		if (Qcontrol.warning) {
			Qcontrol.AlertOff();
		}
	}
	
	void EnableRenderers(int value) {
		int enabledValue = value-1;
		foreach (GenerateQRenderer renderObject in FindObjectsOfType<GenerateQRenderer>()) {
			renderObject.Activate(enabledValue);
		}
	}
}
