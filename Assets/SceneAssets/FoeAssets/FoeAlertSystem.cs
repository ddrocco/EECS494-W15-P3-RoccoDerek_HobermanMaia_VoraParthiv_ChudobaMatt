using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FoeAlertSystem : MonoBehaviour {
	static public List<Foe_Detection_Handler> foeList;
	
	//To release guards that are stationary waiting for a friend:
	static public List<World_Foe_Coordinator> posLockList;
	
	void Awake () {
		if (foeList == null) {
			foeList = new List<Foe_Detection_Handler>();
		}
		if (posLockList == null) {
			posLockList = new List<World_Foe_Coordinator>();
		}
	}
	
	public static void Alert(Vector3 position) {
		foreach (Foe_Detection_Handler foe in foeList) {
			if (foe.canCommunicate) {
				foe.MoveToPlayer();
				foe.movementHandler.StartInvestigation(position);
				foe.isAggressive = true;
			}
		}
		foreach (World_Foe_Coordinator coordinator in posLockList) {
			coordinator.ReleaseCommunicatingGuards();
		}
	}
}
