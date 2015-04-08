using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class World_Foe_Route_Node : MonoBehaviour {
	public bool pauseAndLookOnEntry = false;
	
	static public GameObject[] routeNodeList = new GameObject[32];
	
	//Set OBJECT NAME to anything followed by a NUM.  That NUM will be its ID.
	//If NUM exceeds 32, feel free to change the size of routeNodeList.
	
	void Awake () {
		string id = Regex.Replace(name, @"[^\d]", "");
		routeNodeList[int.Parse(id)] = gameObject;
		
		if (pauseAndLookOnEntry) {
			GetComponent<Renderer>().material.color = Color.green;
		}
	}
	
	void OnTriggerEnter(Collider other) {
		if (pauseAndLookOnEntry && other.GetComponent<Foe_Movement_Handler>() != null
				&& other.GetComponent<Foe_Movement_Handler>().state
				!= Foe_Movement_Handler.alertState.investigating) {
			other.gameObject.GetComponentInChildren<Foe_Glance_Command>().ReceiveGlanceCommand(6f, 0f, -90f, 90f);
			other.gameObject.GetComponent<Foe_Movement_Handler>().stayFrozenOnLook = true;
		}
	}
}
