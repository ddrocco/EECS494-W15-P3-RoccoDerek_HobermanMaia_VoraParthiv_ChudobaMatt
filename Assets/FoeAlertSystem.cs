using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FoeAlertSystem : MonoBehaviour {
	static public List<Foe_Detection_Handler> foeList;
	
	void Start () {
		foeList = new List<Foe_Detection_Handler>();
	}
	
	public static void Alert(Vector3 position) {
		foreach (Foe_Detection_Handler foe in foeList) {
			foe.MoveToPlayer();
			foe.movementHandler.StartInvestigation(position);
		}
	}
}
