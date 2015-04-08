using UnityEngine;
using System.Collections;

public class QCameraOverview : QCameraLocation
{
	public float panSpeed;
	public float zoomSpeed;
	public float minX, maxX;
	public float minY, maxY;
	public float minZoom, maxZoom;
	[HideInInspector]
	public bool camActive;

	private Camera cam;
	private Vector3 dir;

	void Awake()
	{
		cam = GameObject.Find("QCamera").GetComponent<Camera>();
		cam.orthographicSize = zoom;		
	}
	
	void Start() {
		foreach (Transform obj in FindObjectOfType<Transform>()) {
			if (obj.position.x < minX) {
				minX = obj.position.x;
			}
			if (obj.position.x > maxX) {
				maxX = obj.position.x;
			}
			if (obj.position.y < minY) {
				minY = obj.position.y;
			}
			if (obj.position.y > maxY) {
				maxY = obj.position.y;
			}
		}
	}
	
	void Update()
	{
		if (!camActive || PauseScript.GamePaused) return;

		GetCameraInput();
		ZoomCamera();
	}

	void FixedUpdate()
	{
		if (!camActive || PauseScript.GamePaused) return;

		MoveCamera();
	}

	void GetCameraInput()
	{
		float horizontal = Input.GetAxis("HorizontalQOverview");
		float vertical = Input.GetAxis("VerticalQOverview");
		dir = new Vector3(horizontal, 0f, vertical);
	}

	void MoveCamera()
	{
		Vector3 pos = transform.position;
		pos.x += (dir.x * panSpeed * Time.fixedDeltaTime);
		pos.z += (dir.z * panSpeed * Time.fixedDeltaTime);

		pos.x = Mathf.Clamp(pos.x, minX, maxX);
		pos.z = Mathf.Clamp(pos.z, minY, maxY);

		transform.position = pos;
		cam.transform.position = transform.position;
		cam.transform.rotation = transform.rotation;
	}

	void ZoomCamera()
	{
		// Cannot zoom when Stan has mouse controls
		if (PlayerController.mouseMovement) return;

		if (Input.GetAxis("Mouse ScrollWheel") < 0)
		{
			zoom += zoomSpeed;
		}
		if (Input.GetAxis("Mouse ScrollWheel") > 0)
		{
			zoom -= zoomSpeed;
		}

		zoom = Mathf.Clamp(zoom, minZoom, maxZoom);
		cam.orthographicSize = zoom;
	}
}