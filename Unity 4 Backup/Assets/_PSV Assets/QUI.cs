using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class QUI : MonoBehaviour {
	static string textcontents;
	
	public Text nosignal;
	public Text textoutput;
	public QCameraControl qcc;
	public GameObject player;
	
	int frameVisibleMask = (1 << 5) + (1 << Layerdefs.q_visible);
	int frameInvisibleMask = (1 << 5);


	// Use this for initialization
	void Start () {
		qcc = GetComponent<QCameraControl> ();
		textcontents = textoutput.text;
		GetComponent<Camera>().cullingMask = frameInvisibleMask;
	}
	
	// Update is called once per frame
	void Update () {
		if(!qcc.enabled){
			qcc.pivotPoint = player.transform.position;
		}
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
			qcc.enabled = true;
			GetComponent<Camera>().cullingMask = frameVisibleMask;
		} else {
			nosignal.enabled = true;
			qcc.enabled = false;
			GetComponent<Camera>().cullingMask = frameInvisibleMask;
		}
	}
}
