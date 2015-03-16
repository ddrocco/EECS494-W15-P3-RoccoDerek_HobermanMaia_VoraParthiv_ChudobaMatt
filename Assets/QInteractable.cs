using UnityEngine;
using System.Collections;

public class QInteractable : MonoBehaviour {
	public float cost;
	float time = 0f;
	
	QPowerBar bar;
	
	// Use this for initialization
	void Start () {
		bar = FindObjectOfType<QPowerBar>();
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
}
