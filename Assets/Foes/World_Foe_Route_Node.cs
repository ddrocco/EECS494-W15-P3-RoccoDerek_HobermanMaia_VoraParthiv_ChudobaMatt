using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class World_Foe_Route_Node : MonoBehaviour {
	static int global_route_id = 0;
	public int routeNodeID = -1;
	public bool pauseAndLookOnEntry = false;
	
	static public GameObject[] routeNodeList = new GameObject[32];
	
	void Awake () {
		if (routeNodeID == -1) {
			routeNodeID = global_route_id;
		}
		routeNodeList[routeNodeID] = gameObject;
		++global_route_id;
		
		if (pauseAndLookOnEntry) {
			GetComponent<Renderer>().material.color = Color.green;
		}
	}
	
	void OnTriggerEnter(Collider other) {
		if (pauseAndLookOnEntry && other.gameObject.tag == "FoeBody") {
			other.gameObject.GetComponentInChildren<Foe_Glance_Command>().ReceiveGlanceCommand(6f, 0f, -90f, 90f);
			other.gameObject.GetComponent<Foe_Movement_Handler>().stayFrozenOnLook = true;
		}
	}
}
