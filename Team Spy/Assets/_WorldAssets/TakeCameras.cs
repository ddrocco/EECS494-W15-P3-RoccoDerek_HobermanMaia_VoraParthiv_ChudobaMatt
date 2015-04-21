using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TakeCameras : MonoBehaviour {
	public List<int> camNumbers = new List<int>();
	
	void Interact() {
		GameController.SendPlayerMessage("Camera access granted", 5);

		foreach (int i in camNumbers) {
			GameObject cameraLocation = GameObject.Find("Cam" + i.ToString());
			if (cameraLocation) {
				ComputerConsole.TakeCameraControl(cameraLocation.GetComponentInChildren<CameraControl>());
			} else {
				print("Cam" + i.ToString() + " not found");
			}
		}
	}
}
