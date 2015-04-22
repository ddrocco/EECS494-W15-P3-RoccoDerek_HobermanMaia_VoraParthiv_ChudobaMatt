using UnityEngine;
using System.Collections;

public class CeilingControl : QInteractable {

	public override void Start() {
		transform.GetComponent<MeshRenderer>().enabled = true;
		base.Start();
		tagView.transform.localPosition = new Vector3(0, -15, 0);
		tagView.transform.localScale = new Vector3(.25f, .01f, .25f);
	}
	
	public void Interact () {
		return;
	}
	public override void Trigger () {
		return;
	}
	
	void OnDestroy() {
		transform.GetComponent<MeshRenderer>().enabled = false;
	}
	
	public override Sprite GetSprite () {
		return ButtonSpriteDefinitions.main.BlankIcon;
	}
}
