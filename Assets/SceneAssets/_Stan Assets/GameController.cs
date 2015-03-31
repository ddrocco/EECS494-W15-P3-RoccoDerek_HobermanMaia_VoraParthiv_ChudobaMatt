﻿using UnityEngine;
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
		if (!PlayerDead && !PlayerWon) return;

		EnableText();

		if (Input.GetKeyDown(KeyCode.Escape) || device.Action1.WasPressed || Input.GetKeyDown(KeyCode.Mouse0))
		{
			if(PlayerDead){
				PlayerDead = false;
				Application.LoadLevel(Application.loadedLevel);
			} else if (PlayerWon){
				PlayerWon = false;
				if(Application.levelCount > Application.loadedLevel + 1) Application.LoadLevel(Application.loadedLevel + 1);
				else Application.LoadLevel(0);
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
		playerGameOverText.text = "Game Over";
		playerGameOverMessageText.text = GameOverMessage;
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
