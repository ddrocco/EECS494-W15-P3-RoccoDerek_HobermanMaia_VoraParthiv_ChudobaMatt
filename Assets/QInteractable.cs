using UnityEngine;
using System.Collections;

public class QInteractable : MonoBehaviour {
	public float cost;
	float time = 0f;
	
	QPowerSystem bar;
	
	public enum Type {
		box,
		door,
		laser,
		guard
	};
	
	public Type type;
	
	// Use this for initialization
	void Start () {
		bar = FindObjectOfType<QPowerSystem>();
	}
	
	// Update is called once per frame
	void Update () {
		time += Time.deltaTime;
		if (time > 4f) {
			time = 0f;
			bar.DropObject(this);
		}
		if (time > 2f) {
			bar.UseObject(this);
		}
	}
	
	public void QSelect () {
		switch(type) {
			case Type.door:
				//Command lock: Doorlock();
				//etc
				break;
		}
	}
	
	void BoxDisplay() {
	
	}
	void DoorLock() {
	
	}
	void DoorUnlock() {
	
	}
}
