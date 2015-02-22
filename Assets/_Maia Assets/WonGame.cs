using UnityEngine;
using System.Collections;

public class WonGame : MonoBehaviour {

	void OnTriggerEnter(Collider other) {
		if  (other.gameObject.layer == Layerdefs.player) {
			//endgame
		}
	}
}
