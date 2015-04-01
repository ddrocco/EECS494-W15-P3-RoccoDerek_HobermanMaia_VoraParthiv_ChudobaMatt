using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class QCameraLocation : MonoBehaviour
{
	public float minRotation;
	public float maxRotation;
	public string description;
	public float zoom;
	public bool usable = false;

	[HideInInspector]
	public int cameraNumber;
	
	void Start() {
		GameObject securityCamera = Instantiate(ObjectPrefabDefinitions.main.SecurityCamera);
		securityCamera.transform.parent = transform;
		securityCamera.transform.localPosition = Vector3.zero;
		securityCamera.transform.localEulerAngles = Vector3.zero;
		securityCamera.GetComponent<CameraControl>().QIsWatching = false;
	}
}

public class CameraComp : IComparer<QCameraLocation>
{
	public int Compare(QCameraLocation x, QCameraLocation y)
	{
		int numX = Convert.ToInt32(x.name.Substring(3));
		int numY = Convert.ToInt32(y.name.Substring(3));
		if (numX < numY) return -1;
		if (numX > numY) return 1;
		return 0;
	}
}