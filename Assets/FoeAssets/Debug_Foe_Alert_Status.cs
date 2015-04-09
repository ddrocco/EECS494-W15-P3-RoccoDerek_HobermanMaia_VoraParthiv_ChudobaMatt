using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Debug_Foe_Alert_Status : MonoBehaviour {
	public static float audialDetectionValue;
	public static bool playerSpotted;
	public static int currentRoom;

	// Use this for initialization
	void Start () {
		playerSpotted = false;
		audialDetectionValue = 0f;
		currentRoom = -1;
	}
	
	// Update is called once per frame
	void Update () {
		GetComponent<Text>().text = "Visual Detection: " + playerSpotted
				+ "\nAudialDetection: " + audialDetectionValue.ToString("F3")
				+ "\nCurrentRoom: " + currentRoom;
	}
}
