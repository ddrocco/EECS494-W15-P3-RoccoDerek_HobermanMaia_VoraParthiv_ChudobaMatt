using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class QInteractionUI : MonoBehaviour, IPointerClickHandler {
	public QInteractable controlledObject;
	List<GameObject> optionlist;
	public List<string> options;
	public Image displayIcon;
	float displayIconDisplacement = 10f;
	Color color0 = Color.white;
	Color color1 = Color.white;
	GameObject displayIconObject;
	public bool hasDisplayIcon;
	bool buttonEnabled = true;

	// Use this for initialization
	void Start () {
		if (controlledObject.GetComponent<AlertHub>() != null) {
			hasDisplayIcon = false;
		} else {
			hasDisplayIcon = true;
		}
		
		if (hasDisplayIcon) {
			displayIconObject = Instantiate (ObjectPrefabDefinitions.main.QDisplayIcon) as GameObject;
			displayIconObject.transform.SetParent(transform);
			displayIconObject.GetComponent<RectTransform>().localPosition = new Vector3(displayIconDisplacement, displayIconDisplacement, 0);
			displayIcon = displayIconObject.GetComponent<Image>();
			displayIcon.sprite = controlledObject.GetDisplayStatus();
		}
	}
	
	void Update () {
		float t = Mathf.PingPong(Time.time, 1);
		Image temp = GetComponent<Image>();
		temp.color = Color.Lerp(color0, color1, t);
		temp.sprite = controlledObject.GetSprite();
		if (displayIconObject != null) {
			displayIconObject.GetComponent<Image>().enabled = temp.enabled;
		}
	}
	
	public void OnPointerClick(PointerEventData mouseData) {
		if (!buttonEnabled) {
			return;
		}
		
		if (mouseData.button == PointerEventData.InputButton.Left) {
			controlledObject.Toggle(false);
		}
		if (mouseData.button == PointerEventData.InputButton.Right) {
			controlledObject.Toggle(true);
			displayIcon.sprite = controlledObject.GetDisplayStatus();
		}
	}
	
	public void SetEnabled(bool newEnabledState) {
		buttonEnabled = newEnabledState;
		if (buttonEnabled) {
			color0 = Color.white;
			if (color1 == Color.gray) {
				color1 = Color.white;
			}
		} else {
			color0 = Color.gray;
			color1 = Color.gray;
		}
	}
	
	//Call AlertOn to cause an icon to flash red
	public void AlertOn() {
		color1 = Color.red;
	}
	
	public void InUseOn() {
		color1 = Color.green;
	}
	
	//Call AlertOff to turn off the flashing alert
	public void AlertOff() {
		color1 = Color.white;
	}
}
