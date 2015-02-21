using UnityEngine;
using System.Collections;
using InControl;

public class PlayerController : MonoBehaviour
{
	private InputDevice device;
	private CharacterController controller;
	private Camera headCamera;
	private Vector3 moveDirection = Vector3.zero;
	private float rotationY = 0f;
	private float rotationYDelta = 0f;
	private Vector3 xRotation = Vector3.zero;
	private Vector3 yRotation = Vector3.zero;

	public float moveSpeed;
	public float rotateSpeed;
	public float minimumY = -60f;
	public float maximumY = 60f;
	
	void Awake()
	{
		device = InputManager.ActiveDevice;
		controller = GetComponent<CharacterController>();
		headCamera = GetComponentInChildren<Camera>();
	}

	void Update()
	{
		SetMoveDirection();
		SetLookDirection();
	}

	void FixedUpdate()
	{
		float dt = Time.fixedDeltaTime;
		Move(dt);
		Look(dt);
	}

	void SetMoveDirection()
	{
		// Get the input vector from left analog stick
		moveDirection.x = device.LeftStickX.Value;
		moveDirection.z = device.LeftStickY.Value;

		moveDirection = transform.TransformDirection(moveDirection);

		if (moveDirection != Vector3.zero)
		{
			// Get the length of the direction vector and then normalize it
			// Dividing by the length is cheaper than normalizing
			float directionLength = moveDirection.magnitude;
			moveDirection = moveDirection / directionLength;
			
			// Make sure the length is no bigger than 1
			directionLength = Mathf.Min(1, directionLength);
			
			// Make the input vector more sensitive towards the extremes and less sensitive in the middle
			// This makes it easier to control slow speeds when using analog sticks
			directionLength = directionLength * directionLength;
			
			// Multiply the normalized direction vector by the modified length
			moveDirection = moveDirection * directionLength;
		}
	}

	void SetLookDirection()
	{
		// Chanage x rotation based on right stick input
		xRotation.y = device.RightStickX.Value * rotateSpeed;

		// Increment y rotation by right stick input
		rotationYDelta = device.RightStickY.Value * rotateSpeed;
	}

	void Move(float dt)
	{
		controller.SimpleMove(moveDirection * moveSpeed * dt);
	}

	void Look(float dt)
	{
		// Calculate player's x rotation based on delta time
		xRotation.y *= dt;

		// Calculate player's y rotation based on delta time
		rotationY += rotationYDelta * dt;
		rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);
		yRotation.x = -rotationY;
		yRotation.y = headCamera.transform.localEulerAngles.y;

		// Set player x rotation and camera y rotation
		transform.Rotate(xRotation);
		headCamera.transform.localEulerAngles = yRotation;
	}
}