using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Foe_Alert_Status : MonoBehaviour {
	public static float visualDetectionValue, audialDetectionValue;

	// Use this for initialization
	void Start () {
		visualDetectionValue = 0f;
		audialDetectionValue = 0f;
	}
	
	// Update is called once per frame
	void Update () {
		GetComponent<Text>().text = "Visual Detection: " + visualDetectionValue.ToString("F3") +
				"\nAudialDetection: " + audialDetectionValue.ToString("F3");
	}
}
