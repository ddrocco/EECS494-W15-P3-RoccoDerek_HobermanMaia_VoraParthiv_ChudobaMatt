using UnityEngine;
using System.Collections;

public class AudioDefinitions : MonoBehaviour {
	public AudioClip WilhelmScream,
		StaticNoise,
		GuardDyingNoise1, GuardDyingNoise2,
		AlarmNoise,
		BoxOpening, BoxClosing,
		ComputerAccess,
		PaperNoises,
		GuardChatter1, GuardChatter2, GuardChatter3;
	public static AudioDefinitions main;
	
	void Awake() {
		main = this;
	}
}
