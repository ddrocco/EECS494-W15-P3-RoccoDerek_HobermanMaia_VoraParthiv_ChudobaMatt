using UnityEngine;
using System.Collections;

public class ButtonSpriteDefinitions : MonoBehaviour {
	public Sprite bomb5, bomb4, bomb3, bomb2, bomb1, bombLit, bombDefault, bombDefused,
			alarmSounding, alarmSilent,
			guardSounding, guardSilent,
			doorUnlocked, doorLocked,
			laser, poly_laser,
			files,
			cameraIcon;
	public static ButtonSpriteDefinitions main;
	
	void Awake() {
		main = this;
	}
}
