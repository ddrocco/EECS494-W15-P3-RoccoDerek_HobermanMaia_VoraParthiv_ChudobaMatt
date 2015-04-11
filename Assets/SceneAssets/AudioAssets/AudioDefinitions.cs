using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AudioDefinitions : MonoBehaviour {
	public AudioClip WilhelmScream,
		StaticNoise,
		AlarmNoise,
		BoxOpening, BoxClosing,
		DoorLocked, DoorOpen, DoorClose,
		ComputerAccess,
		PaperNoises,
		GuardChatter1, GuardChatter2, GuardChatter3;
	public List<AudioClip> CrouchFootsteps, Footsteps, RunFootsteps,
		GuardSpotsPlayer;
	public static AudioDefinitions main;
	
	void Awake() {
		main = this;
	}
}
