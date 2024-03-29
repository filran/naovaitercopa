﻿using UnityEngine;

public class GUIManager2 : MonoBehaviour {
	
	private static GUIManager2 instance;
	
	public GUIText boostsText, distanceText, gameOverText, instructionsText, runnerText;
	
	void Start () {
		instance = this;
		GameEventManager.GameStart += GameStart;
		GameEventManager.GameOver += GameOver;
		gameOverText.enabled = false;
	}

	void Update () {
		if(Input.GetButtonDown("Jump")){
			GameEventManager.TriggerGameStart();
		}
	}
	
	private void GameStart () {
		gameOverText.enabled = false;
		instructionsText.enabled = false;
		runnerText.enabled = false;
		enabled = false;
	}
	
	private void GameOver () {
		gameOverText.enabled = true;
		instructionsText.enabled = true;
		enabled = true;
	}
	
	public static void SetBoosts(int boosts){
		instance.boostsText.text = boosts.ToString();
	}

	public static void SetDistance(float distance){
		instance.distanceText.text = distance.ToString("f0");
	}
}