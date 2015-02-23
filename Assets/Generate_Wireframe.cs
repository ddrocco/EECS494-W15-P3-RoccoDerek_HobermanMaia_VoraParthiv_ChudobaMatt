using UnityEngine;
using System.Collections;

public class Generate_Wireframe : MonoBehaviour {
	public GameObject wireframePrefab;
	
	void Start () {
		GameObject wireframe = Instantiate(wireframePrefab, transform.position, Quaternion.identity) as GameObject;
		wireframe.transform.parent = transform;
		wireframe.transform.localScale = Vector3.one;
	}
}
