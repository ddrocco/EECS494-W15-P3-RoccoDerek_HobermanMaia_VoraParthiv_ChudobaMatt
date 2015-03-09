using UnityEngine;
using System.Collections;

public class Debug_Player_Sound : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey(KeyCode.LeftShift)) {
			//Walking
			Foe_Detection_Handler.audioMultiplier = 36f;
		} else if (Input.GetKey(KeyCode.LeftControl)) {
			//Walking
			Foe_Detection_Handler.audioMultiplier = 6f;
		} else {
			//Still
			Foe_Detection_Handler.audioMultiplier = 1f;
		}
	}
}
