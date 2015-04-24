using InControl;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CreditScroll : MonoBehaviour
{
	public float speed = 1.0f;
	public float startPos;
	public float endPos;
	public float loadingPos;

	private RectTransform pos;
	private InputDevice device;

	void Start()
	{
		pos = GetComponent<RectTransform>();

		Vector2 temp = pos.anchoredPosition;
		temp.y = startPos;
		pos.anchoredPosition = temp;

		device = InputManager.ActiveDevice;
	}

	void Update()
	{
		if (Input.anyKeyDown)
			CreditsDone();
		if (device.AnyButton.WasPressed)
			CreditsDone();
	}

	void FixedUpdate()
	{
		Vector2 temp = pos.anchoredPosition;
		temp.y += speed * Time.fixedDeltaTime;
		pos.anchoredPosition = temp;

		if (temp.y >= endPos)
			CreditsDone();
	}

	void CreditsDone()
	{
		GetComponent<Text>().text = "Loading Main Menu...";
		Vector2 temp = pos.anchoredPosition;
		temp.y = loadingPos;
		pos.anchoredPosition = temp;

		Application.LoadLevel("MainMenu");
	}
}
