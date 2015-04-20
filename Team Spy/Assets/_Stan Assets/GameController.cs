using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using InControl;
using UnityEditor.Events;

public class GameController : MonoBehaviour
{
	private InputDevice device;
//	private string levelName = "_Final_Prototype_Map";
	private GameObject GameOverBG;
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

	private static bool _won;
	public static bool PlayerWon
	{
		get { return _won; }
		set
		{
			_won = value;
		}
	}

	public static string GameOverMessage;

	void Awake()
	{
		device = InputManager.ActiveDevice;

		GameOverBG = GameObject.Find ("GameOverBG");
		playerGameOverText = GameObject.Find("GameOverText").GetComponent<Text>();
		playerGameOverMessageText = GameObject.Find("GameOverMessageText").GetComponent<Text>();
		playerMessageText = GameObject.Find("PlayerMessageText").GetComponent<Text>();

		playerGameOverText.enabled = false;
		playerGameOverMessageText.enabled = false;
		playerMessageText.enabled = false;
		GameOverBG.SetActive (false);
	}

	void Update()
	{
		DisplayPlayerMessage();
		if (!PlayerDead && !PlayerWon) return;

		EnableText();

		if (device.Action1.WasPressed || Input.GetKeyDown(KeyCode.Space))
		{
			if(PlayerDead){
				PlayerDead = false;
				Application.LoadLevel(Application.loadedLevel);
				//ThirdWindow 
				//.NewLevelLoaded();
			} else if (PlayerWon){
				PlayerWon = false;
				if (Application.levelCount > Application.loadedLevel + 1) {
					Application.LoadLevel(Application.loadedLevel + 1);
					//ThirdWindow.NewLevelLoaded();
				} else {
					Application.LoadLevel(0);
				}
			}
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
		if (_won) {
			playerGameOverText.text = "SUCCESS";
			playerGameOverMessageText.text = "Mission Complete! Press A to continue";
		} else {
			playerGameOverText.text = "Game Over";
			playerGameOverMessageText.text = GameOverMessage;
		}

		GameOverBG.SetActive (true);
		Time.timeScale = 0;
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
