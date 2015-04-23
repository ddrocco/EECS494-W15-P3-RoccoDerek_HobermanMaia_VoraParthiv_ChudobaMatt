using UnityEngine;
using System.Collections;

public class CreateMajorEntities : MonoBehaviour {
	public bool functional = false;
	public bool customCompass = false;

	public GameObject //AgentPrefab, HackerPrefab,
	CompassPrefab, PauseSystemPrefab, EventSystemPrefab, MusicPlayerPrefab;
	
	//public Vector3 AgentPosition, AgentRotation;
	
	void Awake () {
		if (functional) {
			if (!customCompass) {
				Instantiate(CompassPrefab);
			}
			Instantiate(PauseSystemPrefab);
		}
		//Instantiate(AgentPrefab, AgentPosition, Quaternion.Euler (AgentRotation));
		//Instantiate(HackerPrefab);
		Instantiate(EventSystemPrefab);
		if (!MusicPlayer.Exists()) {
			Instantiate(MusicPlayerPrefab);
		}
	}
}
