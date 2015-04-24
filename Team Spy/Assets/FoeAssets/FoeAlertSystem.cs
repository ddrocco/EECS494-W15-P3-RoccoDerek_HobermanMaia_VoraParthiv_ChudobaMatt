using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FoeAlertSystem : MonoBehaviour {
	public static float playerRange = 0.5f;
	public static float bombRange = 3f;
	
	public static void Alert(Vector3 position, bool isPlayer) {
		float minDist = 100000f;
		Foe_Detection_Handler closest = null;
		foreach (Foe_Detection_Handler foe in FindObjectsOfType<Foe_Detection_Handler>()) {
			if (foe.enabled) {
			    float dist = Vector3.Distance(position, foe.transform.position);
			    if (dist < minDist) {
				    closest = foe;
					minDist = dist;
				}
			}
		}
		if (closest == null) {
			GuardSpawner spawner = FindObjectOfType<GuardSpawner>();
			if (spawner == null) {
				return;
			}
			GameObject newFoe = Instantiate(ObjectPrefabDefinitions.main.Foe, spawner.transform.position,
				spawner.transform.rotation) as GameObject;
			closest = newFoe.GetComponentInChildren<Foe_Detection_Handler>();
		}
		closest.movementHandler.StartInvestigation(position, isPlayer);
		closest.isAggressive = true;
	}
}
