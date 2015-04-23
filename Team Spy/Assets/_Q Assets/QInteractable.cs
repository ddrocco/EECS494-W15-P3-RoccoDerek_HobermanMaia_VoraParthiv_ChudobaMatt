using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public enum MapGroup
{
	One,
	Two,
	Three,
	Never
};

public class QInteractable : MonoBehaviour {
	public MapGroup group = MapGroup.One;
	public MapGroup unusableGroup = MapGroup.Never;
	public GameObject QCamera;
	public GameObject QInteractionButton;

	float buttonoffset = 0;
	
	GameObject InteractionCanvas;
	
	public bool qHasFunctionAccess = true;
	public bool functionIsActive = false;
	public float functionCost = 0;
	
	public bool objectIsTaggable = true;
	public bool qHasDisplayAccess = true;
	public bool displayIsActive = false;
	public float displayCost = 0;
	
	//Color activeColor = new Color(1f, 0.4f, 0.4f);
	//Color inactiveColor = new Color(0.2f, 0.2f, 0.2f);
	
	public GameObject tagView;
	static QInteractable taggedButton = null;
	static GameObject qTagPrefab = null;
	PlayerTagCompass tagCompass;
	
	public virtual void Start () {		
		InteractionCanvas = GameObject.Find ("InteractionCanvas");

		//Generate and position button
		QInteractionButton = Instantiate (ObjectPrefabDefinitions.main.QInteractiveButton);
		QInteractionButton.GetComponent<RectTransform> ().localPosition =
			new Vector3 (transform.position.x + buttonoffset,
			             InteractionCanvas.GetComponent<RectTransform> ().localPosition.y,
			             transform.position.z + buttonoffset);
		QInteractionButton.GetComponent<RectTransform> ().localRotation = 
			InteractionCanvas.GetComponent<RectTransform> ().localRotation;
		QInteractionButton.GetComponent<QInteractionUI> ().controlledObject = this;
		QInteractionButton.transform.SetParent (InteractionCanvas.transform);
		
		//Generate Stan-visible (3d) tag object
		GameObject tagPrefab = GetStanVisibleTag();
		if (tagPrefab == null) {
			objectIsTaggable = false;
		}
		if (objectIsTaggable) {
			tagView = Instantiate(GetStanVisibleTag(), transform.position, Quaternion.identity)
				as GameObject;
			tagView.transform.parent = transform;
			tagView.transform.localScale = Vector3.one;
			tagView.transform.localEulerAngles = Vector3.zero;
			if (GetComponent<MeshFilter>() != null && tagView.GetComponent<MeshFilter>() != null) {
				tagView.GetComponent<MeshFilter>().mesh = GetComponent<MeshFilter>().mesh;
			}
		}
		
		tagCompass = FindObjectOfType<PlayerTagCompass>();
		if (group != MapGroup.One) {
			disableButtonView();
		}
		
		UnTag();
	}
	
	public virtual GameObject GetStanVisibleTag() {
		return ObjectPrefabDefinitions.main.TagViewSolid;
	}
	
	public void Toggle (bool toggleDisplay) {
		if (toggleDisplay && qHasDisplayAccess) {
			if (!displayIsActive) {
				Tag();
				displayIsActive = true;
			} else if (displayIsActive) {
				UnTag();
				displayIsActive = false;
			}
		} else if (!toggleDisplay && qHasFunctionAccess) {
			if (!functionIsActive) {
				functionIsActive = true;
				Trigger();
			} else if (functionIsActive) {
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
	
	void LateUpdate() {
		if (!displayIsActive) {
			return;
		}
		var player = FindObjectOfType<PlayerCameraFollow>();
		Vector3 playerLookDirection = player.transform.forward;
		playerLookDirection.y = 0;
		Vector3 playerPos = player.transform.position;
		playerPos.y = 0;
		Vector3 thisPos = new Vector3(transform.position.x, 0, transform.position.z);
		
		Quaternion lookRotation = Quaternion.FromToRotation(thisPos-playerPos, playerLookDirection);
		
		tagCompass.SetDirection(lookRotation);
	}
		
	public virtual void Tag() {
		displayIsActive = true;
		if (tagView.GetComponent<MeshRenderer>() != null) {
			tagView.GetComponent<MeshRenderer>().enabled = true;
		}
		tagView.GetComponent<ParticleSystemRenderer>().enabled = true;
		if (taggedButton != null) {
			taggedButton.UnTag();
		}
		taggedButton = this;
		qTagPrefab = Instantiate(ObjectPrefabDefinitions.main.QDisplayIcon,
				QInteractionButton.transform.position + Vector3.up,
				QInteractionButton.transform.rotation) as GameObject;
		FindObjectOfType<DetectTaggedObjects>().taggedObject = tagView;
		FindObjectOfType<DetectTaggedObjects>().taggedMesh = tagView.GetComponent<MeshFilter>();
	}
	public virtual void UnTag() {
		if (taggedButton != this) {
			return;
		}
		displayIsActive = false;
		if (tagView.GetComponent<MeshRenderer>() != null) {
			tagView.GetComponent<MeshRenderer>().enabled = false;
		}
		tagView.GetComponent<ParticleSystemRenderer>().enabled = false;
		taggedButton = null;
		FindObjectOfType<DetectTaggedObjects>().taggedObject = null;
		FindObjectOfType<DetectTaggedObjects>().taggedMesh = null;
		Destroy(qTagPrefab);
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
		return sprites[index];
	}
}
