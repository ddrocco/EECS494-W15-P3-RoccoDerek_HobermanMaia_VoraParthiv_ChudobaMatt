using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MusicPlayer : MonoBehaviour {
	AudioSource music;
	static MusicPlayer main;
	static List<Foe_Detection_Handler> chasingGuards = new List<Foe_Detection_Handler>();
	static List<Foe_Detection_Handler> investigatingGuards = new List<Foe_Detection_Handler>();
	
	public float baseVolume = 0.05f;
	public float chaseVolume = 0.05f;
	public float quietVolume = 0.01f;
	
	void Start() {
		DontDestroyOnLoad(gameObject);
		main = this;
		music = GetComponent<AudioSource>();
		music.clip = AudioDefinitions.main.ExploreMusic;
		music.volume = baseVolume;
		music.Play();
	}
	
	void OnLevelWasLoaded (int level) {
		new List<Foe_Detection_Handler>();
		new List<Foe_Detection_Handler>();
		music.volume = baseVolume;
		if (main.music.clip != AudioDefinitions.main.ExploreMusic) {
			main.music.clip = AudioDefinitions.main.ExploreMusic;
			main.music.Play();
		}
	}
	
	public static void Heard(Foe_Detection_Handler guard) {
		if (!investigatingGuards.Contains(guard) && !chasingGuards.Contains(guard)) {
			investigatingGuards.Add(guard);		
		}
		
		if (chasingGuards.Count == 0) {
			main.music.volume = main.quietVolume;
		}
	}
	
	public static void Spotted(Foe_Detection_Handler guard) {
		if (!chasingGuards.Contains(guard)) {
			if (investigatingGuards.Contains(guard)) {
				investigatingGuards.Remove(guard);
			}
			chasingGuards.Add(guard);	
		}
		if (main.music.clip != AudioDefinitions.main.ActionMusic) {
			main.music.volume = main.chaseVolume;
			main.music.clip = AudioDefinitions.main.ActionMusic;
			main.music.Play();
		}
	}
	
	public static void Escaped(Foe_Detection_Handler guard) {
		if (chasingGuards.Contains(guard)) {
			chasingGuards.Remove(guard);
			if (chasingGuards.Count == 0) {
				if (investigatingGuards.Count == 0) {
					main.music.volume = main.baseVolume;
				} else {
					main.music.volume = main.quietVolume;
				}
				main.music.clip = AudioDefinitions.main.ExploreMusic;
				main.music.Play();
			}
		} else if (investigatingGuards.Contains(guard)) {
			investigatingGuards.Remove(guard);
			if (investigatingGuards.Count == 0) {
				main.music.volume = main.baseVolume;
				main.music.clip = AudioDefinitions.main.ExploreMusic;
				main.music.Play();
			}
		}
	}
	
	public static bool Exists() {
		return (main != null);
	}
}
