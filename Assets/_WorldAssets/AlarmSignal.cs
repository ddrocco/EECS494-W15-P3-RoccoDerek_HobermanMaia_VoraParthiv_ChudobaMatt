using UnityEngine;
using System.Collections;

public class AlarmSignal : QInteractable {
	public float timeAlive = 0;
	public bool shouldDestroy = false;
	
	public Vector3 detectionLocation;
	
	public override Sprite GetSprite () {
		return ButtonSpriteDefinitions.main.AlarmSignal;
	}
	
	public override void Trigger () {
		Destroy (this.gameObject);
	}
	
	void Update() {
		timeAlive += 3*Time.deltaTime/4f;
	}
	
	void OnDestroy () {
		Destroy (QInteractionButton.gameObject);
		transform.GetComponentInParent<ExternalAlertSystem>().RemoveActiveSignal(this);
	}
}
