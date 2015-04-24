using UnityEngine;
using System.Collections;

public class PlayMovie : MonoBehaviour {

	MovieTexture movie;

	void Awake () {
		movie = (MovieTexture)GetComponent<Renderer> ().material.mainTexture;
		movie.Play ();
	}
	
	void Update () {
		if (!movie.isPlaying) {
			Application.LoadLevel(Application.loadedLevel + 1);
		}
	}
}
