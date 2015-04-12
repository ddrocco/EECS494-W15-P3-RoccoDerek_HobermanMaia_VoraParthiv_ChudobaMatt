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
	}
	
	void OtherAction(int value) {
		if (value == 1) {
			FindObjectOfType<QUI>().showCamera(true);
			if (Regex.Replace(EditorApplication.currentScene, "[^0-9]", "").CompareTo("3") == 0) {
				Lightpocalypse();
			}
		} else if (value == 2) {
			GameController.SendPlayerMessage("Additional access granted", 5);
		} else if (value == 3) {
			GameController.SendPlayerMessage("Full system access granted:\nGet to the elevator", 5);
		} else if (value == 4) {
			//hack this camera!
			GameObject Parent = transform.parent.gameObject;
			CameraControl camControl = Parent.GetComponentInChildren<CameraControl>();
			camControl.QIsWatching = true;
			QCameraControl Qcontrol = FindObjectOfType<QCameraControl>();
			QCameraLocation loc = GetComponentInParent<QCameraLocation>();
			Qcontrol.ToggleCamera(loc.cameraNumber, true);
			if (Qcontrol.warning) {
				Qcontrol.AlertOff();
			}
		}
	}
	
	void Lightpocalypse() {
		FindObjectOfType<BrokenLightParent>().Lightpocalypse();
	}
}
