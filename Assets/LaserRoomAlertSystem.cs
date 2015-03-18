using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LaserRoomAlertSystem : MonoBehaviour {
	List<LaserBehavior> activeLasers;
	float targetLight = 0f;
	Light light;
	
	void Start () {
		activeLasers = new List<LaserBehavior>();
		light = GetComponent<Light>();
	}
	
	void Update () {
		if (activeLasers.Count == 0) {
			targetLight = 0f;
		} else if (light.intensity == 0f) {
			//Play alarm sounding up
			targetLight = 8f;
		} else if (light.intensity == 8f) {
			//Play alarm sounding down
			targetLight = 0f;
		}
		
		if (light.intensity < targetLight) {
			light.intensity += 0.3f;
		} else {
			light.intensity -= 0.3f;
		}
		
		if (light.intensity < 0f) {
			light.intensity = 0f;
		} else if (light.intensity > 8f){
			light.intensity = 8f;
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
}
