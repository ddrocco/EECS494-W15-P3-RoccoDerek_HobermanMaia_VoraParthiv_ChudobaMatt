using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class QCameraControl : MonoBehaviour {
	//Sound things:
	/*
	public List<AudioClip> steps;
	public AudioSource source;
	public int currentStep = 0;
	public bool isPanning = false;
	*/
	
	public GameObject player;
	public Vector3 pivotPoint;
	
	public float UDrotation, LRrotation, distance;
	
	public float rotationSpeed = 200f;
	public float panSpeed = 50f;
	public float distanceMin = 10f;
	public float distanceMax = 50f;

	float radconv = Mathf.PI / 180f;
	
	// Use this for initialization
	void Start () {
		//Sound stuff:
		/*
		source = GetComponent<AudioSource>();
		currentStep = 0;
		isPanning = false;
		audioSource = GetComponent<AudioSource>();
		audioSource.enabled = false;
		*/
		pivotPoint = player.transform.position;

	}
	
	// Update is called once per frame
	void Update () {
		GetCameraInput();		
		UpdateCameraPosition();
		//UpdateSounds();
	}
	
	void GetCameraInput() {

		//rotate
		//isPanning = false;
		if (Input.GetKey(KeyCode.D)) {
			LRrotation -= rotationSpeed * Time.deltaTime;
			//isPanning = true;
		}
		if (Input.GetKey(KeyCode.A)) {
			LRrotation += rotationSpeed * Time.deltaTime;
			//isPanning = true;
		}
		if (Input.GetKey(KeyCode.W)) {
			UDrotation += rotationSpeed * Time.deltaTime;
			//isPanning = true;
		}
		if (Input.GetKey(KeyCode.S)) {
			UDrotation -= rotationSpeed * Time.deltaTime;
			//isPanning = true;
		}

		//zoom
		if (Input.GetKey(KeyCode.E)) {
			distance -= rotationSpeed * Time.deltaTime;
			//isPanning = true;
		}
		if (Input.GetKey(KeyCode.Q)) {
			distance += rotationSpeed * Time.deltaTime;
			//isPanning = true;
		}

		//pan
		if (Input.GetKey(KeyCode.L)) {
			pivotPoint.z -= panSpeed * Time.deltaTime * Mathf.Sin(LRrotation * radconv);
			pivotPoint.x += panSpeed * Time.deltaTime * Mathf.Cos(LRrotation * radconv);
		}
		if (Input.GetKey(KeyCode.J)) {
			pivotPoint.z += panSpeed * Time.deltaTime * Mathf.Sin(LRrotation * radconv);
			pivotPoint.x -= panSpeed * Time.deltaTime * Mathf.Cos(LRrotation * radconv);
		}
		if (Input.GetKey(KeyCode.I)) {
			pivotPoint.z += panSpeed * Time.deltaTime * Mathf.Cos(LRrotation * radconv);
			pivotPoint.x += panSpeed * Time.deltaTime * Mathf.Sin(LRrotation * radconv);
		}
		if (Input.GetKey(KeyCode.K)) {
			pivotPoint.z -= panSpeed * Time.deltaTime * Mathf.Cos(LRrotation * radconv);
			pivotPoint.x -= panSpeed * Time.deltaTime * Mathf.Sin(LRrotation * radconv);
		}
		if (Input.GetKey(KeyCode.O)) {
			pivotPoint.y += panSpeed * Time.deltaTime;
		}
		if (Input.GetKey(KeyCode.U)) {
			pivotPoint.y -= panSpeed * Time.deltaTime;
		}

		//snap
		if (Input.GetKey(KeyCode.Space)) {
			Vector3 displacement = player.transform.position - pivotPoint;
			if(displacement.magnitude < rotationSpeed * Time.deltaTime){
				pivotPoint = player.transform.position;
			} else {
				pivotPoint += displacement.normalized * rotationSpeed * Time.deltaTime;
			}
		}
	}
	
	void UpdateCameraPosition() {
		if (UDrotation > 90f) {
			UDrotation = 90f;
		}
		if (UDrotation < -90f) {
			UDrotation = -90f;
		}
		if (LRrotation > 360f) {
			LRrotation -= 360f;
		}
		if (LRrotation < 0f) {
			LRrotation += 360f;
		}
		
		if (distance < distanceMin) {
			distance = distanceMin;
		}
		if (distance > distanceMax) {
			distance = distanceMax;
		}
		
		transform.rotation = Quaternion.Euler (new Vector3(UDrotation, LRrotation, 0));
		transform.position = pivotPoint + transform.rotation * Vector3.back * distance;
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
