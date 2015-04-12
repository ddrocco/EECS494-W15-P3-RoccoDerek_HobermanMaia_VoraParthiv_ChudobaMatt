using UnityEngine;
using System.Collections;

public class ObjectPrefabDefinitions : MonoBehaviour {
	public GameObject Wireframe, WireframeDynamic, QRenderer,
		Laser, PolyLaserChild,
		TagViewSolid, TagViewLaser,
		FoeVisionLineRenderer,
		FoeTaser, FoeNavigator,
		QInteractiveButton, QDisplayIcon, Tooltip,
		AlarmSignal,
		SecurityCamera,
		Explosion, ExplosionSmoke,
		XWall, ZWall, Pillar, Ceiling;
		
	public Material alertConnection;
	public static ObjectPrefabDefinitions main;
	
	void Awake() {
		main = this;
	}
}
