using UnityEngine;
using System.Collections;

public class Generate_Wireframe : MonoBehaviour {
	public int type = 0;
	public int group = 0;
	public bool isDynamic;
	
	void Start () {
		GameObject wireframe;
		if (isDynamic) {
			wireframe = Instantiate(ObjectPrefabDefinitions.main
					.WireframeDynamic, transform.position, Quaternion.identity) as GameObject;
		} else {
			wireframe = Instantiate(ObjectPrefabDefinitions.main
                    .Wireframe, transform.position, Quaternion.identity) as GameObject;
		}
		wireframe.transform.parent = transform;
		wireframe.transform.localScale = Vector3.one;
		wireframe.GetComponent<MeshFilter>().mesh = GetComponent<MeshFilter>().mesh;
		if (type == 1) { //dangerous
			wireframe.GetComponent<Renderer>().material.color = Color.red;
		} else if (type == 2) { //good
			wireframe.GetComponent<Renderer>().material.color = Color.cyan;
		} else if (type == 4) {
			wireframe.GetComponent<Renderer>().material.color = Color.red;
			wireframe.GetComponent<Renderer>().enabled = false;
		}
	}
}
