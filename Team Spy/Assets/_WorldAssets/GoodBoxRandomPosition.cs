using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GoodBoxRandomPosition : MonoBehaviour {
	static List<Vector3> positions = new List<Vector3>();
	static List<Quaternion> rotations = new List<Quaternion>();
	public static bool IsUsed = false;

	void Awake() {
		positions.Add (transform.position);
		rotations.Add (transform.rotation);
		IsUsed = true;
		GetComponent<Renderer>().enabled = false;
	}
	
	public static void GetPos(GameObject GoodBox) {
		int RandValue = Mathf.FloorToInt(Random.Range(0, positions.Count));
		GoodBox.transform.position = positions[RandValue];
		GoodBox.transform.rotation = rotations[RandValue];
	}
}
