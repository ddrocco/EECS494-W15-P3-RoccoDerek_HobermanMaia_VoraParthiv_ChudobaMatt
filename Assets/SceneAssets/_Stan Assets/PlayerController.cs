using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using InControl;

public class PlayerController : MonoBehaviour
{
	public static PlayerController player;
	
	private enum State
	{
		walking, sprinting, crouching
	}
	private State _state;
	private State state
	{
		get { return _state; }
		set
		{
			PlayerStateChange(value);
			_state = value;
		}
	}

	// Object references
	private InputDevice device;
	private Camera headCamera;

	// Movement variables
	private float currentSpeed;
	private float targetSpeed;
	private Vector3 normalScale;
	private Vector3 crouchScale;
	private float crouchFactor = 0.66f;
	private float crouchLerpVal = 1f;

	// Rotation variables
	private Vector3 moveDirection = Vector3.zero;
	private float rotationY = 0f;
	private float rotationYDelta = 0f;
	private Vector3 xRotation = Vector3.zero;
	private Vector3 yRotation = Vector3.zero;

	// Public variables to be changed in the inspector
	public float walkSpeed;
	public float sprintSpeed;
	public float crouchSpeed;
	public float crouchAnimationSpeed;
	public float acceleration;
	public float rotateSpeed;
	public float minimumY = -60f;
	public float maximumY = 60f;
	public GameObject camPrefab;
	public GameObject QCamera;

	// Public variables accessible by other classes
	[HideInInspector]
	public bool canInteract = false;
	[HideInInspector]
	public bool canTag = false;
	[HideInInspector]
	public GameObject interactiveObj;
	[HideInInspector]
	public GameObject taggableObj;
	[HideInInspector]
	public static bool debugControls = false;
	
	void Awake()
	{
		player = this;
		GameObject cam = GameObject.Find("PlayerCamera");
		if (cam == null)
			cam = Instantiate(camPrefab, transform.position, Quaternion.identity) as GameObject;
		headCamera = cam.GetComponent<Camera>();

		device = InputManager.ActiveDevice;

		state = State.walking;
		currentSpeed = walkSpeed;
		targetSpeed = walkSpeed;
		normalScale = transform.localScale;
		crouchScale = normalScale;
		crouchScale.y *= crouchFactor;

		if (InputManager.Devices.Count == 0)
		{
			debugControls = true;
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
		}
	}

	void Update()
	{
		if (GameController.PlayerDead)
		{
			GetComponent<Rigidbody>().velocity = Vector3.zero;
			return;
		}
		if (debugControls)
		{
			SetMoveDirectionDebug();
			SetLookDirectionDebug();
			CheckDebugStates();
			return;
		}

		SetMoveDirection();
		SetLookDirection();

		// Change states based on controller input
		if (device.LeftStickButton.WasPressed)
		{
			if (state == State.sprinting)
				state = State.walking;
			else
				state = State.sprinting;
		}
		if (device.Action2.WasPressed)
		{
			if (state == State.crouching)
				state = State.walking;
			else
				state = State.crouching;
		}
		if (device.Action1.WasPressed)
		{
			Interact();
		}
		if (device.Action3.WasPressed)
		{
			Tag();
		}
	}

	void FixedUpdate()
	{
		if (GameController.PlayerDead)
		{
			GetComponent<Rigidbody>().velocity = Vector3.zero;
			return;
		}

		float dt = Time.fixedDeltaTime;
		Move(dt);
		Look(dt);
		Crouch(dt);
	}

	void CheckDebugStates()
	{
		if (Input.GetKeyDown(KeyCode.Mouse0)) // Interact
		{
			Interact();
		}
		if (Input.GetKeyDown(KeyCode.Mouse1)) // Crouch
		{
			if (state == State.crouching)
				state = State.walking;
			else
				state = State.crouching;
		}
		if (Input.GetKeyDown(KeyCode.RightControl) || Input.GetKeyDown(KeyCode.RightApple)) // Sprint
		{
			if (state == State.sprinting)
				state = State.walking;
			else
				state = State.sprinting;
		}
	}

	// Sets the player's movement direction based on controller left stick input
	// Called in update, so controller input is registered as quickly as possible
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
			
			AdjustSoundConstants();
		}
		else
		{
			if (state == State.sprinting)
				state = State.walking;
			Foe_Detection_Handler.audioMultiplier = 1f;
		}
	}

	void SetMoveDirectionDebug()
	{
		moveDirection.x = Input.GetAxis("Horizontal");
		moveDirection.z = Input.GetAxis("Vertical");

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
		else
		{
			if (state == State.sprinting)
				state = State.walking;
		}
	}

	// Sets the direction for the player to look in, based on the controller's right stick
	void SetLookDirection()
	{
		// Chanage x rotation based on right stick input
		xRotation.y = device.RightStickX.Value * rotateSpeed;

		// Increment y rotation by right stick input
		rotationYDelta = device.RightStickY.Value * rotateSpeed;
	}

	void SetLookDirectionDebug()
	{
		xRotation.y += Input.GetAxis("Mouse X") * rotateSpeed;

		rotationYDelta = Input.GetAxis("Mouse Y") * rotateSpeed * 2f;
	}

	// Moves the player on a fixed interval
	void Move(float dt)
	{
		currentSpeed = IncrementTowards(currentSpeed, targetSpeed, acceleration);
		GetComponent<Rigidbody>().velocity = (moveDirection * currentSpeed * dt);
	}

	// Rotates the player and camera on a fixed interval
	void Look(float dt)
	{
		// Calculate player's x rotation based on delta time
		xRotation.y *= dt;

		// Calculate player's y rotation based on delta time
		rotationY += rotationYDelta * dt;
		rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);
		yRotation.x = -rotationY;
		yRotation.y = headCamera.transform.eulerAngles.y;

		// Set player x rotation and camera y rotation
		transform.Rotate(xRotation);
		headCamera.transform.eulerAngles = yRotation;
	}

	// Called whenever the player's "state" variable gets changed
	// Handles movement speed changes and crouching
	void PlayerStateChange(State newState)
	{
		// Don't make any changes if the state was not changed
		if (state == newState) return;

		if (state == State.walking && newState == State.sprinting)
		{
			currentSpeed = walkSpeed;
			targetSpeed = sprintSpeed;
			acceleration = Mathf.Abs(acceleration);
		}
		else if (state == State.walking && newState == State.crouching)
		{
			currentSpeed = walkSpeed;
			targetSpeed = crouchSpeed;
			acceleration = Mathf.Abs(acceleration) * -1f;
		}
		else if (state == State.sprinting && newState == State.walking)
		{
			targetSpeed = walkSpeed;
			acceleration = Mathf.Abs(acceleration) * -1f;
		}
		else if (state == State.sprinting && newState == State.crouching)
		{
			currentSpeed = walkSpeed;
			targetSpeed = crouchSpeed;
			acceleration = Mathf.Abs(acceleration) * -1f;
		}
		else if (state == State.crouching && newState == State.walking)
		{
			targetSpeed = walkSpeed;
			acceleration = Mathf.Abs(acceleration);
		}
		else if (state == State.crouching && newState == State.sprinting)
		{
			currentSpeed = (walkSpeed + crouchSpeed) / 2f;
			targetSpeed = sprintSpeed;
			acceleration = Mathf.Abs(acceleration);
		}
		else
			Debug.LogError("Invalid player state change");
	}

	void Crouch(float dt)
	{
		Vector3 scaleBefore = transform.localScale;
		Vector3 scaleAfter;
		if (state == State.crouching) // lerp from standing to crouching
		{
			crouchLerpVal = Mathf.Clamp(crouchLerpVal - (dt * crouchAnimationSpeed), 0f, 1f);
			transform.localScale = Vector3.Lerp(crouchScale, normalScale, crouchLerpVal);
			scaleAfter = transform.localScale;

			if (scaleBefore != scaleAfter)
			{
				float delta = scaleBefore.y - scaleAfter.y;
				Vector3 pos = transform.position;
				pos.y -= delta;
				transform.position = pos;
			}
		}
		else // lerp from crouching to standing
		{
			crouchLerpVal = Mathf.Clamp(crouchLerpVal + (dt * crouchAnimationSpeed), 0f, 1f);
			transform.localScale = Vector3.Lerp(crouchScale, normalScale, crouchLerpVal);
			scaleAfter = transform.localScale;

			if (scaleBefore != scaleAfter)
			{
				float delta = scaleAfter.y - scaleBefore.y;
				Vector3 pos = transform.position;
				pos.y += delta;
				transform.position = pos;
			}
		}
	}

	void Interact()
	{
		if (!canInteract) return;

		if(interactiveObj.name == "InitialComputer"){
			QCamera.GetComponent<QUI>().showCamera(true);
			//QUI.setText("Hello testing 123");
			GameController.SendPlayerMessage("Great job! Try interacting with the pile of papers on the desk.", 5);
			return;
		}
		else if (interactiveObj.name.Contains("Door")) {
			DoorControl obj = interactiveObj.GetComponent<DoorControl>();
			obj.Interact();
			return;
		}
		else if (interactiveObj.name.Contains("Box")) {
			BoxControl obj = interactiveObj.GetComponentInChildren<BoxControl>();
			obj.Interact();
			return;
		}
		else if (interactiveObj.name.Contains("Paper")) {
			InformationForPlayer obj = interactiveObj.GetComponent<InformationForPlayer>();
			obj.Interact();
			return;
		}
		else if (interactiveObj.name.Contains("Filing")) {
			FileCabinetControl obj = interactiveObj.GetComponentInChildren<FileCabinetControl>();
			obj.Interact();
			return;
		}
		else
		{
			Debug.LogError("Interactive object is not a Door or Box");
			return;
		}

	}

	void Tag()
	{
		if (!canTag) return;
		if (taggableObj == null)
		{
			Debug.LogError("Null taggable object reference");
			return;
		}

		Taggable obj = taggableObj.GetComponent<Taggable>();
		if (obj == null)
		{
			Debug.LogError("Taggable object does not have script of type Taggable");
			return;
		}

		obj.TagObject();
	}

	float IncrementTowards(float current, float target, float accel)
	{
		if (current == target) return current;

		if (Mathf.Sign(accel) > 0f)
		{
			return Mathf.Clamp(current + accel, current, target);
		}
		else
		{
			return Mathf.Clamp(current + accel, target, current);
		}
	}
	
	void AdjustSoundConstants() {
		if (state == State.crouching) {
			Foe_Detection_Handler.audioMultiplier = 3f;
		} else if (state == State.walking) {
			Foe_Detection_Handler.audioMultiplier = 6f;
		} else if (state == State.sprinting) {
			Foe_Detection_Handler.audioMultiplier = 15f;
		}
	}
}