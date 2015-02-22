using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EndGame : MonoBehaviour {
	
	public void exit (string exitMessage) {
		string display = "GAME OVER\n" + exitMessage; 
		GetComponentInChildren<Text>().text = display;
	}

}
