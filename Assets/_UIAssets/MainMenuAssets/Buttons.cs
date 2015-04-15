using UnityEngine;
using System.Collections;
using InControl;

public class Buttons : MonoBehaviour {

	public GameObject levelwarp;

	public void Play(){
		Application.LoadLevel (Application.loadedLevel + 1);
	}

	public void LevelSelect(){
		levelwarp.SetActive (true);
	}

	public void Back(){
		levelwarp.SetActive (false);
	}

	public void LevelButton(int i){
		if (Application.levelCount > i)
			Application.LoadLevel (i);
		else
			Debug.Log ("No level with number " + i +" in Build Settings.");
	}

	void Update(){
		if (InputManager.MenuWasPressed) Play ();
	}

}
