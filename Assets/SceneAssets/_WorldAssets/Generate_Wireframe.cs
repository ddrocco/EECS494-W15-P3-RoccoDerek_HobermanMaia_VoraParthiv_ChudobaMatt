using UnityEngine;
using System.Collections;

public class Generate_Wireframe : MonoBehaviour {
	public GameObject wireframePrefab;
	public int type = 0;
	public int group = 0;
	
	void Start () {
		GameObject wireframe = Instantiate(wireframePrefab, transform.position, Quaternion.identity) as GameObject;
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
