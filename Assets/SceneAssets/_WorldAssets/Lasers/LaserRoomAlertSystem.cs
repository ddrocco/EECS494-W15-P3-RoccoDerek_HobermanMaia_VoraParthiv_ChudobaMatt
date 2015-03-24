using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LaserRoomAlertSystem : MonoBehaviour {
	List<LaserBehavior> activeLasers;
	List<PolyLaserParent> activeLaserGroups;
	float targetLight = 0f;
	Light alarmLight;
	
	void Start () {
		activeLasers = new List<LaserBehavior>();
		alarmLight = GetComponent<Light>();
	}
	
	void Update () {
		if (activeLasers.Count == 0) {
			targetLight = 0f;
		} else if (alarmLight.intensity == 0f) {
			//Play alarm sounding up
			targetLight = 8f;
		} else if (alarmLight.intensity == 8f) {
			//Play alarm sounding down
			targetLight = 0f;
		}
		
		if (alarmLight.intensity < targetLight) {
			alarmLight.intensity += 0.3f;
		} else {
			alarmLight.intensity -= 0.3f;
		}
		
		if (alarmLight.intensity < 0f) {
			alarmLight.intensity = 0f;
		} else if (alarmLight.intensity > 8f){
			alarmLight.intensity = 8f;
		}
	}
	
	public void Activate(LaserBehavior trigger) {
		if (!activeLasers.Contains(trigger)) {
			activeLasers.Add (trigger);
			if (targetLight == 0f) {
				targetLight = 8f;
			}
		}
	}
	
	public void Activate(PolyLaserParent trigger) {
		if (!activeLaserGroups.Contains(trigger)) {
			activeLaserGroups.Add (trigger);
			if (targetLight == 0f) {
				targetLight = 8f;
			}
		}
	}
}
