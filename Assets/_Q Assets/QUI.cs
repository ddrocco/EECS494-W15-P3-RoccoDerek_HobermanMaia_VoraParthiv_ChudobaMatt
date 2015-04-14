using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class QUI : MonoBehaviour {
	static string textcontents;
	static string controlstextcontents;
	
	public Text nosignal;
	public Text textoutput;
	public Text controlstextoutput;
	public Text cameraDesc;
	public GameObject player;
	public GameObject QCompass;
	//public GameObject Legend;
	
	int frameInvisibleMask = (1 << Layerdefs.ui);


	// Use this for initialization
	void Start () {
		textcontents = textoutput.text;
		controlstextcontents = controlstextoutput.text;
		GetComponent<Camera>().cullingMask = frameInvisibleMask;
		GameObject.Find ("InteractionCanvas").GetComponent<CanvasGroup> ().alpha = 0;
		QCompass = GameObject.Find ("QCompass");
		QCompass.SetActive (false);
		//Legend = GameObject.Find ("Legend");
		//Legend.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {
		textoutput.text = textcontents;
		controlstextoutput.text = controlstextcontents;
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

	public static void setControlsText(string newtext){
		controlstextcontents = newtext;
	}
	
	public static void appendControlsText(string newtext){
		controlstextcontents += "\n" + newtext;
	}
	
	public static void clearControlsText(){
		controlstextcontents = "";
	}

	public void showCamera(bool visible){
		if(visible){
			nosignal.enabled = false;
			cameraDesc.enabled = true;
			clearText();
			GetComponent<Camera>().cullingMask = GetComponent<QCameraControl>().overviewCullingMask;
			GetComponent<QCameraControl>().enabled = true;
			GameObject.Find("CamOverview").GetComponent<QCameraOverview>().camActive = true;
			//FindObjectOfType<QPowerSystem>().Enabled(true);
			GameObject.Find ("InteractionCanvas").GetComponent<CanvasGroup> ().alpha = 1;
			QCompass.SetActive (true);
			//Legend.SetActive (true);
			GetComponent<QCameraControl>().DisableCameras();
		} else {
			nosignal.enabled = true;
			cameraDesc.enabled = false;
			setText("Tell the agent to hack the computer.");
			GetComponent<Camera>().cullingMask = frameInvisibleMask;
			GetComponent<QCameraControl>().enabled = false;
			//FindObjectOfType<QPowerSystem>().Enabled(false);
			GameObject.Find ("InteractionCanvas").GetComponent<CanvasGroup> ().alpha = 0;
			QCompass.SetActive (false);
			//Legend.SetActive (false);
		}
	}
}
