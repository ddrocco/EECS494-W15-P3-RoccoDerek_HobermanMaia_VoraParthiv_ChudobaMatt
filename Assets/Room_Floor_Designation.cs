using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Room_Floor_Designation : MonoBehaviour {
	static int global_floor_id = -1;
	public int floorID;
	
	static public List<GameObject> floorList = new List<GameObject>();
	
	void Awake () {
		floorID = ++global_floor_id;
		floorList.Add (gameObject);
	}
	
	bool isInRoom(Vector3 location) {
		if (
				location.x < transform.position.x + collider.bounds.extents.x
		    	&& location.x > transform.position.x - collider.bounds.extents.x
				&& location.z < transform.position.z + collider.bounds.extents.z
				&& location.z > transform.position.z - collider.bounds.extents.z
		) {
			return true;
		} else {
			return false;
		}
	}
	
	public static int GetCurrentRoom(Vector3 location) {
		float minDistance = float.MaxValue;
		int minLocation = -1;
		foreach (GameObject floor in floorList) {
			float distance = (floor.transform.position - location).magnitude;
			if (distance < minDistance) {
				minDistance = distance;
				minLocation = floor.GetComponent<Room_Floor_Designation>().floorID;
			}
		}
		if (
				location.x < floorList[minLocation].transform.position.x + floorList[minLocation].collider.bounds.extents.x
				&& location.x > floorList[minLocation].transform.position.x - floorList[minLocation].collider.bounds.extents.x
				&& location.z < floorList[minLocation].transform.position.z + floorList[minLocation].collider.bounds.extents.z
				&& location.z > floorList[minLocation].transform.position.z - floorList[minLocation].collider.bounds.extents.z
		) {
			return minLocation;
		} else {
			return -1;
		}
	}
}
