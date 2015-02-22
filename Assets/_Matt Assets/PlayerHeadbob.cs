using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerHeadbob : MonoBehaviour {
	public List<AudioClip> walkSteps, runSteps;

	Vector3 baseLocalPosition;
	
	public List<float> walkStepDurations, runStepDurations;
	public float currentTime = float.MaxValue;
	
	int currentStep = 0;
	public float walkStepHeight;
	public float runStepHeight;
	
	void Start () {	
		baseLocalPosition = transform.localPosition;
		foreach (AudioClip stepSound in walkSteps) {
			walkStepDurations.Add (stepSound.length);
		}
		foreach (AudioClip stepSound in runSteps) {
			runStepDurations.Add (stepSound.length);
		}
	}
	
	void Update () {
		float ratio = 0;
		float stepHeight = 0f;
		//if (walking) {
			ratio = currentTime / walkStepDurations[currentStep];
			stepHeight = walkStepHeight;
		/*} else if (running) {
			ratio = currentTime / runStepDurations[currentStep];
			stepHeight = runStepHeight;
		}*/
		if (ratio < 1f) {
			float yDisplace = (1f - Mathf.Cos(ratio * 2f * Mathf.PI)) * stepHeight;
			transform.localPosition = baseLocalPosition + yDisplace * Vector3.up;
			currentTime += Time.deltaTime;
		}
	}
}
