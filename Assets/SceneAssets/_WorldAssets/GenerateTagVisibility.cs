using UnityEngine;
using System.Collections;

public class GenerateTagVisibility : MonoBehaviour {
	public GameObject tagViewPrefab;
	public GameObject tagView;
	
	void Start () {
		tagView = Instantiate(tagViewPrefab, transform.position, Quaternion.identity) as GameObject;
		tagView.transform.parent = transform;
		tagView.transform.localScale = Vector3.one;
		tagView.transform.eulerAngles = Vector3.zero;
		tagView.GetComponent<MeshFilter>().mesh = GetComponent<MeshFilter>().mesh;
	}
	
	public void Tag() {
		tagView.GetComponent<MeshRenderer>().enabled = true;
		tagView.GetComponent<ParticleSystemRenderer>().enabled = true;
	}
	public void UnTag() {
		tagView.GetComponent<MeshRenderer>().enabled = false;
		tagView.GetComponent<ParticleSystemRenderer>().enabled = false;
	}
}
