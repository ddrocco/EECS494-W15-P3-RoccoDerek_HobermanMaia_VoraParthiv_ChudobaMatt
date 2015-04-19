using UnityEngine;
using System.Collections;

public class AlarmSignal : QInteractable {
	public float timeAlive = 0;
	public bool shouldDestroy = false;
	public Vector3 detectionLocation;
	public GameObject sourceObject;
	
	public override Sprite GetSprite () {
		return ButtonSpriteDefinitions.main.AlarmSignal;
	}
	
	public override void Trigger () {
		//Destroy (this.gameObject);
		transform.GetComponentInParent<ExternalAlertSystem>().RemoveActiveSignal(this);
	}
	
	void Update() {
		timeAlive += 3*Time.deltaTime/4f;
		QInteractionButton.transform.position = new Vector3(transform.position.x,
		                                                    QInteractionButton.transform.position.y,
		                                                    transform.position.z);
	}
	
	void OnDestroy () {
		Destroy (QInteractionButton.gameObject);
	}
}
