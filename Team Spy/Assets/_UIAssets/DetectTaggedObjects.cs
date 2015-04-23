using UnityEngine;
using System.Collections;

public class DetectTaggedObjects : MonoBehaviour {
	public bool tagCompassVisible = false;
	public GameObject taggedObject = null;
	public MeshFilter taggedMesh = null;
	private Camera myCam = null;
	
	void Start() {
		myCam = GetComponent<Camera>();
	}
	
	void Update () {
		if (taggedObject == null || taggedMesh == null) {
			if (tagCompassVisible) {
				FindObjectOfType<PlayerTagCompass>().isVisible = false;
				FindObjectOfType<PlayerTagCompass>().pic.enabled = false;
			}
			tagCompassVisible = false;
			return;
		}
		/*Plane[] planes = GeometryUtility.CalculateFrustumPlanes(myCam);
		if (GeometryUtility.TestPlanesAABB(planes, taggedMesh.mesh.bounds)) {
			tagCompassVisible = false;
		} else {
			tagCompassVisible = true;
		}
		return;*/
		tagCompassVisible = true;
		return;
	}
}
