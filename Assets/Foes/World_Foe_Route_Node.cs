using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class World_Foe_Route_Node : MonoBehaviour {
	static int global_route_id = -1;
	public int routeNodeID;
	
	static public List<GameObject> routeNodeList = new List<GameObject>();
	
	void Awake () {
		routeNodeID = ++global_route_id;
		routeNodeList.Add (gameObject);
	}
}
