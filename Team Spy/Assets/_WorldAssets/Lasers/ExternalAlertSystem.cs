using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ExternalAlertSystem : MonoBehaviour {
	public Light alarmLight;
	bool lightRampingUp;
	float maxIntensity = 1f;
	
	public bool useAlarmSystem = true;
	public Vector3 connectingWireJoint;
	float connectingWireJointRatio;
	float timeToAlarm = 5f;
	public List<AlarmSignal> signals;
	public bool signalsInTransit;
	public bool alarmRaised;
	float timeSinceSignalSent = 0f;
	ExternalAlertSystem[] externalAlertSystems;
	
	AlertHub system;
	
	void Start () {
		alarmLight = GetComponent<Light>();
		signals = new List<AlarmSignal>();
		ConnectToAlarm();
		GetComponent<LineRenderer>().enabled = false;
		alarmRaised = false;
		externalAlertSystems = FindObjectsOfType<ExternalAlertSystem>();
	}
	
	void Update () {
		if (signals.Count > 0) {
			signalsInTransit = true;
		} else {
			signalsInTransit = false;
			foreach (ExternalAlertSystem system in externalAlertSystems) {
				if (system.signals.Count > 0) {
					signalsInTransit = true;
					break;
				}
			}
		}
		timeSinceSignalSent += Time.deltaTime;
		if (alarmLight != null) {
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
	
	public void SignalAlarm(Vector3 location, GameObject sourceObject = null) {
		if (!useAlarmSystem) {
			return;
		}
		if (timeSinceSignalSent < 2f) {
			return;
		}
		if (alarmLight != null && alarmLight.intensity == 0) {
			lightRampingUp = true;
		}
		GameObject alarmSignal = Instantiate(ObjectPrefabDefinitions.main.AlarmSignal,
		                                     transform.position,
		                                     Quaternion.identity) as GameObject;
		alarmSignal.transform.parent = transform;
		AlarmSignal temp = alarmSignal.GetComponent<AlarmSignal>();
		temp.detectionLocation = location;
		temp.sourceObject = sourceObject;
		signals.Add(temp);
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
	
	public void RemoveActiveSignal(AlarmSignal sig) {
		signals.Remove (sig);
		Destroy (sig.gameObject);
		//if ()
	}
	public void RemoveAllActiveSignals() {
		List<AlarmSignal> temp = new List<AlarmSignal>();
		foreach (AlarmSignal sig in signals) {
			temp.Add (sig);
		}
		foreach (AlarmSignal sig in temp) {
			RemoveActiveSignal(sig);
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
			//float timeRatio = 1f - signal.timeAlive / timeToAlarm;
		}
		foreach(AlarmSignal signal in signalsToDestroy) {
			system.Signal(signal.detectionLocation, signal.sourceObject, null, this);
			alarmRaised = true;
			RemoveActiveSignal(signal);
		}	
	}
	
	public virtual GameObject TagPrefab() {
		return ObjectPrefabDefinitions.main.TagViewLaser;
	}
}
