using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LaserRoomAlertSystem : MonoBehaviour {
	Light alarmLight;
	bool lightRampingUp;
	
	public bool useAlarmSystem = true;
	Vector3 connectingWireJoint;
	float connectingWireJointRatio;
	float timeToAlarm = 5f;
	List<AlarmSignal> signals;
	float timeSinceSignalSent = 0f;
	
	void Start () {
		alarmLight = GetComponent<Light>();
		signals = new List<AlarmSignal>();
		ConnectToAlarm();
	}
	
	void Update () {
		timeSinceSignalSent += Time.deltaTime;
		UpdateAlarmLight();
		UpdateActiveSignals();
	}
	
	void ConnectToAlarm() {
		if (!useAlarmSystem) {
			return;
		}
		AlarmSystem system = FindObjectOfType<AlarmSystem>();
		if (system == null) {
			print ("Could not find alarm system!");
			useAlarmSystem = false;
			return;
		}
		GetComponent<LineRenderer>().SetPosition(0,transform.position);
		connectingWireJoint = new Vector3(transform.position.x, transform.position.y, system.transform.position.z);
		GetComponent<LineRenderer>().SetPosition(1,connectingWireJoint);
		GetComponent<LineRenderer>().SetPosition(2,system.transform.position);
		float distance1 = Vector3.Distance(transform.position, connectingWireJoint);
		connectingWireJointRatio = distance1 / (distance1 + Vector3.Distance(connectingWireJoint, system.transform.position));
	}
	
	public void SignalAlarm() {
		if (!useAlarmSystem) {
			return;
		}
		if (timeSinceSignalSent < 0.5f) {
			return;
		}
		if (alarmLight.intensity == 0) {
			lightRampingUp = true;
		}
		GameObject alarmSignal = Instantiate(ObjectPrefabDefinitions.main.AlarmSignal);
		alarmSignal.transform.parent = transform;
		signals.Add(alarmSignal.GetComponent<AlarmSignal>());
		timeSinceSignalSent = 0;
	}
	
	void UpdateAlarmLight() {
		if (lightRampingUp) {
			alarmLight.intensity += 0.3f;
			if (alarmLight.intensity >= 8f) {
				lightRampingUp = false;
				alarmLight.intensity = 8f;
			}
		} else if (alarmLight.intensity > 0){
			alarmLight.intensity -= 0.3f;
			if (alarmLight.intensity <= 0) {
				alarmLight.intensity = 0;
				if (signals.Count > 0) {
					lightRampingUp = true;
				}
			}
		}
	}
	
	void UpdateActiveSignals() {
		if (!useAlarmSystem) {
			return;
		}
		AlarmSystem system = FindObjectOfType<AlarmSystem>();
		float wireJointTime = timeToAlarm * connectingWireJointRatio;
		List<AlarmSignal> signalsToDestroy = new List<AlarmSignal>();
		foreach (AlarmSignal signal in signals) {
			if (signal.timeAlive > wireJointTime) {
				float secondLegRatio = (signal.timeAlive - wireJointTime) / (timeToAlarm - wireJointTime);
				if (secondLegRatio > 1) {
					signalsToDestroy.Add (signal);
					continue;
				}
				signal.transform.position = connectingWireJoint * (1f - secondLegRatio)
						+ system.transform.position * secondLegRatio + new Vector3(0, 5, 0);
			} else {
				float firstLegRatio = signal.timeAlive / wireJointTime;
				signal.transform.position = transform.position * (1f - firstLegRatio)
						+ connectingWireJoint * firstLegRatio + new Vector3(0, 5, 0);
			}
			float timeRatio = 1f - signal.timeAlive / timeToAlarm;
			signal.GetComponent<ParticleSystem>().startColor = new Color(1f, timeRatio, 0f);
		}
		foreach(AlarmSignal signal in signalsToDestroy) {
			signals.Remove (signal);
			Destroy (signal.gameObject);
			system.Signal();
		}	
	}
	
	public virtual GameObject TagPrefab() {
		return ObjectPrefabDefinitions.main.TagViewLaser;
	}
}
