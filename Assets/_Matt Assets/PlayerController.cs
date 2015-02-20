using UnityEngine;
using System.Collections;
using InControl;

public class PlayerController : MonoBehaviour
{
	private InputDevice device;
	private CharacterController controller;
	private Camera camera;

	public float speed;

	void Awake()
	{
		device = InputManager.ActiveDevice;
		controller = GetComponent<CharacterController>();
		camera = GetComponentInChildren<Camera>();
	}

	void Update()
	{
		Debug.Log("Left stick X: " + device.LeftStickX.Value);
		Debug.Log("Left stick Y: " + device.LeftStickY.Value);
		Debug.Log("Right stick X: " + device.RightStickX.Value);
		Debug.Log("Right stick Y: " + device.RightStickY.Value);

		if (Input.GetKey(KeyCode.W))
			controller.SimpleMove(new Vector3(speed, 0f, 0f));
	}
}