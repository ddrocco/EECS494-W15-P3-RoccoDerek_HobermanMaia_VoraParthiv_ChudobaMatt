using UnityEngine;
using System.Collections;

public class PlayerCameraFollow : MonoBehaviour
{
	private GameObject player;

	void Start()
	{
		player = GameObject.Find("Player");
	}

	void Update()
	{
		// Fix camera position to player position
		Vector3 pos = player.transform.position;
		pos.y += .75f;//player.transform.lossyScale.y;
		transform.position = pos;

		// Fix camera y rotation to player y rotation
		Vector3 rot = transform.eulerAngles;
		rot.y = player.transform.eulerAngles.y;
		transform.eulerAngles = rot;
	}
}
