using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UnlockBoxes : MonoBehaviour {
	private static List<GameObject> boxes;

	void Awake () {
		boxes = new List<GameObject>();
		GameObject[] objs = GameObject.FindGameObjectsWithTag("Interactive");
		print ("gameobjs="+objs.Length);
		foreach (GameObject obj in objs)
		{
			BoxControl temp = obj.GetComponentInChildren<BoxControl>();
			if (temp != null) {
				boxes.Add(obj);
			}
		}
		print (boxes.Count);
	}
	
	public void Interact() {
		GameController.SendPlayerMessage("You have unlocked VIEWING BOXES for your partner!", 5);
		//QUI.setText("Now you can view and interact with boxes from your Overview without your partner having to tag them!");
		UnlockAll();
	}

	public void UnlockAll() {
		foreach (GameObject box in boxes) {
			print ("onebox");
			QInteractable interact = box.GetComponentInChildren<QInteractable>();
			interact.enabled = true;
		}
	}
}
