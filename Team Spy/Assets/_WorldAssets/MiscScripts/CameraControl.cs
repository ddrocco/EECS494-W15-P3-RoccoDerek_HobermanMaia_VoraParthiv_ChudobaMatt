using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CameraControl : QInteractable {
	//Identification
	public int ID;
	
	//Detection
	public bool QIsWatching = false;
	public bool isBlinded = false;
	public bool Offline = false;
	public bool wasDetected = false;
	private Plane[] planes;
	private Camera myCam;
	private GameObject player;
	public bool rotate;
	
	//Alert
	private ExternalAlertSystem alertSystem;

	//Stan-Visual
	public MeshRenderer lens;
	public Light alertLight;
	public Color yellow = new Color(220f/255f, 170f/255f, 30f/255f, 1);
	public Color color0 = new Color(220f/255f, 170f/255f, 30f/255f, 1);
	public Color color1 = new Color(220f/255f, 170f/255f, 30f/255f, 1);
	public Color green;

	// Camera switching
	private QCameraControl camControl;
	private QCameraLocation camLocation;
	private QInteractionUI interactionUI;
	
	private GameObject QArrowIcon;
	
	public override void Start () {
	
		//Setting up detection
		myCam = GetComponentInChildren<Camera>();
		planes = GeometryUtility.CalculateFrustumPlanes(myCam);
		myCam.enabled = false;
		alertSystem = GetComponentInChildren<ExternalAlertSystem>();
		player = PlayerController.player.gameObject;
		
		//Setting up appearance
		Transform temp1 = transform.Find("Camera");
		alertLight = temp1.GetComponentInChildren<Light>();
		color0 = new Color(220f/255f, 170f/255f, 30f/255f, 1);
		color1 = color0;
		Transform temp = transform.Find("Lens");
		lens = temp.GetComponent<MeshRenderer>();
		lens.material.color = Color.green;
		
		//Setting up switching
		camControl = GameObject.Find("QCamera").GetComponent<QCameraControl>();
		camLocation = GetComponentInParent<QCameraLocation>();
		
		//Setting up Q-button
		base.Start();

		interactionUI = QInteractionButton.GetComponent<QInteractionUI>();
	}
	
	void Update () {
		float t = Mathf.PingPong(Time.time, 1);
		if (alertLight.enabled) {
			alertLight.color = Color.Lerp(color0, color1, t);
		}
		if (QArrowIcon != null) {
			QArrowIcon.transform.localEulerAngles = new Vector3(0, 180, transform.eulerAngles.y);
		}
		
		//Hacked in/broken
		if (QIsWatching || isBlinded) {
			if (wasDetected) {
				if (!alertSystem.signalsInTransit && !alertSystem.alarmRaised) {
					camControl.AlertOff();
					color1 = color0 = yellow;
					wasDetected = false;
				}
			}
			lens.material.color = Color.black; //lens off
			alertLight.enabled = false;
			interactionUI.AlertOff();
			return;
		} else if (!wasDetected) { //Camera is on alert but hasn't detected Stan
			lens.material.color = Color.red; //appears red (dangerous)
		}
		Vector3 detectionLocation = Vector3.down;
		if (Vector3.Distance(player.transform.position, transform.position) <= 11f) { //if player in range
			detectionLocation  = detectStan();	//(0, -1, 0) on faiure to detect
		}
		if (detectionLocation != Vector3.down) {
			if (!wasDetected) {
				camControl.WarningOn();
				wasDetected = true;
				color1 = Color.red;
			}
			if (!Offline) {
				alertSystem.SignalAlarm(detectionLocation, this.gameObject);
			}
		} else { //not currently detecting,
			if (wasDetected) { //but warning/alert system was on
				if (!alertSystem.signalsInTransit && !alertSystem.alarmRaised) {
					camControl.AlertOff();
					color1 = color0 = yellow;
					wasDetected = false;
				}
			}
		}
	}
	
	//Uses child camera and raycast to see if Stan is in view
	//Returns location of Stan if detected, and Vector3.down if not.
	Vector3 detectStan () {
		if (rotate) {
			myCam.enabled = true;
			planes = GeometryUtility.CalculateFrustumPlanes(myCam);
			myCam.enabled = false;
		}
		if (GeometryUtility.TestPlanesAABB(planes, player.GetComponent<Collider>().bounds)) {
			RaycastHit hit;
			Vector3 heading = PlayerController.player.transform.position - transform.position;
			float distance = heading.magnitude;
			Vector3 direction = heading/distance;
			if (distance > 4.25f && Physics.Raycast(transform.position, direction, out hit, distance)) {
				if (hit.collider.CompareTag("Player") == true) {
					if (!wasDetected) {
						if (GetComponent<DisplayForQ>() != null) {
							GetComponent<DisplayForQ>().SendMessage();
						}
						AudioSource.PlayClipAtPoint(Foe_Detection_Handler.SelectRandomClip(AudioDefinitions.main.CameraSpotsPlayer),
								transform.position);
					}
					return new Vector3(hit.point.x, 0, hit.point.z);
				} else return Vector3.down;
			} else return Vector3.down;
		} else return Vector3.down;
	}
	
	void SubdueCameraAlert() {
		GameController.SendPlayerMessage("Your partner has successfully turned off the camera alert...I just hope it was in time!", 5);
	}
	
	/*void HackCamera() {
		CameraControl obj = GetComponent<CameraControl>();
		obj.QIsWatching = true;
		camControl.ToggleCamera(obj.ID, true);
	}*/
	
	public void Interact() {
		//camControl.ToggleCamera(camLocation.cameraNumber, true);		
		isBlinded = true;
		/*if (camControl.warning || camControl.alerting) {
			camControl.AlertOff();
		}*/
		tag = "Untagged";
	}
	
	public override void Trigger () {
		camControl.ChangeCamera(camLocation.cameraNumber);
		displayIsActive = false;
	}
	
	public override Sprite GetSprite () {
		if (QIsWatching) {
			return ButtonSpriteDefinitions.main.CameraUnderQ;
		} else if (isBlinded) {
			return ButtonSpriteDefinitions.main.CameraBlinded;
		} else {
			return ButtonSpriteDefinitions.main.CameraOnAlert;
		}
	}
	
	public override void enableButtonView() {
		QInteractionButton.GetComponent<Image>().enabled = true;
		GetComponentInChildren<LineRenderer>().enabled = true;
		
		QArrowIcon = Instantiate (ObjectPrefabDefinitions.main.CameraArrowIcon) as GameObject;
		QArrowIcon.transform.SetParent(QInteractionButton.transform);
		QArrowIcon.transform.localScale = new Vector3(1.025f, 1.025f, 1.025f);
		QArrowIcon.transform.localEulerAngles = new Vector3(0, 180, transform.eulerAngles.y);
		QArrowIcon.transform.localPosition = Vector3.back;
		QArrowIcon.GetComponent<Image>().sprite = ButtonSpriteDefinitions.main.CameraArrow;
	}
}