using UnityEngine;
using System.Collections;

public class DisplayForQ : MonoBehaviour {
	public Canvas message;
	public bool sentMessage = false;
	
	public void SendMessage() {
		if (sentMessage) return;
		message.enabled = true;
		sentMessage = true;
		return;
	}
}
