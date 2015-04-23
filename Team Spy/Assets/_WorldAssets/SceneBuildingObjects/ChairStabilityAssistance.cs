using UnityEngine;
using System.Collections;

public class ChairStabilityAssistance : MonoBehaviour {
	Rigidbody rigid;
	
	void Start() {
		rigid = GetComponent<Rigidbody>();
	}
	
	void Update () {
		rigid.AddForce(20f * Vector3.down);
	}
}
