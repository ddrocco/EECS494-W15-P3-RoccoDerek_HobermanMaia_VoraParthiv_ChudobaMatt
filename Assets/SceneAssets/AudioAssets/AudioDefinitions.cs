using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AudioDefinitions : MonoBehaviour {
	public AudioClip WilhelmScream,
		StaticNoise,
		AlarmNoise,
		BoxOpening, BoxClosing,
		ComputerAccess,
		PaperNoises,
		GuardChatter1, GuardChatter2, GuardChatter3;
	public List<AudioClip> GuardSpotsPlayer;
	public static AudioDefinitions main;
	
	void Awake() {
		main = this;
	}
}
