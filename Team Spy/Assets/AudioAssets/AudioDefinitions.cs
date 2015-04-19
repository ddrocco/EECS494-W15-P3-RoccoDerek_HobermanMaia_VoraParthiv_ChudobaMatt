﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AudioDefinitions : MonoBehaviour {
	public AudioClip WilhelmScream,
		StaticNoise, ShortCircuit,
		AlarmNoise, MGSAlert,
		BoxOpening, BoxClosing,
		TickTock, Explosion,
		DoorLocked, DoorOpen, DoorClose, DoorLocking, DoorUnlocking,
		
		Level5Script;
	public List<AudioClip> CrouchFootsteps, Footsteps, RunFootsteps,
		GuardSpotsPlayer, GuardHearsPlayer;
	public static AudioDefinitions main;
	
	void Awake() {
		main = this;
	}
}