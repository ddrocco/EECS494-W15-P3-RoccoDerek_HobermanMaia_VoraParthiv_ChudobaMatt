using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Foe_Route_Node : MonoBehaviour {
	static int global_route_id = -1;
	public int floorID;
	
	static public List<GameObject> routeNodeList = new List<GameObject>();
	
	void Awake () {
		floorID = ++global_route_id;
		routeNodeList.Add (gameObject);
	}
}
