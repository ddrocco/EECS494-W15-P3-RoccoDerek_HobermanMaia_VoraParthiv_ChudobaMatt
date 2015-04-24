using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BrokenLightParent : MonoBehaviour {
	List<float> timers, shutdownTimers;
	bool active = false;
	
	void Start() {
		timers = new List<float>();
		shutdownTimers = new List<float>();
	}
	
	void Update () {
		if (!active) {
			return;
		}
		for (int i = 0; i < timers.Count; ++i) {
			timers[i] -= Time.deltaTime;
			if (shutdownTimers[i] != 0) {
				shutdownTimers[i] -= Time.deltaTime;
				if (shutdownTimers[i] <= 0) {
					shutdownTimers[i] = 0;
					GetComponentsInChildren<Light>()[i].enabled = false;
					AudioSource.PlayClipAtPoint(AudioDefinitions.main.ShortCircuit,
							GetComponentsInChildren<Light>()[i].transform.position);
				}
			}
			if (timers[i] <= 0f) {
				timers[i] = Random.Range (15f, 45f);
				GetComponentsInChildren<Light>()[i].enabled = true;
				shutdownTimers[i] = 0.5f;
			}
		}
	}
	
	public void Lightpocalypse() {
		foreach (Light light in GetComponentsInChildren<Light>()) {
			Vector3 pos = light.transform.localPosition;
			pos.y = 0f;
			light.transform.localPosition = pos;
			Renderer rend = light.transform.parent.FindChild("Light").GetComponent<Renderer>();
			rend.enabled = false;
			light.enabled = false;
			GameObject sparks = Instantiate(ObjectPrefabDefinitions.main.Sparks, light.transform.position, Quaternion.identity) as GameObject;
			sparks.transform.parent = light.transform;
			timers.Add (Random.Range(0f,15f));
			shutdownTimers.Add (0f);
			sparks.GetComponent<ParticleSystem>().time = timers[timers.Count - 1];
			AudioSource.PlayClipAtPoint(AudioDefinitions.main.ShortCircuit, light.transform.position);
		}
		active = true;
	}
}
