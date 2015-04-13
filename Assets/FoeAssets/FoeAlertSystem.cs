using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FoeAlertSystem : MonoBehaviour {
	public static float playerRange = 0.5f;
	public static float bombRange = 3f;
	
	public static void Alert(Vector3 position, bool isPlayer) {
		foreach (Foe_Detection_Handler foe in FindObjectsOfType<Foe_Detection_Handler>()) {
			if (foe.enabled) {
				foe.movementHandler.StartInvestigation(position, isPlayer);
				foe.isAggressive = true;
			}
		}
		foreach (World_Foe_Coordinator coordinator in FindObjectsOfType<World_Foe_Coordinator>()) {
			coordinator.ReleaseCommunicatingGuards();
		}
	}
}
