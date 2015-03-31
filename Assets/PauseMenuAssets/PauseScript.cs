using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using InControl;

public class PauseScript : MonoBehaviour {
//	private InputDevice device;
	public GameObject QPauseMenu;
	public GameObject StanPauseMenu;

	public bool paused {
		get {
			return Time.timeScale < 0.5;
		}    
	}

	public static bool GamePaused = false;

	// Use this for initialization
	void Start () {
		QPauseMenu.GetComponent<Canvas> ().worldCamera = GameObject.Find ("QCamera").GetComponent<Camera>();
		StanPauseMenu.GetComponent<Canvas> ().worldCamera = GameObject.Find ("PlayerCamera").GetComponent<Camera>();
		Resume ();
	}

	void Awake() {
	//	device = InputManager.ActiveDevice;
	}
	
	// Update is called once per frame
	void Update () {
		if (InputManager.MenuWasPressed || Input.GetKeyUp (KeyCode.Escape)) {
			if(QPauseMenu.activeSelf || StanPauseMenu.activeSelf){
				Resume();
			} else {
				Pause();
			}
		}
	}

	public void Pause(){
		QPauseMenu.SetActive(true);
		StanPauseMenu.SetActive(true);
		Time.timeScale = 0;
		GamePaused = true;
	}

	public void Resume(){
		QPauseMenu.SetActive(false);
		StanPauseMenu.SetActive(false);
		Time.timeScale = 1;
		GamePaused = false;
	}

	public void Restart(){
		Application.LoadLevel (Application.loadedLevel);
	}

	public void Quit(){
		Application.LoadLevel ("MainMenu");
	}
}
