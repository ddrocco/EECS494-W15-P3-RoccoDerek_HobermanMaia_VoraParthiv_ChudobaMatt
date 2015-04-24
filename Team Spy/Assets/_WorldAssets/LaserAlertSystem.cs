using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LaserAlertSystem : MonoBehaviour {
	Light alarmLight;
	public bool lightRampingUp = false;
	float maxIntensity = 1f;
	
	public bool alarmRaised;
	float timeUntilSignalClear = 10f;
	float timeSinceSignal = 10f;
	public bool heardALaser;
	public GameObject alertedLaser = null;
	
	public AlertHub system;
	
	void Start () {
		alarmLight = GetComponent<Light>();
		ConnectToAlarm();
		alarmRaised = false;
	}
	
	void Update () {
		timeUntilSignalClear += Time.deltaTime;
		if (heardALaser && !AlertHub.guardOnAlert) { //de-arm
			heardALaser = false;
			alarmRaised = false;
			timeSinceSignal = timeUntilSignalClear = 10f;
			alarmLight.intensity = 0;
			lightRampingUp = false;
			alertedLaser.GetComponent<QInteractable>().QInteractionButton.GetComponent<QInteractionUI>().AlertOff();
			alertedLaser = null;
			FindObjectOfType<QCameraControl>().AlertOff();
		}
		if (alarmLight != null) {
			UpdateAlarmLight();
		}	
	}
	
	void ConnectToAlarm() {
		AlertHub[] systems = FindObjectsOfType<AlertHub>();
		if (systems == null) {
			print ("Could not find alarm system!");
			return;
		}
		float minDist = float.PositiveInfinity;
		foreach (AlertHub foundSystem in systems) {
			if (Vector3.Distance(foundSystem.transform.position, transform.position) < minDist) {
				minDist = Vector3.Distance(foundSystem.transform.position, transform.position);
				system = foundSystem;
			}
		}
	}
	
	public void SignalAlarm(Vector3 location, GameObject sourceObject = null) {
		timeSinceSignal = 0f;
		if (heardALaser) {
			return;
		}
		if (!alarmRaised) {
			AudioSource.PlayClipAtPoint(Foe_Detection_Handler.SelectRandomClip(AudioDefinitions.main.CameraSpotsPlayer),
		                            FindObjectOfType<PlayerController>().transform.position);
			alarmRaised = true;
			alertedLaser = sourceObject;
		}
		if (alarmLight != null && alarmLight.intensity == 0) {
			lightRampingUp = true;
		}
		system.Signal(location, sourceObject, this, null);
	}
	
	void UpdateAlarmLight() {
		if (!alarmRaised) {
			return;
		}
		if (lightRampingUp) {
			alarmLight.intensity += 3f * Time.deltaTime;
			if (alarmLight.intensity >= maxIntensity) {
				lightRampingUp = false;
				alarmLight.intensity = maxIntensity;
			}
		} else if (alarmLight.intensity > 0){
			alarmLight.intensity -= 3f * Time.deltaTime;;
			if (alarmLight.intensity <= 0) {
				alarmLight.intensity = 0;
				if (timeSinceSignal < timeUntilSignalClear) {
					lightRampingUp = true;
				}
			}
		}
	}
	
	public virtual GameObject TagPrefab() {
		return ObjectPrefabDefinitions.main.TagViewLaser;
	}
}
