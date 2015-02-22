using UnityEngine;
using System.Collections;

public class Foe_Navigation_Helper : MonoBehaviour {
	public GameObject target;
	
	void Start () {
		GetComponent<NavMeshAgent>().destination = target.transform.position;
	}
	
	void Update () {
	
	}
}
