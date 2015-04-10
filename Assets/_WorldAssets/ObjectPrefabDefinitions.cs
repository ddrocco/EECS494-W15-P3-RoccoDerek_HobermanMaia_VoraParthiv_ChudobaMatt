using UnityEngine;
using System.Collections;

public class ObjectPrefabDefinitions : MonoBehaviour {
	public GameObject Wireframe, WireframeDynamic,
		Laser, PolyLaserChild,
		TagViewSolid, TagViewLaser,
		FoeVisionLineRenderer,
		FoeTaser,
		QInteractiveButton, QDisplayIcon, Tooltip,
		AlarmSignal,
		SecurityCamera,
		XWall, ZWall, Pillar, Ceiling;
		
	public Material alertConnection;
	public static ObjectPrefabDefinitions main;
	
	void Awake() {
		main = this;
	}
}
