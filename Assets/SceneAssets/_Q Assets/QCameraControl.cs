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
	
	public GameObject player;
	public Vector3 pivotPoint;
	public float UD_rotation, LR_rotation, zoom;
	public float rotationSpeed = 200f;
	public float zoomSpeed = 50f;
	public float zoomMin = 10f;
	public float zoomMax = 50f;
	public Text cameraDesc;

	[HideInInspector]
	public int camCount;

	private Camera cam;
	private QCameraLocation currentCam;
	private QCameraLocation lastUsedCam;
	private List<QCameraLocation> cameras;
	private QCameraOverview camOverview;
	
	public int overviewCullingMask;
	public int cameraCullingMask;

	// Use this for initialization
	void Awake()
	{
		//Sound stuff:
		/*
		source = GetComponent<AudioSource>();
		currentStep = 0;
		isPanning = false;
		audioSource = GetComponent<AudioSource>();
		audioSource.enabled = false;
		*/
		pivotPoint = player.transform.position;
		cam = GetComponent<Camera>();
		cameras = new List<QCameraLocation>();
		camOverview = GameObject.Find("CamOverview").GetComponent<QCameraOverview>();
		camOverview.camActive = false;

		GameObject[] objs = GameObject.FindGameObjectsWithTag("CameraLocation");
		foreach (GameObject obj in objs)
		{
			QCameraLocation temp = obj.GetComponent<QCameraLocation>();
			if (temp != null)
				cameras.Add(temp);
		}
		CameraComp comp = new CameraComp();
		cameras.Sort(comp);
		camCount = cameras.Count;

		// Internal numbering of cameras
		for (int i = 0; i < camCount; i++)
			cameras[i].cameraNumber = i + 1;

		currentCam = camOverview;
		lastUsedCam = cameras[0];
		cam.orthographic = true;
		camOverview.camActive = true;
		cameraDesc.text = "Camera 0\n" + currentCam.description;
		
		overviewCullingMask = (1 << Layerdefs.ui) + (1 << Layerdefs.q_visible) + (1 << Layerdefs.laser);
		cameraCullingMask = FindObjectOfType<PlayerCameraFollow>().GetComponent<Camera>().cullingMask
				+ (1 << Layerdefs.laser);

		InitialCameraSetup();
	}
	
	// Update is called once per frame
	void Update()
	{
		GetCameraInput();		
		UpdateCameraPosition();
		ChangeCamera();
		ToggleControlText();
		//UpdateSounds();
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
				camCount++;
			}
		}
		else
		{
			if (cameras[camNumber - 1].usable == true)
			{
				cameras[camNumber - 1].usable = false;
				camCount--;
			}
		}
	}

	void InitialCameraSetup()
	{
		camCount = 2;
		cameras[0].usable = true;
		cameras[1].usable = true;
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

	void ChangeCamera()
	{
		bool cameraChanged = false;
		if (Input.GetKeyDown(KeyCode.Space))
		{
			if (currentCam == camOverview) {
				currentCam = lastUsedCam;
			}
			else {
				lastUsedCam = currentCam;
				currentCam = camOverview;
			}
			cameraChanged = true;
		}
		if (Input.GetKeyDown(KeyCode.Alpha1) && cameras.Count >= 1)
		{
			if (cameras[0].usable)
			{
				currentCam = cameras[0];
				cameraChanged = true;
			}
		}
		if (Input.GetKeyDown(KeyCode.Alpha2) && cameras.Count >= 2)
		{
			if (cameras[1].usable)
			{
				currentCam = cameras[1];
				cameraChanged = true;
			}
		}
		if (Input.GetKeyDown(KeyCode.Alpha3) && cameras.Count >= 3)
		{
			if (cameras[2].usable)
			{
				currentCam = cameras[2];
				cameraChanged = true;
			}
		}
		if (Input.GetKeyDown(KeyCode.Alpha4) && cameras.Count >= 4)
		{
			if (cameras[3].usable)
			{
				currentCam = cameras[3];
				cameraChanged = true;
			}
		}
		if (Input.GetKeyDown(KeyCode.Alpha5) && cameras.Count >= 5)
		{
			if (cameras[4].usable)
			{
				currentCam = cameras[4];
				cameraChanged = true;
			}
		}
		if (Input.GetKeyDown(KeyCode.Alpha6) && cameras.Count >= 6)
		{
			if (cameras[5].usable)
			{
				currentCam = cameras[5];
				cameraChanged = true;
			}
		}
		if (Input.GetKeyDown(KeyCode.Alpha7) && cameras.Count >= 7)
		{
			if (cameras[6].usable)
			{
				currentCam = cameras[6];
				cameraChanged = true;
			}
		}
		if (Input.GetKeyDown(KeyCode.Alpha8) && cameras.Count >= 8)
		{
			if (cameras[7].usable)
			{
				currentCam = cameras[7];
				cameraChanged = true;
			}
		}
		if (Input.GetKeyDown(KeyCode.Alpha9) && cameras.Count >= 9)
		{
			if (cameras[8].usable)
			{
				currentCam = cameras[8];
				cameraChanged = true;
			}
		}
		if (Input.GetKeyDown(KeyCode.Alpha0) && cameras.Count >= 10)
		{
			if (cameras[9].usable)
			{
				currentCam = cameras[9];
				cameraChanged = true;
			}
		}
		if (!cameraChanged) return;

		if (currentCam == camOverview) {
			cam.orthographic = true;
			camOverview.camActive = true;
			cam.cullingMask = overviewCullingMask;
		}
		else {
			cam.orthographic = false;
			camOverview.camActive = false;
			cam.cullingMask = cameraCullingMask;
		}

		transform.position = currentCam.transform.position;
		transform.rotation = currentCam.transform.rotation;
		UD_rotation = currentCam.transform.eulerAngles.x;
		LR_rotation = currentCam.transform.eulerAngles.y;
		zoom = currentCam.zoom;

		cameraDesc.text = "Camera " + currentCam.cameraNumber + "\n" + currentCam.description;
	}

	void ToggleControlText()
	{
		if (Input.GetKey(KeyCode.E))
		{
			if (currentCam == camOverview)
				QUI.setControlsText("WASD: Pan\nScroll Wheel: Zoom\n1-" + camCount + ": Change Camera");
			else
				QUI.setControlsText("WASD: Rotate and Zoom\n1-" + camCount + ": Change Camera\nSPACE: Map Overview");
		}
		else
		{
			QUI.setControlsText("Hold E to View Controls");
		}
	}

	/*
	void UpdateSounds() {
		if (isPanning) {
			audioSource.enabled = true;
			if (!audioSource.isPlaying) {
				audioSource.Play ();
			}
		} else {
			audioSource.enabled = false;
		}
	}
	*/
}
