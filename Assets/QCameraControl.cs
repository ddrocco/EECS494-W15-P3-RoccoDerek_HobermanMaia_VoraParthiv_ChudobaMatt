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
	
	public float rotationSpeed = 2f;
	public float distanceMin = 10f;
	public float distanceMax = 50f;
	
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
	}
	
	// Update is called once per frame
	void Update () {
		GetCameraInput();		
		UpdateCameraPosition();
		UpdateSounds();
	}
	
	void GetCameraInput() {
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
		
		if (Input.GetKey(KeyCode.Q)) {
			distance -= rotationSpeed * Time.deltaTime;
			//isPanning = true;
		}
		if (Input.GetKey(KeyCode.E)) {
			distance += rotationSpeed * Time.deltaTime;
			//isPanning = true;
		}
		
		if (Input.GetKey(KeyCode.Space)) {
			pivotPoint = player.transform.position;
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
