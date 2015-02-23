using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class QUI : MonoBehaviour {

	public Image blank;
	public Text nosignal;
	public Text textoutput;
	public QCameraControl qcc;
	public GameObject player;


	// Use this for initialization
	void Start () {
		qcc = GetComponent<QCameraControl> ();
	}
	
	// Update is called once per frame
	void Update () {
		if(!qcc.enabled){
			qcc.pivotPoint = player.transform.position;
		}
	}

	public void setText(string newtext){
		textoutput.text = newtext;
	}

	public void appendText(string newtext){
		textoutput.text += "\n" + newtext;
	}

	public void clearText(){
		textoutput.text = "";
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
