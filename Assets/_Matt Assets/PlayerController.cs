using UnityEngine;
using System.Collections;
using InControl;

public class PlayerController : MonoBehaviour
{
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
	private CharacterController controller;
	private Camera headCamera;
	private Animator anim;

	// Movement variables
	private float currentSpeed;
	private float targetSpeed;
	private Vector3 normalScale;
	private Vector3 crouchScale;
	private float crouchFactor = 0.66f;

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
	public float acceleration;
	public float rotateSpeed;
	public float minimumY = -60f;
	public float maximumY = 60f;
	[HideInInspector]
	public bool canInteract = false;
	[HideInInspector]
	public GameObject interactiveObj;
	
	void Awake()
	{
		device = InputManager.ActiveDevice;
		controller = GetComponent<CharacterController>();
		headCamera = GetComponentInChildren<Camera>();
		anim = GetComponentInChildren<Animator>();

		state = State.walking;
		currentSpeed = walkSpeed;
		targetSpeed = walkSpeed;
		normalScale = transform.localScale;
		crouchScale = normalScale;
		crouchScale.y *= crouchFactor;
	}

	void Update()
	{
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
	}

	void FixedUpdate()
	{
		float dt = Time.fixedDeltaTime;
		Move(dt);
		Look(dt);
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

	// Moves the player on a fixed interval
	void Move(float dt)
	{
		currentSpeed = IncrementTowards(currentSpeed, targetSpeed, acceleration);
		controller.SimpleMove(moveDirection * currentSpeed * dt);
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
		yRotation.y = headCamera.transform.localEulerAngles.y;

		// Set player x rotation and camera y rotation
		transform.Rotate(xRotation);
		headCamera.transform.localEulerAngles = yRotation;
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
			Crouch();
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
			Crouch();
		}
		else if (state == State.crouching && newState == State.walking)
		{
			targetSpeed = walkSpeed;
			acceleration = Mathf.Abs(acceleration);
			Crouch();
		}
		else if (state == State.crouching && newState == State.sprinting)
		{
			currentSpeed = (walkSpeed + crouchSpeed) / 2f;
			targetSpeed = sprintSpeed;
			acceleration = Mathf.Abs(acceleration);
			Crouch();
		}
		else
			Debug.LogError("Invalid player state change");
	}

	void Crouch()
	{
		if (anim.GetBool("Crouching"))
		{
			anim.CrossFade("PlayerStand", 0.25f);
			anim.SetBool("Crouching", false);
		}
		else
		{
			anim.CrossFade("PlayerCrouch", 0.25f);
			anim.SetBool("Crouching", true);
		}
	}

	void Interact()
	{
		if (!canInteract) return;

		Renderer rend = interactiveObj.GetComponent<Renderer>();
		rend.material.color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
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
}