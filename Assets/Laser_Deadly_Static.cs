using UnityEngine;
using System.Collections;

public class Laser_Deadly_Static : MonoBehaviour {
	void Start() {
		Color color = Color.red;
		color.a = 0.8f;
		GetComponent<MeshRenderer>().material.color = color;
	}
	
	void OnCollisionEnter(Collision other) {
		if (other.gameObject.layer == Layerdefs.player) {
			print ("GAME OVER");
		}
	}
}
