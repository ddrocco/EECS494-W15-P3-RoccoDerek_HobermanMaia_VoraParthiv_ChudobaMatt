using UnityEngine;
using System.Collections;

public class Foe_Glance_Command : MonoBehaviour {
	public float timer = 0;
	public float angleMin, angleMax, totalAngle;
	public float startPauseOne, endPauseOne, startPauseTwo, endPauseTwo, fullDuration;
	
	public bool isActive;
	
	//remove later
	public bool recvCmd = false;

	void Start () {
		isActive = false;
	}
	
	void Update () {
		//remove later
		if (recvCmd) {
			recvCmd = false;
			ReceiveGlanceCommand(4f, 1f, -40f, 40f);
		}
	
		if (!isActive) {
			return;
		}
		
		timer += Time.deltaTime;
		float angle = 0;
		
		if (timer < startPauseOne) {
			angle = angleMin * (timer) / (startPauseOne);
		} else if (timer < endPauseOne) {
			angle = angleMin;
		} else if (timer < startPauseTwo) {
			angle = angleMin + totalAngle * (timer - endPauseOne) / (startPauseTwo - endPauseOne);
		} else if (timer < endPauseTwo) {
			angle = angleMax;
		} else if (timer < fullDuration) {
			angle = angleMax * (fullDuration - timer) / (fullDuration - endPauseTwo);
		} else {
			angle = 0;
			isActive = false;
		}
		
		transform.localEulerAngles = new Vector3(0, angle, 0);
	}
	
	public void ReceiveGlanceCommand(
			float totalTime,
			float pauseDuration,
			float leftAngle,
			float rightAngle) {
		isActive = true;
		timer = 0;
		
		angleMin = leftAngle;
		angleMax = rightAngle;		
		totalAngle = angleMax - angleMin;
		
		float totalMotionTime = totalTime - 2 * pauseDuration;
		startPauseOne = totalMotionTime * (-angleMin / totalAngle) / 2;
		endPauseOne = startPauseOne + pauseDuration;
		endPauseTwo = totalTime - totalMotionTime * (angleMax / totalAngle) / 2;
		startPauseTwo = endPauseTwo - pauseDuration;
		fullDuration = totalTime;
	}
}
