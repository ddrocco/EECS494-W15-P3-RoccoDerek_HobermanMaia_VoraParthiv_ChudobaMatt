﻿using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class QCameraLocation : MonoBehaviour {
	public float minRotation;
	public float maxRotation;
	public float zoom;
	public bool usable = false;
	public bool Offline;
	public Quaternion start, finish;
	public bool rotate;
	public bool allowTagging = true;
	public bool toggleDisplayMessageforQ = false;
	public Canvas QMessage;
	private CameraControl thisCam = null;
	
	[HideInInspector]
	public int cameraNumber;
	
	void Start() {
		GameObject securityCamera = Instantiate(ObjectPrefabDefinitions.main.SecurityCamera);
		securityCamera.transform.parent = transform;
		securityCamera.transform.localPosition = new Vector3(0, 0, -.2f);
		securityCamera.transform.localEulerAngles = new Vector3(20, 0, 0);
		thisCam = securityCamera.GetComponent<CameraControl>();
		thisCam.QIsWatching = false;
		thisCam.rotate = rotate;
		if (toggleDisplayMessageforQ) {
			securityCamera.AddComponent<DisplayForQ>();
			securityCamera.GetComponent<DisplayForQ>().message = QMessage;
		}
		if (Offline) {
			thisCam.Offline = true;
			start = finish = Quaternion.Euler(transform.rotation.eulerAngles.x,
			                                  transform.rotation.eulerAngles.y,
			                                  transform.rotation.eulerAngles.z);
		} else {
			start = Quaternion.Euler(transform.rotation.eulerAngles.x,
			                         transform.rotation.eulerAngles.y - 30,
			                         transform.rotation.eulerAngles.z);
			finish = Quaternion.Euler(transform.rotation.eulerAngles.x,
			                          transform.rotation.eulerAngles.y + 30,
			                          transform.rotation.eulerAngles.z);
		}
	}
	
	void Update() {
		if (rotate && !thisCam.QIsWatching) {
			float t = Mathf.PingPong(Time.time/4, 1f);
			transform.rotation = Quaternion.Lerp(start, finish, t);
		}
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