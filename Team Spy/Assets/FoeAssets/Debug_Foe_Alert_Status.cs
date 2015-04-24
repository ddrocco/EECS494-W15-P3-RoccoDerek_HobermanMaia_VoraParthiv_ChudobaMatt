using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Debug_Foe_Alert_Status : MonoBehaviour {
	public static float audialDetectionValue;
	public static bool playerSpotted;
	public static int currentRoom;

	void Start () {
		playerSpotted = false;
		audialDetectionValue = 0f;
		currentRoom = -1;
	}
	
	void Update () {
		GetComponent<Text>().text = "Visual Detection: " + playerSpotted
				+ "\nAudialDetection: " + audialDetectionValue.ToString("F3")
				+ "\nCurrentRoom: " + currentRoom;
	}
}
