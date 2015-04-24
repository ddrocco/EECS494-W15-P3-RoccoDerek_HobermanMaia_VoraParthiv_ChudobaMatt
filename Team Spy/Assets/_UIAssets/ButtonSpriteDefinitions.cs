using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ButtonSpriteDefinitions : MonoBehaviour {
	public Sprite
			BombDefault, BombDefused,
			Alarm, AlarmSignal,
			DoorUnlocked, DoorLocked,
			Laser, PolyLaser,
			Files, PassKey, Computer, BlankIcon,
			Data,
			Elevator,
			CameraOnAlert, CameraBlinded, CameraUnderQ, CameraArrow,
			
			StanTagArrow;
	public List<Sprite> BombDetonationCountdown,
			displayTaggedAnimation;
	public static ButtonSpriteDefinitions main;
	
	void Awake() {
		main = this;
	}
}
