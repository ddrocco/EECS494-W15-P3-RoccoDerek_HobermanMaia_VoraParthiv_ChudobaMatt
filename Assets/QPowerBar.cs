using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class QPowerBar : MonoBehaviour {
	Image unused, used;
	public float power = 1f;
	float time = 0f;
	public List<QInteractable> inUse;
	
	void Start () {
		Image[] children = GetComponentsInChildren<Image>();
		inUse = new List<QInteractable>();
		foreach (Image child in children) {
			if (string.Compare(child.gameObject.name,"PowerBarUnused") == 0) {
				unused = child;
			} else if (string.Compare(child.gameObject.name,"PowerBarUsed") == 0) {
				used = child;
			}
		}
		Enabled (false);
	}
	
	// Update is called once per frame
	void Update () {
		//DebugOscillation();
	}
	
	void DebugOscillation() {
		time += Time.deltaTime;
		UpdatePowerLevel(0.5f * Mathf.Cos (time) + 0.5f);
	}
	
	public void UseObject(QInteractable obj) {
		if (inUse.Contains(obj)) {
			return;
		}
		inUse.Add(obj);
		power -= obj.cost;
		UpdatePowerLevel(newPowerLevel: power);
	}
	
	public void DropObject(QInteractable obj) {
		if (!inUse.Contains(obj)) {
			return;
		}
		inUse.Remove(obj);
		power += obj.cost;
		UpdatePowerLevel(newPowerLevel: power);
	}
	
	public void Enabled(bool status) {
		unused.gameObject.SetActive (status);
		used.gameObject.SetActive (status);
	}
	
	void UpdatePowerLevel(float newPowerLevel) {
		power = newPowerLevel;
		unused.transform.localScale = new Vector3(2f * power, 1, 1);
		used.transform.localScale = new Vector3(2f * (1f - power), 1, 1);
	}
}
