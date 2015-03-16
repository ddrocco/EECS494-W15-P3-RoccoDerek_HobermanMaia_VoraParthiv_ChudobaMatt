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
			cameras.Add(obj.GetComponent<QCameraLocation>());
		}
		CameraComp comp = new CameraComp();
		cameras.Sort(comp);
		camCount = cameras.Count;

		// Internal numbering of cameras
		for (int i = 0; i < camCount; i++)
			cameras[i].cameraNumber = i + 1;

		currentCam = cameras[0];
		transform.position = currentCam.transform.position;
		transform.rotation = currentCam.transform.rotation;
		UD_rotation = currentCam.transform.eulerAngles.x;
		LR_rotation = currentCam.transform.eulerAngles.y;
		currentCam.zoom = zoom;

		cameraDesc.text = "Camera 1\n" + currentCam.description;
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
		if (Input.GetKeyDown(KeyCode.Alpha1) && camCount >= 1)
		{
			currentCam = cameras[0];
			cameraChanged = true;
		}
		if (Input.GetKeyDown(KeyCode.Alpha2) && camCount >= 2)
		{
			currentCam = cameras[1];
			cameraChanged = true;
		}
		if (Input.GetKeyDown(KeyCode.Alpha3) && camCount >= 3)
		{
			currentCam = cameras[2];
			cameraChanged = true;
		}
		if (Input.GetKeyDown(KeyCode.Alpha4) && camCount >= 4)
		{
			currentCam = cameras[3];
			cameraChanged = true;
		}
		if (Input.GetKeyDown(KeyCode.Alpha5) && camCount >= 5)
		{
			currentCam = cameras[4];
			cameraChanged = true;
		}
		if (Input.GetKeyDown(KeyCode.Alpha6) && camCount >= 6)
		{
			currentCam = cameras[5];
			cameraChanged = true;
		}
		if (Input.GetKeyDown(KeyCode.Alpha7) && camCount >= 7)
		{
			currentCam = cameras[6];
			cameraChanged = true;
		}
		if (Input.GetKeyDown(KeyCode.Alpha8) && camCount >= 8)
		{
			currentCam = cameras[7];
			cameraChanged = true;
		}
		if (Input.GetKeyDown(KeyCode.Alpha9) && camCount >= 9)
		{
			currentCam = cameras[8];
			cameraChanged = true;
		}
		if (Input.GetKeyDown(KeyCode.Alpha0) && camCount >= 10)
		{
			currentCam = cameras[9];
			cameraChanged = true;
		}
		if (!cameraChanged) return;

		if (currentCam == camOverview)
		{
			cam.orthographic = true;
			camOverview.camActive = true;
		}
		else
		{
			cam.orthographic = false;
			camOverview.camActive = false;
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
				QUI.setText("WASD: Pan\nScroll Wheel: Zoom\n1-" + camCount + ": Change Camera");
			else
				QUI.setText("WASD: Rotate and Zoom\n1-" + camCount + ": Change Camera\nSPACE: Map Overview");
		}
		else
		{
			QUI.setText("Hold E to View Controls");
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
