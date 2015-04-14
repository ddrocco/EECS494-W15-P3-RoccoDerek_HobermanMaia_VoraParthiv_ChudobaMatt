using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class QUI : MonoBehaviour {
	static string textcontents;

	public Text nosignal;
	public Text textoutput;
	public GameObject player;
	public GameObject QCompass;

	int frameInvisibleMask = (1 << Layerdefs.ui);


	// Use this for initialization
	void Start () {
		textcontents = textoutput.text;
		GetComponent<Camera>().cullingMask = frameInvisibleMask;
		GameObject.Find ("InteractionCanvas").GetComponent<CanvasGroup> ().alpha = 0;
		QCompass = GameObject.Find ("QCompass");
		QCompass.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {
		textoutput.text = textcontents;
	}
	
	public void Interact() {
		showCamera(true);
		QUI.setText("Objective: Find the elevator key");
		GameController.SendPlayerMessage("System access granted:\nFind more terminals", 5);
	}

	public static void setText(string newtext){
		textcontents = newtext;
	}

	public static void appendText(string newtext){
		textcontents += "\n" + newtext;
	}

	public static void clearText(){
		textcontents = "";
	}

	public void showCamera(bool visible){
		if(visible){
			nosignal.enabled = false;
			clearText();
			GetComponent<Camera>().cullingMask = GetComponent<QCameraControl>().overviewCullingMask;
			GetComponent<QCameraControl>().enabled = true;
			GameObject.Find("CamOverview").GetComponent<QCameraOverview>().camActive = true;
			GameObject.Find ("InteractionCanvas").GetComponent<CanvasGroup> ().alpha = 1;
			QCompass.SetActive (true);
			GetComponent<QCameraControl>().DisableCameras();
		} else {
			nosignal.enabled = true;
			setText("Tell the agent to hack the computer.");
			GetComponent<Camera>().cullingMask = frameInvisibleMask;
			GetComponent<QCameraControl>().enabled = false;
			GameObject.Find ("InteractionCanvas").GetComponent<CanvasGroup> ().alpha = 0;
			QCompass.SetActive (false);
		}
	}
}
