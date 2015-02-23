using UnityEngine;
using System.Collections;

public class WonGame : MonoBehaviour {
	public GameObject QCamera;

	void OnTriggerEnter(Collider other) {
		if  (other.gameObject.layer == Layerdefs.player) {
			QCamera.GetComponent<QUI>().setText("Level complete. Good teamwork!");
		}
	}
}
