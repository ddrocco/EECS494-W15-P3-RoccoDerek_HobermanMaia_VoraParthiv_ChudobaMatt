using UnityEngine;
using System.Collections;

public class DoorknobControl : MonoBehaviour {
	//A stupid helper script in case the player selects the knob instead of the door.
	
	public void Interact() {
		transform.parent.GetComponentInChildren<DoorControl>().Interact();
	}
}
