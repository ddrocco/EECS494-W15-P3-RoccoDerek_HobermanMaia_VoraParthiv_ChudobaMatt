﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class ComputerConsole : QInteractable {
	static List<MapGroup> usedMapValues = new List<MapGroup>();
	public string StanMessage;
	public string QMessage;
	public bool useObjectiveUpdate = false;
	public string QObjectiveUpdate;
	public int mapValue;
	public bool debugComputer = false;
	public CameraControl nearestCam;
	public CameraControl[] allCams;
	private bool hasBeenUsed = false;
	
	public override void Start() {
		allCams = FindObjectsOfType<CameraControl>();
		float minDist = 1000000f;
		foreach (CameraControl cam in allCams) {
			float tempDist = (cam.transform.position-transform.position).sqrMagnitude;
			if (tempDist < minDist) {
				minDist = tempDist;
				nearestCam = cam;
			}
		}
		base.Start();
	}
	
	void Update() {
		qHasFunctionAccess = false;
	}
	
	public void Interact() {
		OtherAction(mapValue);
		EnableRenderers(mapValue);

		StanMessage = StanMessage.Replace("NEWLINE", "\n");
		QMessage = QMessage.Replace("NEWLINE", "\n");
		GameController.SendPlayerMessage(StanMessage, 5f);
		QUI.setText(QMessage, objective: false);
		if (useObjectiveUpdate && !hasBeenUsed) {
			QUI.setText(QObjectiveUpdate, objective: true);
		}
		hasBeenUsed = true;
	}
	
	public override void Trigger() {
		return;
	}
	
	void OtherAction(int value) {
		if (value == 1) {
			FindObjectOfType<QUI>().showCamera(true);
		} else if (value == 2) {
			GameController.SendPlayerMessage("Partial system access granted\nObjective: Find and hack another terminal", 5);
		} else if (value == 3) {
			GameController.SendPlayerMessage("Full system access granted\nObjective: Find the elevator key", 5);
		} else if (value == 4) {
			//hack this camera!
			TakeCameraControl(nearestCam);
			if (GetComponent<DisplayForQ>() != null) {
				GetComponent<DisplayForQ>().SendMessage();
			}
		}
	}
	
	public static void TakeCameraControl(CameraControl camControl) {
		camControl.QIsWatching = true;
		QCameraControl Qcontrol = FindObjectOfType<QCameraControl>();
		QCameraLocation loc = camControl.GetComponentInParent<QCameraLocation>();
		Qcontrol.ToggleCamera(loc.cameraNumber, true);
	}
	
	public override Sprite GetSprite() {
		return ButtonSpriteDefinitions.main.Computer;
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
		
		AudioSource source = GetComponent<AudioSource>();
		
		if (!usedMapValues.Contains(group)) {
			usedMapValues.Add(group);
			if (!source.isPlaying) {
				source.clip = AudioDefinitions.main.ComputerBigLoad;
				source.Play();
			}
		} else {
			if (!source.isPlaying) {
				source.clip = AudioDefinitions.main.ComputerLoad;
				source.Play();
			}
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
