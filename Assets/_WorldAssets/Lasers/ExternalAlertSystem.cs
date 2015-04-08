using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ExternalAlertSystem : MonoBehaviour {
	Light alarmLight;
	bool lightRampingUp;
	float maxIntensity = 1f;
	
	public bool useAlarmSystem = true;
	Vector3 connectingWireJoint;
	float connectingWireJointRatio;
	float timeToAlarm = 5f;
	List<AlarmSignal> signals;
	float timeSinceSignalSent = 0f;
	
	AlertHub system;
	
	void Start () {
		alarmLight = GetComponent<Light>();
		signals = new List<AlarmSignal>();
		ConnectToAlarm();
		GetComponent<LineRenderer>().enabled = false;
	}
	
	void Update () {
		timeSinceSignalSent += Time.deltaTime;
		if (GetComponent<Light>() != null) {
			UpdateAlarmLight();
		}
		UpdateActiveSignals();
	}
	
	void ConnectToAlarm() {
		if (!useAlarmSystem) {
			return;
		}
		AlertHub[] systems = FindObjectsOfType<AlertHub>();
		if (systems == null) {
			print ("Could not find alarm system!");
			useAlarmSystem = false;
			return;
		}
		float minDist = float.PositiveInfinity;
		foreach (AlertHub foundSystem in systems) {
			if (foundSystem.isActive) {
				AlertHub.isSounding = true;
			}
			if (Vector3.Distance(foundSystem.transform.position, transform.position) < minDist) {
				minDist = Vector3.Distance(foundSystem.transform.position, transform.position);
				system = foundSystem;
			}
		}
		LineRenderer connection = GetComponent<LineRenderer>();
		connection.SetWidth(0.4f, 0.4f);
		connection.material = ObjectPrefabDefinitions.main.alertConnection;
		connection.SetPosition(0,transform.position);
		connectingWireJoint = new Vector3(transform.position.x, transform.position.y, system.transform.position.z);
       	connection.SetPosition(1,connectingWireJoint
        	 + 0.1f * (transform.position - connectingWireJoint).normalized);
       	connection.SetPosition(2,connectingWireJoint
			+ 0.1f * (system.transform.position - connectingWireJoint).normalized);
       	connection.SetPosition(3,system.transform.position);
		float distance1 = Vector3.Distance(transform.position, connectingWireJoint);
		connectingWireJointRatio = distance1 / (distance1 + Vector3.Distance(connectingWireJoint, system.transform.position));
	}
	
	public void SignalAlarm(Vector3 location) {
		if (!useAlarmSystem) {
			return;
		}
		if (timeSinceSignalSent < 0.5f) {
			return;
		}
		if (GetComponent<Light>() != null && alarmLight.intensity == 0) {
			lightRampingUp = true;
		}
		GameObject alarmSignal = Instantiate(ObjectPrefabDefinitions.main.AlarmSignal);
		alarmSignal.transform.parent = transform;
		alarmSignal.GetComponent<AlarmSignal>().detectionLocation = location;
		signals.Add(alarmSignal.GetComponent<AlarmSignal>());
		timeSinceSignalSent = 0;
	}
	
	void UpdateAlarmLight() {
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
			system.Signal(signal.detectionLocation);
			Destroy (signal.gameObject);
		}	
	}
	
	public virtual GameObject TagPrefab() {
		return ObjectPrefabDefinitions.main.TagViewLaser;
	}
}
