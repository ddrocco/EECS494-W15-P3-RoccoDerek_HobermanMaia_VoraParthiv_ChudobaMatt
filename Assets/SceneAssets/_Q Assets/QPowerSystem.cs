using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class QPowerSystem : MonoBehaviour {
	Image unused, used;
	public float power = 1f;
	float time = 0f;
	public List<QInteractable> inUse;
	public List<QInteractable> inDisplay;
	
	public static QPowerSystem main;
	
	void Start () {
		main = this;
		
		Image[] children = GetComponentsInChildren<Image>();
		inUse = new List<QInteractable>();
		inDisplay = new List<QInteractable>();
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
	
	public bool AddObject(QInteractable obj, bool functionality) {
		if (functionality) {
			if (inUse.Contains(obj) || power < obj.functionCost) {
				return false;
			}
			inUse.Add(obj);
			power -= obj.functionCost;
		} else {
			if (inDisplay.Contains(obj) || power < obj.displayCost) {
				return false;
			}
			inDisplay.Add(obj);
			power -= obj.displayCost;
		}
		UpdatePowerLevel(newPowerLevel: power);
		return true;
	}
	
	public bool DropObject(QInteractable obj, bool functionality) {
		if (functionality){
			if (!inUse.Contains(obj)) {
				return false;
			}
			inUse.Remove(obj);
			power += obj.functionCost;
		} else {
			if (!inDisplay.Contains(obj)) {
				return false;
			}
			inDisplay.Remove(obj);
			power += obj.displayCost;
		}
		UpdatePowerLevel(newPowerLevel: power);
		return true;
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
