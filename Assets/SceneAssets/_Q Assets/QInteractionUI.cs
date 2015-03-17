using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class QInteractionUI : MonoBehaviour {

	bool showingOptions = false;
	QCameraControl qcc;
	GameObject qcanvas;
	public GameObject controlledObject;
	public GameObject optionButton;
	List<GameObject> optionlist;

	// Use this for initialization
	void Start () {
		qcc = GameObject.Find ("QCamera").GetComponent<QCameraControl> ();
		qcanvas = GameObject.Find("QCanvas");
	}
	
	// Update is called once per frame
	void Update () {

	}

	public void toggleOptions(){
		showingOptions = !showingOptions;

		qcc.enabled = showingOptions;

		if(showingOptions){
			optionlist = new List<GameObject>();

			//pick the object type and create specific options
			if (controlledObject.name.StartsWith("QInteractive_Door")){
				for(int i = 0; i < 2; ++i){
					GameObject button = Instantiate(optionButton) as GameObject;
					button.transform.SetParent(qcanvas.transform, false);
					optionlist.Add(button);
				}

				optionlist[0].GetComponentInChildren<Text>().text = "Unlock";
				optionlist[1].GetComponentInChildren<Text>().text = "Lock";

			} else if (controlledObject.name.StartsWith("QInteractive_Laser")){
				for(int i = 0; i < 2; ++i){
					GameObject button = Instantiate(optionButton) as GameObject;
					button.transform.SetParent(qcanvas.transform, false);
					optionlist.Add(button);
				}
				
				optionlist[0].GetComponentInChildren<Text>().text = "Deactivate";
				optionlist[1].GetComponentInChildren<Text>().text = "Activate";
			}
			//position buttons
			if(optionlist.Count > 0) optionlist[0].GetComponent<RectTransform>().anchoredPosition3D = new Vector3(0, 100, 0);
			for(int i = 1; i < optionlist.Count; ++i){
				Vector3 pos = optionlist[i-1].GetComponent<RectTransform>().anchoredPosition3D;
				pos.y -= 40;
				optionlist[i].GetComponent<RectTransform>().anchoredPosition3D = pos;
			}

			//create cancel button
			GameObject cancelbutton = Instantiate(optionButton) as GameObject;
			cancelbutton.transform.SetParent(qcanvas.transform, false);
			cancelbutton.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(0, -150, 0);
			cancelbutton.GetComponentInChildren<Text>().text = "Cancel";
			optionlist.Add(cancelbutton);

			//tell each button it is related to this object and the controlledObject
			foreach(GameObject b in optionlist){
				b.GetComponent<QOptionButtons>().qiui = this;
				b.GetComponent<QOptionButtons>().controlledObject = controlledObject;
			}

		} else {
			//clear options
			foreach(GameObject b in optionlist){
				Destroy(b);
			}
		}
	}
}
