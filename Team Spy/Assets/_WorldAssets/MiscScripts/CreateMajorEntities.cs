using UnityEngine;
using System.Collections;

public class CreateMajorEntities : MonoBehaviour {
	public bool functional = false;
	public bool customCompass = false;

	public GameObject CompassPrefab, PauseSystemPrefab, EventSystemPrefab, MusicPlayerPrefab;
		
	void Awake () {
		if (functional) {
			if (!customCompass) {
				Instantiate(CompassPrefab);
			}
			Instantiate(PauseSystemPrefab);
		}
		Instantiate(EventSystemPrefab);
		if (!MusicPlayer.Exists()) {
			Instantiate(MusicPlayerPrefab);
		}
	}
}
