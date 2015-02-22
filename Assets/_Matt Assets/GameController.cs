using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using InControl;

public class GameController : MonoBehaviour
{
	private InputDevice device;
	private string levelName = "_Final_Prototype_Map";
	private Text playerGameOverText;
	private Text playerGameOverMessageText;

	private static bool _dead;
	public static bool PlayerDead
	{
		get { return _dead; }
		set
		{
			_dead = value;
			if (value == true)
				GameOver();
		}
	}
	public static string GameOverMessage;

	void Awake()
	{
		device = InputManager.ActiveDevice;

		playerGameOverText = GameObject.Find("GameOverText").GetComponent<Text>();
		playerGameOverMessageText = GameObject.Find("GameOverMessageText").GetComponent<Text>();

		playerGameOverText.enabled = false;
		playerGameOverMessageText.enabled = false;
	}

	void Update()
	{
		if (!PlayerDead) return;

		EnableText();

		if (Input.GetKeyDown(KeyCode.Escape) || device.Action1.WasPressed)
		{
			PlayerDead = false;
			Application.LoadLevel(levelName);
		}
	}

	void EnableText()
	{
		playerGameOverText.enabled = true;
		playerGameOverMessageText.enabled = true;
		playerGameOverText.text = "Game Over";
		playerGameOverMessageText.text = GameOverMessage;
	}

	private static void GameOver()
	{
		// Process any one-time game over data here
		return;
	}
}
