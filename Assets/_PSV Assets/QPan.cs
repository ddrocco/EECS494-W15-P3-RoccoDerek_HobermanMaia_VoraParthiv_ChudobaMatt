using UnityEngine;
using System.Collections;

public class QPan : MonoBehaviour {

	public float strafespeed = 1f;
	public float zoomspeed = 1f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		float qforward = Input.GetAxis ("QForward");
		float qside = Input.GetAxis ("QSide");

		this.transform.Translate (new Vector3 (qside * strafespeed, 0, qforward * zoomspeed));
	}
}
