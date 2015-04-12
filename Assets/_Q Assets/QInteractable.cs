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
	public GameObject QCamera;
	public GameObject QInteractionButton;

	float buttonoffset = 0;
	
	GameObject InteractionCanvas;
	
	public bool qHasFunctionAccess = true;
	public bool functionIsActive = false;
	public float functionCost = 0;
	
	public bool displayIsNil = false;
	public bool qHasDisplayAccess = true;
	public bool displayIsActive = false;
	public float displayCost = 0;
	
	Color activeColor = new Color(1f, 0.4f, 0.4f);
	Color inactiveColor = new Color(0.2f, 0.2f, 0.2f);
	
	public GameObject tagView;
	
	public virtual void Start () {
		InteractionCanvas = GameObject.Find ("InteractionCanvas");

		//Generate and position button
		QInteractionButton = Instantiate (ObjectPrefabDefinitions.main.QInteractiveButton);
		QInteractionButton.GetComponent<RectTransform> ().localPosition =
			new Vector3 (transform.position.x + buttonoffset, InteractionCanvas.GetComponent<RectTransform> ().localPosition.y, transform.position.z + buttonoffset);
		QInteractionButton.GetComponent<RectTransform> ().localRotation = InteractionCanvas.GetComponent<RectTransform> ().localRotation;
		QInteractionButton.GetComponent<QInteractionUI> ().controlledObject = this;
		QInteractionButton.transform.SetParent (InteractionCanvas.transform);
		
		//Generate Stan-visible (3d) tag object
		GameObject tagPrefab = GetStanVisibleTag();
		if (tagPrefab != null) {
			tagView = Instantiate(GetStanVisibleTag(), transform.position, Quaternion.identity) as GameObject;
			tagView.transform.parent = transform;
			tagView.transform.localScale = Vector3.one;
			tagView.transform.localEulerAngles = Vector3.zero;
			if (GetComponent<MeshFilter>() != null && tagView.GetComponent<MeshFilter>() != null) {
				tagView.GetComponent<MeshFilter>().mesh = GetComponent<MeshFilter>().mesh;
			}
			QInteractionButton.GetComponent<QInteractionUI>().GenerateDisplayIcon();
		} else {
			displayIsNil = true;
		}
	}
	
	public virtual GameObject GetStanVisibleTag() {
		return ObjectPrefabDefinitions.main.TagViewSolid;
	}
	
	public void Toggle (bool toggleDisplay) {
		if (toggleDisplay && qHasDisplayAccess) {
			if (!displayIsActive && FindObjectOfType<QPowerSystem>().AddObject(this, false)) {
				Tag();
				displayIsActive = true;
			} else if (displayIsActive && FindObjectOfType<QPowerSystem>().DropObject(this, false)) {
				UnTag();
				displayIsActive = false;
			}
		} else if (!toggleDisplay && qHasFunctionAccess) {
			if (!functionIsActive && FindObjectOfType<QPowerSystem>().AddObject(this, true)) {
				functionIsActive = true;
				Trigger();
			} else if (functionIsActive && FindObjectOfType<QPowerSystem>().DropObject(this, true)) {
				functionIsActive = false;
				Trigger();
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
			return GetAnimationFrame(ButtonSpriteDefinitions.main.displayTaggedAnimation);
		} else {
			return ButtonSpriteDefinitions.main.DisplayVisible;
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
	
	public virtual void enableButtonView() {
		QInteractionButton.GetComponent<Image>().enabled = true;
	}
	
	public static Sprite GetAnimationFrame(List<Sprite> sprites) {
		float switchRate = 10f;
		
		int time = Mathf.FloorToInt(Time.time * switchRate);
		int index = time % sprites.Count;
		print (time + " " + index);
		return sprites[index];
	}
}
