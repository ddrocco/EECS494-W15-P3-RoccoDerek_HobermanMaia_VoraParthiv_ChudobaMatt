using UnityEngine;
using System.Collections;

public class QMovement : MonoBehaviour {

	public QPan qpan;
	public QMouseLook qlook;
	public SmoothFollow qsnap;

	// Use this for initialization
	void Start () {
		qpan = this.GetComponent<QPan> ();
		qlook = this.GetComponent<QMouseLook> ();
		qsnap = this.GetComponent<SmoothFollow> ();

		qpan.enabled = true;
		qlook.enabled = true;
		qsnap.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		float snapping = Input.GetAxis ("QSnap");
		if(snapping > 0.5){
			qpan.enabled = false;
			qlook.enabled = false;
			qsnap.enabled = true;
		} else {
			qpan.enabled = true;
			qlook.enabled = true;
			qsnap.enabled = false;
		}
	}
}
