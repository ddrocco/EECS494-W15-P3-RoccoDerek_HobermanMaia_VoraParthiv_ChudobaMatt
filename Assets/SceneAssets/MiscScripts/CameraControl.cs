using UnityEngine;
using System.Collections;

public class CameraControl : QInteractable {
	//Identification
	public int ID;
	
	//Detection
	public bool QIsWatching = true;
	public bool QHasBlinded = false;
	public bool isDetected = false;
	private Plane[] planes;
	private Camera myCam;
	
	//Alert
	public bool alertTimerSet = false;
	public int alertTimer = 0;
	public int timeToAlert = 100;
	
	//Stan-Visual
	public MeshRenderer lens;

	// Camera switching
	private QCameraControl camControl;
	private QCameraLocation camLocation;
	
	void Start () {
		base.Start();
		myCam = GetComponentInChildren<Camera>();
		planes = GeometryUtility.CalculateFrustumPlanes(myCam);
		camControl = GameObject.Find("QCamera").GetComponent<QCameraControl>();
		camLocation = GetComponentInParent<QCameraLocation>();
	}
	
	void Update () {
		if (QIsWatching || QHasBlinded) {
			lens.material.color = Color.green;
			return;
		} else {
			lens.material.color = Color.red;
		}
		isDetected = detectStan();
		if (isDetected) {
			//GameController.SendPlayerMessage("You have been detected by a camera!", 5);
			//Include visual warning for Stan and audio for Q
			alertTimerSet = true;
			alertTimer = 0;
		}
		if (alertTimerSet) {
			++alertTimer;
		}
		if (alertTimer >= timeToAlert) {
			FoeAlertSystem.Alert(transform.position);
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
		return ButtonSpriteDefinitions.main.camera;
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
