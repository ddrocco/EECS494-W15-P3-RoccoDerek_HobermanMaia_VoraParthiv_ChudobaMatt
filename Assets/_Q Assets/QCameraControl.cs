using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class QCameraControl : MonoBehaviour
{
	//Sound things:
	/*
	public List<AudioClip> steps;
	public AudioSource source;
	public int currentStep = 0;
	public bool isPanning = false;
	*/
	
	public Vector3 pivotPoint;
	public float UD_rotation, LR_rotation, zoom;
	public float rotationSpeed = 200f;
	public float zoomSpeed = 50f;
	public float zoomMin = 10f;
	public float zoomMax = 50f;

	//[HideInInspector]
	public static int camCount;

	private Camera cam;
	private QCameraLocation currentCam;
	private static List<QCameraLocation> cameras;
	private QCameraOverview camOverview;
	
	public int overviewCullingMask;
	public int cameraCullingMask;

	//Alert flashing
	public Color color0 = Color.black;
	public Color color1 = Color.black;	
	public bool warning = false;
	public bool alerting = false;

	// Use this for initialization
	void Awake()
	{
		GetComponent<AudioSource>().enabled = false;
		
		pivotPoint = FindObjectOfType<PlayerController>().transform.position;
		cam = GetComponent<Camera>();
		cameras = new List<QCameraLocation>();
		camOverview = GameObject.Find("CamOverview").GetComponent<QCameraOverview>();

		GameObject[] objs = GameObject.FindGameObjectsWithTag("CameraLocation");
		foreach (GameObject obj in objs)
		{
			QCameraLocation temp = obj.GetComponent<QCameraLocation>();
			if (temp != null) {
				cameras.Add(temp);
			}
		}
		CameraComp comp = new CameraComp();
		cameras.Sort(comp);
		camCount = cameras.Count;

		// Internal numbering of cameras
		for (int i = 0; i < camCount; i++)
			cameras[i].cameraNumber = i + 1;

		currentCam = camOverview;
		cam.orthographic = true;

		overviewCullingMask = (1 << Layerdefs.ui) + (1 << Layerdefs.q_visible) + (1 << Layerdefs.laser);
		cameraCullingMask = FindObjectOfType<PlayerCameraFollow>().GetComponent<Camera>().cullingMask
				+ (1 << Layerdefs.laser);
	}

	void Start() {
		InitializeSprites();
	}

	public void DisableCameras()
	{
		camCount = 0;
		//cameras[0].usable = true;
		//cameras[1].usable = true;
		
		for (int i = 0; i < cameras.Count; ++i)
		{
			CameraControl control =
				cameras[i].gameObject.GetComponentInChildren<CameraControl>();
			if (control == null) {
				continue;
			}
			control.QIsWatching = false;
			//control.disableButtonView();
		}
	}
	
	// Update is called once per frame
	void Update()
	{
		if (PauseScript.GamePaused) return;
		
		float t = Mathf.PingPong(Time.time, 1f);
		cam.backgroundColor = Color.Lerp(color0, color1, t);
	
		GetCameraInput();		
		UpdateCameraPosition();
		SwitchToOverview();
	}

	// Toggles a camera on or off to be able to be used by Q
	// camNumber is the camera you want to enable/disable (starting at 1, not 0)
	// newState is true if you want to activate the chosen camera, false if not
	public void ToggleCamera(int camNumber, bool newState)
	{
		if (camNumber == 0)
		{
			Debug.LogWarning("ToggleCamera(int, bool): 0 passed in for camNumber; only >1 allowed");
			return;
		}
		if (camNumber > cameras.Count)
		{
			Debug.LogError("Q Camera index out of range: ToggleCamera(int, bool)");
			return;
		}

		if (newState)
		{
			if (cameras[camNumber - 1].usable == false)
			{
				cameras[camNumber - 1].usable = true;
				CameraControl control =
					cameras[camNumber - 1].gameObject.GetComponentInChildren<CameraControl>();
				control.enableButtonView();
				camCount++;
			}
		}
		else
		{
			if (cameras[camNumber - 1].usable == true)
			{
				cameras[camNumber - 1].usable = false;
				CameraControl control =
					cameras[camNumber - 1].gameObject.GetComponentInChildren<CameraControl>();
				//control.disableButtonView();
				camCount--;
			}
		}
	}

	void GetCameraInput()
	{
		if (currentCam == camOverview)
		{
			return;
		}

		//rotate
		if (Input.GetKey(KeyCode.A))
		{
			LR_rotation -= rotationSpeed * Time.deltaTime;
		}
		if (Input.GetKey(KeyCode.D))
		{
			LR_rotation += rotationSpeed * Time.deltaTime;
		}

		//zoom
		if (Input.GetKey(KeyCode.W))
		{
			zoom -= zoomSpeed * Time.deltaTime;
		}
		if (Input.GetKey(KeyCode.S))
		{
			zoom += zoomSpeed * Time.deltaTime;
		}
	}
	
	void UpdateCameraPosition()
	{
		if (currentCam == camOverview)
		{
			return;
		}

		if (LR_rotation > 360f)
		{
			LR_rotation -= 360f;
		}
		if (LR_rotation < 0f)
		{
			LR_rotation += 360f;
		}

		if (LR_rotation > currentCam.maxRotation) LR_rotation = currentCam.maxRotation;
		if (LR_rotation < currentCam.minRotation) LR_rotation = currentCam.minRotation;
		
		if (zoom < zoomMin)
		{
			zoom = zoomMin;
		}
		if (zoom > zoomMax)
		{
			zoom = zoomMax;
		}

		currentCam.transform.rotation = Quaternion.Euler(new Vector3(UD_rotation, LR_rotation, 0));
		transform.rotation = currentCam.transform.rotation;
		currentCam.zoom = zoom;
		cam.fieldOfView = zoom;
	}

	void SwitchToOverview()
	{
		if (currentCam == camOverview) return;

		if (Input.GetKeyDown(KeyCode.Mouse0))
		{
			currentCam = camOverview;
			cam.orthographic = true;
			camOverview.camActive = true;
			cam.cullingMask = overviewCullingMask;

			transform.position = currentCam.transform.position;
			transform.rotation = currentCam.transform.rotation;
			UD_rotation = currentCam.transform.eulerAngles.x;
			LR_rotation = currentCam.transform.eulerAngles.y;
			zoom = currentCam.zoom;
		}
	}

	public void ChangeCamera(int camNumber)
	{
		if (cameras.Count >= camNumber && cameras[camNumber - 1].usable)
		{
			currentCam = cameras[camNumber - 1];
		}
		else
			return;

		cam.orthographic = false;
		camOverview.camActive = false;
		cam.cullingMask = cameraCullingMask;

		transform.position = currentCam.transform.position;
		transform.rotation = currentCam.transform.rotation;
		UD_rotation = currentCam.transform.eulerAngles.x;
		LR_rotation = currentCam.transform.eulerAngles.y;
		zoom = currentCam.zoom;
	}
	
	//Call AlertOn to cause the background to flash red
	public void AlertOn() {
		color1 = new Color(60f/255f, 10f/255f, 10f/255f, 1f);
		alerting = true;
		warning = false;
		GetComponent<AudioSource>().enabled = true;
	}
	
	//Call AlertOn to cause the background to flash yellow
	public void WarningOn() {
		color1 = new Color(60f/255f, 60f/255f, 0f/255f, 1f);
		warning = true;
		alerting = false;
	}
	
	//Call AlertOff to turn off the flashing alert
	public void AlertOff() {
		color1 = Color.black;
		warning = alerting = false;
		GetComponent<AudioSource>().enabled = false;
		foreach (QInteractable ctr in FindObjectsOfType<QInteractable>()) {
			ctr.QInteractionButton.GetComponent<QInteractionUI>().AlertOff();
		}
		foreach (CameraControl ctr in FindObjectsOfType<CameraControl>()) {
			ctr.wasDetected = false;
		}
	}
	
	void InitializeSprites() {
		int value = 1;
		int enabledValue = 0;
		MapGroup group = MapGroup.One;
		
		foreach (GenerateQRenderer renderObject in FindObjectsOfType<GenerateQRenderer>()) {
			renderObject.Activate(enabledValue);
		}
		foreach (QInteractable interactableObj in FindObjectsOfType<QInteractable>()) {
			if (interactableObj.group == group) {
				interactableObj.enableButtonView();
				interactableObj.QInteractionButton.GetComponent<QInteractionUI>().SetEnabled(true);
				interactableObj.qHasFunctionAccess = true;
				if (interactableObj.objectIsTaggable) {
					interactableObj.qHasDisplayAccess = true;
				}
			} else if (interactableObj.unusableGroup == group) {
				interactableObj.enableButtonView();
				interactableObj.qHasFunctionAccess = false;
				interactableObj.QInteractionButton.GetComponent<QInteractionUI>().SetEnabled(false);
				if (interactableObj.objectIsTaggable) {
					interactableObj.qHasDisplayAccess = true;
				}
			}
		}
	}
}