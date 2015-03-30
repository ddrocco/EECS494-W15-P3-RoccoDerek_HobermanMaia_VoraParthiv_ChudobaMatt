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

	// Use this for initialization
	void Start () {		
		GameObject displayIconObject = Instantiate (ObjectPrefabDefinitions.main.QDisplayIcon) as GameObject;
		displayIconObject.transform.SetParent(transform);
		displayIconObject.GetComponent<RectTransform>().localPosition = new Vector3(displayIconDisplacement, displayIconDisplacement, 0);
		displayIcon = displayIconObject.GetComponent<Image>();
		displayIcon.enabled = false;
	}
	
	void Update () {
		float t = Mathf.PingPong(Time.time, 1);
		GetComponent<Image>().color = Color.Lerp(color0, color1, t);
		GetComponent<Image>().sprite = controlledObject.GetSprite();
	}
	
	public void OnPointerClick(PointerEventData mouseData) {
		if (mouseData.button == PointerEventData.InputButton.Left) {
			controlledObject.Toggle(false);
		}
		if (mouseData.button == PointerEventData.InputButton.Right) {
			controlledObject.Toggle(true);
			displayIcon.enabled = controlledObject.displayIsActive;
		}
	}
	
	//Call AlertOn to cause an icon to flash red
	public void AlertOn() {
		color1 = Color.red;
	}
	
	//Call AlertOff to turn off the flashing alert
	public void AlertOff() {
		color1 = Color.white;
	}
}
