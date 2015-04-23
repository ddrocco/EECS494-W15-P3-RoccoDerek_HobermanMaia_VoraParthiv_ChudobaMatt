using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ExternalAlertSystem : MonoBehaviour {
	public Light alarmLight;
	private AudioSource audiosrc;
	bool lightRampingUp;
	float maxIntensity = 1f;
	
	public bool useAlarmSystem = true;
	public Vector3 connectingWireJoint;
	float connectingWireJointRatio;
	float signalTimeToAlarm = 5f;
	float probeTimeToAlarm = 1f;
	public List<AlarmSignal> signals;
	public bool signalsInTransit;
	public bool alarmRaised;
	float timeSinceSignalSent = 0f;
	float timeBetweenSignals = 2f;
	ExternalAlertSystem[] externalAlertSystems;
	
	AlertHub system;
	ParticleSystem firstArm, secondArm;
	
	void Start () {
		alarmLight = GetComponent<Light>();
		audiosrc = GetComponent<AudioSource>();
		signals = new List<AlarmSignal>();
		ConnectToAlarm();
		alarmRaised = false;
	}
	
	void Update () {
		if (signals.Count > 0) {
			signalsInTransit = true;
			firstArm.startColor = Color.yellow;
			secondArm.startColor = Color.yellow;
			if (!audiosrc.isPlaying) {
				audiosrc.loop = true;
				audiosrc.Play();
			}
		} else {
			audiosrc.loop = false;
			signalsInTransit = false;
			firstArm.startColor = Color.green;
			secondArm.startColor = Color.green;
			if (externalAlertSystems == null) {
				externalAlertSystems = FindObjectsOfType<ExternalAlertSystem>();
			}
			foreach (ExternalAlertSystem system in externalAlertSystems) {
				if (system.signals.Count > 0) {
					signalsInTransit = true;
					break;
				}
			}
		}
		foreach (ExternalAlertSystem system in externalAlertSystems) {
			if (system.alarmRaised) {
				firstArm.startColor = Color.red;
				secondArm.startColor = Color.red;
				break;
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
		connectingWireJoint = new Vector3(transform.position.x, transform.position.y, system.transform.position.z);
       	float distance1 = Vector3.Distance(transform.position, connectingWireJoint);
		connectingWireJointRatio = distance1 / (distance1 + Vector3.Distance(connectingWireJoint, system.transform.position));
		
		GameObject firstArmObject = Instantiate(ObjectPrefabDefinitions.main.AlertConnectionSegmentPrefab) as GameObject;
		firstArmObject.name = "Alarm System First Arm";
		GameObject secondArmObject = Instantiate(ObjectPrefabDefinitions.main.AlertConnectionSegmentPrefab) as GameObject;
		secondArmObject.name = "Alarm System Second Arm";
		firstArm = firstArmObject.GetComponent<ParticleSystem>();
		secondArm = secondArmObject.GetComponent<ParticleSystem>();
		firstArm.transform.position = transform.position;
		secondArm.transform.position = connectingWireJoint;
		if (transform.position.z < connectingWireJoint.z) {
			firstArm.transform.eulerAngles = new Vector3(0, 0, 0);
		} else {
			firstArm.transform.eulerAngles = new Vector3(0, 180, 0);
		}
		if (connectingWireJoint.x < system.transform.position.x) {
			secondArm.transform.eulerAngles = new Vector3(0, 90, 0);
		} else {
			secondArm.transform.eulerAngles = new Vector3(0, 270, 0);
		}
		firstArm.startLifetime = probeTimeToAlarm * (connectingWireJointRatio);
		firstArm.startSpeed = (distance1 + Vector3.Distance(connectingWireJoint, system.transform.position)) / probeTimeToAlarm;
		secondArm.startLifetime = probeTimeToAlarm * (1 - connectingWireJointRatio);
		secondArm.startSpeed = (distance1 + Vector3.Distance(connectingWireJoint, system.transform.position)) / probeTimeToAlarm;
	}
	
	public void SignalAlarm(Vector3 location, GameObject sourceObject = null) {
		if (!useAlarmSystem) {
			return;
		}
		if (timeSinceSignalSent < timeBetweenSignals) {
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
		float wireJointTime = signalTimeToAlarm * connectingWireJointRatio;
		List<AlarmSignal> signalsToDestroy = new List<AlarmSignal>();
		foreach (AlarmSignal signal in signals) {
			if (signal.timeAlive > wireJointTime) {
				float secondLegRatio = (signal.timeAlive - wireJointTime) / (signalTimeToAlarm - wireJointTime);
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
