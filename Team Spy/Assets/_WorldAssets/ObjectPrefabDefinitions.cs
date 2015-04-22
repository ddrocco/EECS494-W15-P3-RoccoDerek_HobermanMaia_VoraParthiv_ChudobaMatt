﻿using UnityEngine;
using System.Collections;

public class ObjectPrefabDefinitions : MonoBehaviour {
	public GameObject Wireframe, WireframeDynamic, QRenderer,
		Laser, PolyLaserChild,
		TagViewSolid, TagViewLaser,
		FoeTaser,
		QInteractiveButton, QDisplayIcon, Tooltip,
		AlarmSignal, AlertConnectionSegmentPrefab,
		SecurityCamera, CameraArrowIcon,
		Explosion, ExplosionSmoke, Flare, Sparks,
		XWall, ZWall, Pillar, Ceiling;
		
	public Material alertConnection;
	public static ObjectPrefabDefinitions main;
	
	void Awake() {
		main = this;
	}
}
