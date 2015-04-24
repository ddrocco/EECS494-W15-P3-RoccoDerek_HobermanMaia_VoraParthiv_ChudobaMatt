using UnityEngine;
using System.Collections;

public class DetectTaggedObjects : MonoBehaviour {
	public bool taggedObjectValid = false;
	public GameObject taggedObject = null;
	public MeshFilter taggedMesh = null;
	
	void Update () {
		if (taggedObject == null || taggedMesh == null) {
			if (taggedObjectValid) {
				FindObjectOfType<PlayerTagCompass>().isVisible = false;
				FindObjectOfType<PlayerTagCompass>().pic.enabled = false;
			}
			taggedObjectValid = false;
			return;
		}
		taggedObjectValid = true;
		return;
	}

}
