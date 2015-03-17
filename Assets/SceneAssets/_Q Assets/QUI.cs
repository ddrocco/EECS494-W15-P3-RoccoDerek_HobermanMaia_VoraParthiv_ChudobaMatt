﻿using UnityEngine;
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
		GameObject.Find ("InteractionCanvas").GetComponent<CanvasGroup> ().alpha = 0;
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
			GetComponent<QCameraControl>().enabled = true;
			FindObjectOfType<QPowerSystem>().Enabled(true);
			GameObject.Find ("InteractionCanvas").GetComponent<CanvasGroup> ().alpha = 1;
		} else {
			nosignal.enabled = true;
			cameraDesc.enabled = false;
			GetComponent<Camera>().cullingMask = frameInvisibleMask;
			GetComponent<QCameraControl>().enabled = false;
			FindObjectOfType<QPowerSystem>().Enabled(false);
			GameObject.Find ("InteractionCanvas").GetComponent<CanvasGroup> ().alpha = 0;
		}
	}
}
