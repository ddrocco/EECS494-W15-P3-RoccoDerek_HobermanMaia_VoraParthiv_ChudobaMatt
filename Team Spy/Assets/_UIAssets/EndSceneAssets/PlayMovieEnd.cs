using UnityEngine;
using System.Collections;
using InControl;

public class PlayMovieEnd : MonoBehaviour {

	InputDevice device;
	MovieTexture movie;


	// Use this for initialization
	void Awake () {
		device = InputManager.ActiveDevice;

		movie = (MovieTexture)GetComponent<Renderer> ().material.mainTexture;
		movie.Play ();
	}
	
	// Update is called once per frame
	void Update () {
		if (!movie.isPlaying || Input.anyKeyDown)
			NextLevel ();

		if (device.AnyButton.WasPressed)
			NextLevel ();
	}

	void NextLevel(){
		if (Application.levelCount > Application.loadedLevel + 1)
			Application.LoadLevel (Application.loadedLevel + 1);
		else
			Application.LoadLevel (0);
	}
}
