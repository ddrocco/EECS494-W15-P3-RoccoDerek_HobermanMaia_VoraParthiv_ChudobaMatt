using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour {
	public bool QIsWatching = true;
	public bool QHasBlinded = false;
	private GameObject player;
	private int cullingMask;
	
	void Start () {
		player = GameObject.Find("Player");
		cullingMask = (1 << Layerdefs.wall) + (1 << Layerdefs.floor)
			+ (1 << Layerdefs.interactable) + (1 << Layerdefs.door);
	}
	
	int GetPlayerRaycasts() {
		Vector3[] playerVertices = player.GetComponent<Player_Vertices>().GetVertices();
		
		int visibleVertices = 0;
		foreach (Vector3 vertex in playerVertices) {
			RaycastHit hitInfo;
			bool raycastHit = Physics.Raycast(
				transform.position,
				(vertex - transform.position),
				out hitInfo,
				(vertex - transform.position).magnitude,
				cullingMask);
			if (!raycastHit) {
				++visibleVertices;
				//Debug.DrawRay (transform.position, (vertex - transform.position), Color.green);
			} else {
				//Debug.DrawRay (transform.position, (vertex - transform.position), Color.magenta);
			}
		}
		return visibleVertices;
	}
}
