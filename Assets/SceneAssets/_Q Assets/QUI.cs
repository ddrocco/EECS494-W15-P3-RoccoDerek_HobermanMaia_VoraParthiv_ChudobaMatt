using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class QUI : MonoBehaviour {
	static string textcontents;
	
	public Text nosignal;
	public Text textoutput;
	public Text cameraDesc;
	public GameObject player;
	
	int frameVisibleMask = (1 << 5) + (1 << Layerdefs.q_visible);
	int frameInvisibleMask = (1 << 5);


	// Use this for initialization
	void Start () {
		textcontents = textoutput.text;
		GetComponent<Camera>().cullingMask = frameInvisibleMask;
	}
	
	// Update is called once per frame
	void Update () {
		textoutput.text = textcontents;
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
			cameraDesc.enabled = true;
			GetComponent<Camera>().cullingMask = frameVisibleMask;
			FindObjectOfType<QPowerSystem>().Enabled(true);
		} else {
			nosignal.enabled = true;
			cameraDesc.enabled = false;
			GetComponent<Camera>().cullingMask = frameInvisibleMask;
			FindObjectOfType<QPowerSystem>().Enabled(false);
		}
	}
}
