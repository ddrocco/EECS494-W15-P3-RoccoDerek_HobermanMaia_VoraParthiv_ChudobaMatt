using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class QCameraLocation : MonoBehaviour
{
	public float minRotation;
	public float maxRotation;
	public string description;

	[HideInInspector]
	public float zoom;
	[HideInInspector]
	public int cameraNumber;

	void Awake()
	{
		zoom = 60f;
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