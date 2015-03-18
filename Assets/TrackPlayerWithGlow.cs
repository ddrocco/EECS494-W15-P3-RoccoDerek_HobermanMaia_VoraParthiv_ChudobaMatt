using UnityEngine;
using System.Collections;

public class TrackPlayerWithGlow : MonoBehaviour {
	void Update () {
		transform.LookAt(PlayerController.player.transform.position);
		transform.localPosition = transform.right;
	}
}
