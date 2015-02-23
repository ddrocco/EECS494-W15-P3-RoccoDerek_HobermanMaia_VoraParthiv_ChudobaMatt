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
	private Text playerMessageText;
	private static string messageText;
	private static float messageTime;

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
		playerMessageText = GameObject.Find("PlayerMessageText").GetComponent<Text>();

		playerGameOverText.enabled = false;
		playerGameOverMessageText.enabled = false;
		playerMessageText.enabled = false;
	}

	void Update()
	{
		DisplayPlayerMessage();
		if (!PlayerDead) return;

		EnableText();

		if (Input.GetKeyDown(KeyCode.Escape) || device.Action1.WasPressed)
		{
			PlayerDead = false;
			Application.LoadLevel(levelName);
		}
	}

	void DisplayPlayerMessage()
	{
		if (messageTime > 0)
		{
			messageTime -= Time.deltaTime;
			playerMessageText.enabled = true;
			playerMessageText.text = messageText;
		}
		else
		{
			playerMessageText.text = "";
			playerMessageText.enabled = false;
		}
	}

	void EnableText()
	{
		playerGameOverText.enabled = true;
		playerGameOverMessageText.enabled = true;
		playerMessageText.enabled = false;
		playerGameOverText.text = "Game Over";
		playerGameOverMessageText.text = GameOverMessage;
	}

	public static void SendPlayerMessage(string message, float time)
	{
		messageText = message;
		messageTime = time;
	}

	private static void GameOver()
	{
		// Process any one-time game over data here
		return;
	}
}
