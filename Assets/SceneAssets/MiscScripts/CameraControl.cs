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
	public int alertTimer = 0;
	public int timeToAlert = 100;
	
	//Stan-Visual
	public MeshRenderer lens;
	public Light light;
	public Color color0;
	public Color color1;

	// Camera switching
	private QCameraControl camControl;
	private QCameraLocation camLocation;
	
	void Start () {
		base.Start();
		myCam = GetComponentInChildren<Camera>();
		planes = GeometryUtility.CalculateFrustumPlanes(myCam);
		Transform temp1 = transform.Find("Camera");
		light = temp1.GetComponentInChildren<Light>();
		color0 = light.color;
		color1 = color0;
		Transform temp = transform.Find("Lens");
		lens = temp.GetComponent<MeshRenderer>();
		lens.material.color = Color.green;

		camControl = GameObject.Find("QCamera").GetComponent<QCameraControl>();
		camLocation = GetComponentInParent<QCameraLocation>();
	}
	
	void Update () {
		float t = Mathf.PingPong(Time.time, 1);
		light.color = Color.Lerp(color0, color1, t);
		
		
		//Hacked in/broken
		if (QIsWatching || QHasBlinded) {
			lens.material.color = Color.black; //light off
			color1 = color0 = Color.green; //camera appears dark
			return;
		} else if (!wasDetected) { //Camera is on alert but hasn't detected Stan
			lens.material.color = Color.red; //appears red (dangerous)
			color1 = color0; //light is a constant yellow
		}
		isDetected = detectStan();
		if (isDetected) {
			wasDetected = isDetected;
			//string message = "You have been detected by camera " + ID; 
			//GameController.SendPlayerMessage(message, 5);
			//Include audio for Q
			color1 = Color.red; //sets 2nd color to red so light will flash
			alertTimerSet = true;
			alertTimer = 0;
		}
		if (alertTimerSet) {
			++alertTimer;
		}
		if (alertTimer >= timeToAlert) {
			if (ID != 0) FoeAlertSystem.Alert(transform.position);
			alertTimerSet = false;
			alertTimer = 0;
		}
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
	
	public override void Trigger () {
		camControl.ChangeCamera(camLocation.cameraNumber);
		displayIsActive = false;
	}
	
	public override Sprite GetSprite () {
		return ButtonSpriteDefinitions.main.cameraIcon;
	}
	
	public override void Tag() {
		GenerateTagVisibility tagScript = GetComponentInChildren<GenerateTagVisibility>();
		tagScript.Tag();
	}
	
	public override void UnTag() {
		GenerateTagVisibility tagScript = GetComponentInChildren<GenerateTagVisibility>();
		tagScript.UnTag();
	}
	
}