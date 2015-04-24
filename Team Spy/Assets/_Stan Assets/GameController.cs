using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using InControl;

public class GameController : MonoBehaviour
{
	public static bool LookedAway = false;

	private static bool countdown = false;
	private InputDevice device;
	private GameObject GameOverBG;
	private Text playerGameOverText;
	private Text playerGameOverMessageText;
	private Text playerMessageText;
	private Image playerMessagePanel;
	private RectTransform messageTextRect;
	private RectTransform messagePanelRect;
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
		playerMessagePanel = GameObject.Find("MessagePanel").GetComponent<Image>();
		messageTextRect = playerMessageText.gameObject.GetComponent<RectTransform>();
		messagePanelRect = playerMessagePanel.gameObject.GetComponent<RectTransform>();

		playerGameOverText.enabled = false;
		playerGameOverMessageText.enabled = false;
		playerMessageText.enabled = false;
		playerMessagePanel.enabled = false;
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
			} else if (PlayerWon){
				PlayerWon = false;
				if (Application.levelCount > Application.loadedLevel + 1) {
					Application.LoadLevel(Application.loadedLevel + 1);
				} else {
					Application.LoadLevel(0);
				}
			}
		}
	}

	void DisplayPlayerMessage()
	{
		if (messageText == "") return;

		if (LookedAway)
		{
			LookedAway = false;
			countdown = true;
		}

		if (messageTime > 0)
		{
			if (countdown)
				messageTime -= Time.deltaTime;

			Vector2 temp = messageTextRect.offsetMin;
			temp.x -= 35f;
			temp.y -= 35f;
			messagePanelRect.offsetMin = temp;

			playerMessageText.enabled = true;
			playerMessageText.text = messageText;
			playerMessagePanel.enabled = true;
		} else
		{
			playerMessageText.text = "";
			playerMessageText.enabled = false;
			playerMessagePanel.enabled = false;
			countdown = false;
		}
	}

	void EnableText()
	{
		playerGameOverText.enabled = true;
		playerGameOverMessageText.enabled = true;
		playerMessageText.enabled = false;
		playerMessagePanel.enabled = false;
		if (_won) {
			playerGameOverText.text = "Mission Complete";
			playerGameOverMessageText.text = "Press A to continue";
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
		countdown = false;
	}

	private static void GameOver()
	{
		// Process any one-time game over data here
		return;
	}
}
