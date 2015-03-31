using UnityEngine;
using System.Collections;

public class CameraControl : QInteractable {
	//Identification
	public int ID;
	
	//Detection
	public bool QIsWatching = true;
	public bool QHasBlinded = false;
	public bool isDetected = false;
	public bool wasDetected = false;
	private Plane[] planes;
	private Camera myCam;
	
	//Alert
	public bool alertTimerSet = false;
	public float timeBetweenSignals = 0.5f;
	public float timeSinceLastSignal = 0f;
	
	//Stan-Visual
	public MeshRenderer lens;
	public Light alertLight;
	public Color color0;
	public Color color1;

	// Camera switching
	private QCameraControl camControl;
	private QCameraLocation camLocation;
	
	public override void Start () {
		myCam = GetComponentInChildren<Camera>();
		planes = GeometryUtility.CalculateFrustumPlanes(myCam);
		Transform temp1 = transform.Find("Camera");
		alertLight = temp1.GetComponentInChildren<Light>();
		color0 = alertLight.color;
		color1 = color0;
		Transform temp = transform.Find("Lens");
		lens = temp.GetComponent<MeshRenderer>();
		lens.material.color = Color.green;

		camControl = GameObject.Find("QCamera").GetComponent<QCameraControl>();
		camLocation = GetComponentInParent<QCameraLocation>();
		base.Start();
	}
	
	void Update () {
		float t = Mathf.PingPong(Time.time, 1);
		alertLight.color = Color.Lerp(color0, color1, t);
		
		//Hacked in/broken
		if (QIsWatching || QHasBlinded) {
			lens.material.color = Color.black; //light off
			color1 = color0 = Color.green; //camera appears dark
			QInteractionButton.GetComponent<QInteractionUI>().AlertOff();
			return;
		} else if (!wasDetected) { //Camera is on alert but hasn't detected Stan
			lens.material.color = Color.red; //appears red (dangerous)
			color1 = color0; //light is a constant yellow
		}
		isDetected = detectStan();
		if (isDetected && timeSinceLastSignal >= timeBetweenSignals) {
			GetComponent<ExternalAlertSystem>().SignalAlarm();
			color1 = Color.red; //sets 2nd color to red so light will flash
			QInteractionButton.GetComponent<QInteractionUI>().AlertOn();
			timeSinceLastSignal = 0f;
		}
		timeSinceLastSignal += Time.deltaTime;
	}
	
	//Uses child camera and raycast to see if Stan is in view
	bool detectStan () {
		if (GeometryUtility.TestPlanesAABB(planes, PlayerController.player.GetComponent<Collider>().bounds)) {
			RaycastHit hit;
			Vector3 heading = PlayerController.player.transform.position - transform.position;
			float distance = heading.magnitude;
			Vector3 direction = heading/distance;
			if (Physics.Raycast(transform.position, direction, out hit, distance)) {
				if (hit.collider.CompareTag("Player") == true) {
					return true;
				} else return false;
			} else return false;
		} else return false;
	}
	
	void SubdueCameraAlert() {
		GameController.SendPlayerMessage("Your partner has successfully turned off the camera alert...I just hope it was in time!", 5);
	}
	
	void HackCamera() {
		CameraControl obj = GetComponent<CameraControl>();
		obj.QIsWatching = true;
		QCameraControl camObj = QCamera.GetComponent<QCameraControl>();
		camObj.ToggleCamera(obj.ID, true);
	}
	
	public override void Trigger () {
		camControl.ChangeCamera(camLocation.cameraNumber);
		displayIsActive = false;
	}
	
	public override Sprite GetSprite () {
		return ButtonSpriteDefinitions.main.cameraIcon;
	}	
}