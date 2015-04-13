﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ButtonSpriteDefinitions : MonoBehaviour {
	public Sprite
			DisplayInvisible, DisplayVisible, nil,
			BombDefault, BombDefused,
			Alarm,
			DoorUnlocked, DoorLocked,
			Laser, PolyLaser,
			Files,
			Elevator,
			CameraOnAlert, CameraBlinded, CameraUnderQ, CameraArrow;
	public List<Sprite> BombDetonationCountdown,
			displayTaggedAnimation;
	public static ButtonSpriteDefinitions main;
	
	void Awake() {
		main = this;
	}
}
