﻿using UnityEngine;
using System.Collections;

public class ObjectPrefabDefinitions : MonoBehaviour {
	public GameObject Wireframe, WireframeDynamic,
		Laser, PolyLaserChild,
		TagViewSolid, TagViewLaser,
		FoeVisionLineRenderer,
		FoeTaser,
		QDisplayIcon,
		AlarmSignal,
		
		XWall, ZWall, Pillar, Ceiling;
	public static ObjectPrefabDefinitions main;
	
	void Awake() {
		main = this;
	}
}
