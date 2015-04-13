﻿using UnityEngine;
using System.Collections;

public class RigInteractionToAlarm : MonoBehaviour {
	bool used = false;
	public void Interact() {
		if (!used) {
			FindObjectOfType<AlertHub>().Signal(FindObjectOfType<PlayerController>().transform.position);
			GameController.SendPlayerMessage("Using that computer triggered an alarm!  Quick, find a hiding spot!", 5);
			QUI.setText("Using that computer triggered an alarm!  Quick, help your partner find a hiding place!");
			used = true;
		}
	}
}
