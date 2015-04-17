using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using InControl;

public class Buttons : MonoBehaviour {

	public GameObject levelwarp;
	public GameObject levelwarpstan;
	public GameObject stanlevels;
	Button[] buttonset;
	int stanbuttonindex = 0;

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
		//play from main menu
		if (InputManager.MenuWasPressed && !levelwarpstan.activeInHierarchy) Play ();

		//level select
		if (Input.GetKeyDown (KeyCode.Y) && !levelwarpstan.activeInHierarchy) {
			levelwarpstan.SetActive (true);
			stanbuttonindex = 0;			
			buttonset = stanlevels.GetComponentsInChildren<Button> ();
			buttonset [stanbuttonindex].Select ();

		} else if (levelwarpstan.activeInHierarchy) { //level select is up
			//go back
			if (Input.GetKeyDown(KeyCode.B)){
				levelwarpstan.SetActive(false);
			} 
			//move right
			else if (Input.GetKeyDown(KeyCode.RightArrow)){
				if(stanbuttonindex + 1 < buttonset.Length){
					buttonset[++stanbuttonindex].Select();
				}
			} 
			//move left
			else if (Input.GetKeyDown(KeyCode.LeftArrow)){
				if(stanbuttonindex - 1 >= 0){
					buttonset[--stanbuttonindex].Select();
				}
			}
			//select the level
			else if(Input.GetKeyDown(KeyCode.A)){
				LevelButton(stanbuttonindex + 1);
			}
		}

	}

}
