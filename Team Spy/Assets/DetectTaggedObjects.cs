﻿using UnityEngine;
using System.Collections;

public class DetectTaggedObjects : MonoBehaviour {
	public GameObject taggedObject = null;
	public static bool tagCompassVisible = false;
	
	void Update () {
		if (taggedObject == null) {
			tagCompassVisible = false;
			return;
		}
		Camera myCam = GetComponent<Camera>();
		Plane[] planes = GeometryUtility.CalculateFrustumPlanes(myCam);
		if (!GeometryUtility.TestPlanesAABB(planes, taggedObject.GetComponent<MeshFilter>().mesh.bounds)) {
			tagCompassVisible = false;
			print ("NOPE");
			return;
		}
		tagCompassVisible = true;
		print ("YEP");
	}
}
