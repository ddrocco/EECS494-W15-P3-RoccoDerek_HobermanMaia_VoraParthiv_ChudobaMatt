using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class QInteractionUI : MonoBehaviour, IPointerClickHandler {
	public QInteractable controlledObject;
	public GameObject optionButton;
	GameObject InteractionCanvas;
	List<GameObject> optionlist;
	public List<string> options;	
	public GameObject displayIconPrefab;
	public Image displayIcon;
	float displayIconDisplacement = 10f;

	// Use this for initialization
	void Start () {
		InteractionCanvas = GameObject.Find ("InteractionCanvas");
		optionButton = InteractionCanvas.GetComponent<InteractionCanvasSetup>().OptionButton;
		
		GameObject displayIconObject = Instantiate (displayIconPrefab) as GameObject;
		displayIconObject.transform.SetParent(transform);
		displayIconObject.GetComponent<RectTransform>().localPosition = new Vector3(displayIconDisplacement, displayIconDisplacement, 0);
		displayIcon = displayIconObject.GetComponent<Image>();
		displayIcon.enabled = false;
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
}
