using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class QInteractionUI : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler {
	public QInteractable controlledObject;
	List<GameObject> optionlist;
	public List<string> options;
	float displayIconDisplacement = -8f;
	Color color0 = Color.white;
	Color color1 = Color.white;
	GameObject displayIconObject;
	private GameObject tooltip;
	bool buttonEnabled = true;
	private Image image;
		
	void Start() {
		image = GetComponent<Image>();
	}

	public void GenerateDisplayIcon() {
		displayIconObject = Instantiate (ObjectPrefabDefinitions.main.QDisplayIcon) as GameObject;
		displayIconObject.transform.SetParent(transform);
		displayIconObject.GetComponent<RectTransform>().localPosition =
			new Vector3(displayIconDisplacement, displayIconDisplacement, 0);
	}
	
	void Update () {
		if (image.enabled) {
			float t = Mathf.PingPong(Time.time, 1);
			image.color = Color.Lerp(color0, color1, t);
			image.sprite = controlledObject.GetSprite();
		}
		if (displayIconObject != null) {
			displayIconObject.GetComponent<Image>().enabled = GetComponent<Image>().enabled;
			displayIconObject.GetComponent<Image>().sprite = controlledObject.GetDisplayStatus();
		}
	}
	
	public void OnPointerClick(PointerEventData mouseData) {
		if (mouseData.button == PointerEventData.InputButton.Left && controlledObject.qHasFunctionAccess) {
			controlledObject.Toggle(false);
			OnPointerExit(null);
			OnPointerEnter(null);
		}		
		
		if (mouseData.button == PointerEventData.InputButton.Right && controlledObject.qHasDisplayAccess) {
			controlledObject.Toggle(true);
		}
	}

	//generate tooltip
	public void OnPointerEnter(PointerEventData mouseData){
		if (!buttonEnabled) {
			return;
		}

		tooltip = Instantiate (ObjectPrefabDefinitions.main.Tooltip) as GameObject;
		tooltip.transform.SetParent (transform);
		Text tooltipText = tooltip.GetComponent<Text> ();
		RectTransform recttransform = tooltip.GetComponent<RectTransform>();
		recttransform.localEulerAngles = new Vector3(0f, 0f, 0f);
		recttransform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
		recttransform.anchoredPosition3D = new Vector3(-20f, 10f, 0f);

		//Door Locks
		if (controlledObject.GetComponent<DoorControl> () != null) {
			if (controlledObject.GetComponent<DoorControl> ().isLocked)
				tooltipText.text = ">Unlock Door";
			else
				tooltipText.text = ">Lock Door";
		}
		//Alarm Signals
		else if (controlledObject.GetComponent<AlarmSignal>() != null) {
			tooltipText.text = ">Block signal";
		}
		//Cameras
		else if (controlledObject.GetComponent<CameraControl> () != null) {
			tooltipText.text = ">Enter Camera View";
		}
		//Boxes
		else if (controlledObject.GetComponent<BoxControl> () != null) {
			//Bombs
			if (controlledObject.GetComponent<BoxControl> ().isBomb){
				if (!controlledObject.GetComponent<BoxControl>().isArmed) {
					tooltipText.text = "Disarmed Bomb";
				} else if(controlledObject.GetComponent<BoxControl>().timerSet) {
					tooltipText.text = ">Defuse Bomb";
				} else {
					tooltipText.text = ">Set Off Bomb";
				}
			}
			//Not Bombs
			else
				tooltipText.text = "Box";
		}
		//Alarm
		else if (controlledObject.GetComponent<AlertHub> () != null) {
			if (controlledObject.GetComponent<AlertHub> ().isActive)
				tooltipText.text = ">Disable Alarm";
			else
				tooltipText.text = ">Enable Alarm";
		}
		//Lasers
		else if (controlledObject.GetComponent<LaserBehavior> () != null) {
			tooltipText.text = "Laser";
		}
		else if (controlledObject.GetComponent<PolyLaserParent> () != null) {
			tooltipText.text = "Laser Group";
		}

	}

	//destroy tooltip
	public void OnPointerExit(PointerEventData mouseData){
		if (!buttonEnabled) {
			return;
		}

		Destroy (tooltip);
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
