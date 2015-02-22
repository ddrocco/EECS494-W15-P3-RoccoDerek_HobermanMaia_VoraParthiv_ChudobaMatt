using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Debug_Foe_Alert_Status : MonoBehaviour {
	public static float visualDetectionValue, audialDetectionValue;
	public static int currentRoom;

	// Use this for initialization
	void Start () {
		visualDetectionValue = 0f;
		audialDetectionValue = 0f;
		currentRoom = -1;
	}
	
	// Update is called once per frame
	void Update () {
		GetComponent<Text>().text = "Visual Detection: " + visualDetectionValue.ToString("F3")
				+ "\nAudialDetection: " + audialDetectionValue.ToString("F3")
				+ "\nCurrentRoom: " + currentRoom;
	}
}
