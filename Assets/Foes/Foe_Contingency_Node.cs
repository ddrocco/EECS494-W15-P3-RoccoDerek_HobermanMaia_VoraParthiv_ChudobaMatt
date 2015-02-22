using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Foe_Contingency_Node : MonoBehaviour {
	static int global_contingency_id = -1;
	public int floorID;
	
	static public List<GameObject> contingencyNodeList = new List<GameObject>();
	
	void Awake () {
		floorID = ++global_contingency_id;
		contingencyNodeList.Add (gameObject);
	}
}
