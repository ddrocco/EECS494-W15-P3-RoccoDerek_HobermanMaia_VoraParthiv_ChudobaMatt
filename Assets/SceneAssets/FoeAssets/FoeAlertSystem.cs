using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FoeAlertSystem : MonoBehaviour {	
	
	public static void Alert(Vector3 position) {
		foreach (Foe_Detection_Handler foe in FindObjectsOfType<Foe_Detection_Handler>()) {
			if (foe.canCommunicate) {
				foe.MoveToPlayer();
				foe.movementHandler.StartInvestigation(position);
				foe.isAggressive = true;
			}
		}
		foreach (World_Foe_Coordinator coordinator in FindObjectsOfType<World_Foe_Coordinator>()) {
			coordinator.ReleaseCommunicatingGuards();
		}
	}
}
