using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public enum MapGroup
{
	One,
	Two,
	Three
};

public class QInteractable : MonoBehaviour {
	public MapGroup group;
	public float functionCost;
	public float displayCost;
	public GameObject QCamera;
	public GameObject QInteractionButton;

	float buttonoffset = 0;
	
	GameObject InteractionCanvas;
	
	
	public bool functionIsActive = false;
	public bool displayIsActive = false;
	public float cost = 0;
	
	Color activeColor = new Color(1f, 0.4f, 0.4f);
	
	public GameObject tagView;
	
	public virtual void Start () {
		InteractionCanvas = GameObject.Find ("InteractionCanvas");

		QInteractionButton = Instantiate (ObjectPrefabDefinitions.main.QInteractiveButton);
		QInteractionButton.GetComponent<RectTransform> ().localPosition =
			new Vector3 (transform.position.x + buttonoffset, InteractionCanvas.GetComponent<RectTransform> ().localPosition.y, transform.position.z + buttonoffset);
		QInteractionButton.GetComponent<RectTransform> ().localRotation = InteractionCanvas.GetComponent<RectTransform> ().localRotation;
		QInteractionButton.GetComponent<QInteractionUI> ().controlledObject = this;
		QInteractionButton.transform.SetParent (InteractionCanvas.transform);
		QInteractionButton.GetComponent<Image>().sprite = GetSprite();
		
		tagView = Instantiate(TagPrefab(), transform.position, Quaternion.identity) as GameObject;
		tagView.transform.parent = transform;
		tagView.transform.localScale = Vector3.one;
		tagView.transform.localEulerAngles = Vector3.zero;
		if (GetComponent<MeshFilter>() != null && tagView.GetComponent<MeshFilter>() != null) {
			tagView.GetComponent<MeshFilter>().mesh = GetComponent<MeshFilter>().mesh;
		}
	}
	
	public virtual GameObject TagPrefab() {
		return ObjectPrefabDefinitions.main.TagViewSolid;
	}
	
	public void Toggle (bool toggleDisplay) {
		if (toggleDisplay) {
			if (!displayIsActive && FindObjectOfType<QPowerSystem>().AddObject(this, false)) {
				Tag();
				displayIsActive = true;
				//Eyecon stuff
			} else if (displayIsActive && FindObjectOfType<QPowerSystem>().DropObject(this, false)) {
				UnTag();
				displayIsActive = false;
				//Eyecon stuff
			}
		} else {
			if ((!functionIsActive && FindObjectOfType<QPowerSystem>().AddObject(this, true))
			    || (functionIsActive && FindObjectOfType<QPowerSystem>().DropObject(this, true))) {
				functionIsActive = !functionIsActive;
				Trigger();
				QInteractionButton.GetComponent<Image>().sprite = GetSprite();
				if (functionIsActive) {
					QInteractionButton.GetComponent<Image>().color = activeColor;
				} else {
					QInteractionButton.GetComponent<Image>().color = Color.white;
				}
			}
		}
	}

	public virtual void Trigger() {
		
	}
	public virtual Sprite GetSprite() {
		return null;
	}
	public virtual Sprite GetDisplayStatus() {
		if (displayIsActive) {
			return ButtonSpriteDefinitions.main.displayHighlight;
		} else {
			return ButtonSpriteDefinitions.main.displayVisible;
		}
	}
		
	public virtual void Tag() {
		if (tagView.GetComponent<MeshRenderer>() != null) {
			tagView.GetComponent<MeshRenderer>().enabled = true;
		}
		tagView.GetComponent<ParticleSystemRenderer>().enabled = true;
	}
	public virtual void UnTag() {
		if (tagView.GetComponent<MeshRenderer>() != null) {
			tagView.GetComponent<MeshRenderer>().enabled = false;
		}
		tagView.GetComponent<ParticleSystemRenderer>().enabled = false;
	}
	
	public void disableButtonView() {
		QInteractionButton.GetComponent<Image>().enabled = false;
	}
	
	public void enableButtonView() {
		QInteractionButton.GetComponent<Image>().enabled = true;
	}
}
