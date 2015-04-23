using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using InControl;

public class Buttons : MonoBehaviour {

	public GameObject levelwarp;
	public GameObject levelwarpstan;
	public GameObject stanlevels;

	public GameObject movie_obj;

	public GameObject LCanvas;
	public GameObject RCanvas;

	Button[] buttonset;
	int stanbuttonindex = 0;
	InputDevice device;

	void Awake()
	{
		device = InputManager.ActiveDevice;
	}

	public void Play(){
		movie_obj.SetActive (true);
		RCanvas.SetActive (false);
		LCanvas.SetActive(false);
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
		//play from main menu
		if (InputManager.MenuWasPressed && !levelwarpstan.activeInHierarchy) Play ();

		//level select
		if (device.Action4.WasPressed && !levelwarpstan.activeInHierarchy) {
			levelwarpstan.SetActive (true);
			stanbuttonindex = 0;			
			buttonset = stanlevels.GetComponentsInChildren<Button> ();
			buttonset [stanbuttonindex].Select ();

		} else if (levelwarpstan.activeInHierarchy) { //level select is up
			//go back
			if (device.Action2.WasPressed){
				levelwarpstan.SetActive(false);
			} 
			//move right
			else if (device.DPadRight.WasPressed){
				if(stanbuttonindex + 1 < buttonset.Length){
					buttonset[++stanbuttonindex].Select();
				}
			} 
			//move left
			else if (device.DPadLeft.WasPressed){
				if(stanbuttonindex - 1 >= 0){
					buttonset[--stanbuttonindex].Select();
				}
			}
			//select the level
			else if(device.Action1.WasPressed){
				LevelButton(stanbuttonindex + 1);
			}
		}

	}

}
