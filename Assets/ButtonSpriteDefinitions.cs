using UnityEngine;
using System.Collections;

public class ButtonSpriteDefinitions : MonoBehaviour {
	public Sprite
			displayInvisible, displayVisible, displayHighlight,
			bomb5, bomb4, bomb3, bomb2, bomb1, bombLit, bombDefault, bombDefused,
			alarmSounding, alarmSilent,
			guardSounding, guardSilent,
			doorUnlocked, doorLocked,
			laser, polyLaser,
			files,
			cameraIcon;
	public static ButtonSpriteDefinitions main;
	
	void Awake() {
		main = this;
	}
}
