using UnityEngine;
using System.Collections;

public class HeartbeatMonitor : QInteractable {
	bool functional = true;
	
	public void ReceiveDistressCall(Vector3 position) {
		if (functional) {
			GetComponent<ExternalAlertSystem>().SignalAlarm(position);
		}
	}
	
	public override void Trigger() {
		functional = !functional;
	}
	
	public override Sprite GetSprite() {
		if (functional) {
			return ButtonSpriteDefinitions.main.heartbeatMonitor;
		} else {
			return ButtonSpriteDefinitions.main.heartbeatMonitorDisabled;
		}
	}
	
	public override Sprite GetDisplayStatus() {
		return ButtonSpriteDefinitions.main.nil;
	}
}