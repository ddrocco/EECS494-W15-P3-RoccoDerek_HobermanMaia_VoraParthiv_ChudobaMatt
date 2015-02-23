using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class QUI : MonoBehaviour {
	static string textcontents;
	
	public Image blank;
	public Text nosignal;
	public Text textoutput;
	public QCameraControl qcc;
	public GameObject player;


	// Use this for initialization
	void Start () {
		qcc = GetComponent<QCameraControl> ();
		textcontents = textoutput.text;
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
			blank.enabled = false;
			nosignal.enabled = false;
			qcc.enabled = true;
		} else {
			blank.enabled = true;
			nosignal.enabled = true;
			qcc.enabled = false;
		}
	}
}
