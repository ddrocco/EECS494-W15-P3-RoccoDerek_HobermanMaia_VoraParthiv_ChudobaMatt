using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class QInteractionUI : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler {
	public QInteractable controlledObject;
	List<GameObject> optionlist;
	public List<string> options;
	public Color color0 = Color.white;
	public Color color1 = Color.white;
	private GameObject tooltip;
	bool buttonEnabled = true;
	private Image image;
	private AudioSource audiosrc;
		
	void Start() {
		image = GetComponent<Image>();
		audiosrc = GetComponent<AudioSource>();
	}
	
	void Update () {
		if (image.enabled) {
			float t = Mathf.PingPong(Time.time, 1);
			image.color = Color.Lerp(color0, color1, t);
			image.sprite = controlledObject.GetSprite();
		}
	}
	
	public void OnPointerClick(PointerEventData mouseData) {
		if (mouseData.button == PointerEventData.InputButton.Left) {
			if (controlledObject.qHasFunctionAccess) {
				controlledObject.Toggle(false);
				OnPointerExit(null);
				OnPointerEnter(null);
				audiosrc.clip = AudioDefinitions.main.QValidAction;
				audiosrc.Play();
			} else {
				audiosrc.clip = AudioDefinitions.main.QInvalidAction;
				audiosrc.Play();
			}
		}
		
		if (mouseData.button == PointerEventData.InputButton.Right) {
			if (controlledObject.qHasDisplayAccess) {
				controlledObject.Toggle(true);
				if (controlledObject.displayIsActive) {
					audiosrc.clip = AudioDefinitions.main.QObjectTagged;
					audiosrc.Play();
				} else {
					audiosrc.clip = AudioDefinitions.main.QObjectUntagged;
					audiosrc.Play();
				}
			} else {
				audiosrc.clip = AudioDefinitions.main.QInvalidAction;
				audiosrc.Play();
			}
		}
	}

	//generate tooltip
	public void OnPointerEnter(PointerEventData mouseData){
		tooltip = Instantiate (ObjectPrefabDefinitions.main.Tooltip) as GameObject;
		tooltip.transform.SetParent (transform);
		Text tooltipText = tooltip.GetComponent<Text> ();
		RectTransform recttransform = tooltip.GetComponent<RectTransform>();
		recttransform.localEulerAngles = new Vector3(0f, 0f, 0f);
		recttransform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
		recttransform.anchoredPosition3D = new Vector3(-20f, 10f, 0f);
		tooltip.transform.SetParent (transform.parent);
		//Door Locks
		if (controlledObject.GetComponent<DoorControl> () != null) {
			if(buttonEnabled){
				if (controlledObject.GetComponent<DoorControl> ().isLocked)
					tooltipText.text = ">Unlock Door";
				else
					tooltipText.text = ">Lock Door";
			} else {
				tooltipText.text = "Door";
			}
		}
		//Paper piles
		else if (controlledObject.GetComponent<InformationForPlayer>() != null) {
			tooltipText.text = "Data";
		}
		//Computers
		else if (controlledObject.GetComponent<ComputerConsole>() != null) {
			tooltipText.text = "Computer";
		}
		else if (controlledObject.GetComponent<UselessDataComputer>() != null) {
			tooltipText.text = "Computer";
		}
		//Elevators
		else if (controlledObject.GetComponent<ElevatorControl>() != null) {
			tooltipText.text = "Elevator";
		}
		//Alarm Signals
		else if (controlledObject.GetComponent<AlarmSignal>() != null) {
			if(buttonEnabled){
				tooltipText.text = ">Block signal";
			} else {
				tooltipText.text = "Alarm Signal";
			}
		}
		//Cameras
		else if (controlledObject.GetComponent<CameraControl> () != null) {
			if (controlledObject.GetComponent<CameraControl>().QIsWatching) {
				tooltipText.text = ">Enter Camera View";
			} else {
				tooltipText.text = "Camera";
			}
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
			if(buttonEnabled){
				if (controlledObject.GetComponent<AlertHub> ().isActive)
					tooltipText.text = ">Disable Alarm";
				else
					tooltipText.text = ">Enable Alarm";
			} else {
				tooltipText.text = "Alarm System";
			}
		}
		//Lasers
		else if (controlledObject.GetComponent<LaserBehavior> () != null) {
			tooltipText.text = "Laser";
		}
	}

	//destroy tooltip
	public void OnPointerExit(PointerEventData mouseData){
		if (tooltip == null) {
			return;
		}

		Destroy (tooltip);
	}

	public void OnDestroy(){
		if (tooltip == null) {
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
		if (buttonEnabled) {
			color0 = color1 = Color.white;
		} else {
			color0 = color1 = Color.grey;
		}
	}
}
