using UnityEngine;
using System.Collections;

public class PlayMovie : MonoBehaviour {

	MovieTexture movie;

	// Use this for initialization
	void Awake () {
		movie = (MovieTexture)GetComponent<Renderer> ().material.mainTexture;
		movie.Play ();
	}
	
	// Update is called once per frame
	void Update () {
		if (!movie.isPlaying) {
			Application.LoadLevel(Application.loadedLevel + 1);
		}
	}
}
