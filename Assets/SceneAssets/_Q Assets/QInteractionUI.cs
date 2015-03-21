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
	
	Button a;
	
	
	public GameObject displayIconPrefab;
	public Image displayIcon;

	// Use this for initialization
	void Start () {
		InteractionCanvas = GameObject.Find ("InteractionCanvas");
		optionButton = InteractionCanvas.GetComponent<InteractionCanvasSetup>().OptionButton;
		
		GameObject displayIconObject = Instantiate (displayIconPrefab) as GameObject;
		displayIconObject.transform.SetParent(transform);
		displayIconObject.GetComponent<RectTransform>().localPosition = new Vector3(15, 15, 0);
		displayIcon = displayIconObject.GetComponent<Image>();
	}
	
	public void OnPointerClick(PointerEventData mouseData) {
		print ("Yoooo");
		if (mouseData.button == PointerEventData.InputButton.Left) {
			Debug.Log ("MyButton was clicked with left mouse button.");
			controlledObject.Toggle(true);
		}
		if (mouseData.button == PointerEventData.InputButton.Right) {
			Debug.Log ("MyButton was clicked with right mouse button.");
			controlledObject.Toggle(false);
		}
	}
}
