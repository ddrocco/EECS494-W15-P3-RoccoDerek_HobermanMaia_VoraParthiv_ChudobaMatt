using UnityEngine;
using System.Collections;

public class Generate_Wireframe : MonoBehaviour {
	public GameObject wireframePrefab;
	public int type = 0;
	
	void Start () {
		GameObject wireframe = Instantiate(wireframePrefab, transform.position, Quaternion.identity) as GameObject;
		wireframe.transform.parent = transform;
		wireframe.transform.localScale = Vector3.one;
		wireframe.GetComponent<MeshFilter>().mesh = GetComponent<MeshFilter>().mesh;
		if (type == 1) { //dangerous
			wireframe.renderer.material.color = Color.red;
		} else if (type == 2) { //good
			wireframe.renderer.material.color = Color.cyan;
		}
	}
}
