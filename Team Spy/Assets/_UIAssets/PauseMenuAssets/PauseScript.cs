using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using InControl;

public class PauseScript : MonoBehaviour {
	public static int sensitivityValue = 5;

	public GameObject QPauseMenu;
	public GameObject StanPauseMenu;
	public Slider sensitivity;
	public Text sensitivityValText;

	private InputDevice device;
	private PlayerController player;

	public bool paused {
		get {
			return Time.timeScale < 0.5;
		}    
	}

	public static bool GamePaused = false;

	// Use this for initialization
	void Start () {
		sensitivity = GameObject.Find("SensitivitySlider").GetComponent<Slider>();
		sensitivity.value = sensitivityValue;
		QPauseMenu.GetComponent<Canvas> ().worldCamera = GameObject.Find ("QCamera").GetComponent<Camera>();
		StanPauseMenu.GetComponent<Canvas> ().worldCamera = GameObject.Find ("PlayerCamera").GetComponent<Camera>();
		player = FindObjectOfType<PlayerController>();
		Resume();
	}

	void Awake() {
		device = InputManager.ActiveDevice;
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
		if (!GamePaused) return;

		if (device.DPadLeft.WasPressed)
		{
			sensitivity.value--;
		}
		if (device.DPadRight.WasPressed)
		{
			sensitivity.value++;
		}
		if (device.Action2.WasPressed) {
			Quit ();
		}
		sensitivityValText.text = sensitivity.value.ToString();
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
		sensitivityValue = (int)sensitivity.value;
		player.SetLookSensitivity(sensitivityValue);
	}

	public void Restart(){
		Application.LoadLevel (Application.loadedLevel);
	}

	public void Quit(){
		Application.LoadLevel ("MainMenu");
	}
}
