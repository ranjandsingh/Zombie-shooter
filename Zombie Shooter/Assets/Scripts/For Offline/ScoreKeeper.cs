using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreKeeper : MonoBehaviour {

	public static int score { get; private set; }
	float lastEnemyKillTime;
	int streakCount;
	float streakExpiryTime = 3;
	int HighScore = 0;
	public TextMeshProUGUI CoinText;
	public TextMeshProUGUI CurrentScore;
	public TextMeshProUGUI HighScoreText;
	public TextMeshProUGUI tmptext;
	public TextMeshProUGUI StreakCountText;

	void Start() {
		Enemy.OnDeathStatic += OnEnemyKilled;
		FindObjectOfType<offlinePlayerController> ().OnDeath += OnPlayerDeath;
		HighScore = PlayerPrefs.GetInt ("HighScore");
	}

	void Update(){
		StreakCountText.text = "Killing Streak :- " + streakCount;

	}

	void OnEnemyKilled() {

		if (Time.time < lastEnemyKillTime + streakExpiryTime) {
			streakCount++;

		} else {
			streakCount = 0;
		}

		lastEnemyKillTime = Time.time;

		score += 5 + (int)Mathf.Pow(2,streakCount);
		CoinText.text = "" + score;
	}

	void OnPlayerDeath() {
		Enemy.OnDeathStatic -= OnEnemyKilled;
		CurrentScore.text = "Score:- " + score;
		if (score > HighScore) {
			PlayerPrefs.SetInt ("HighScore", score);
			HighScoreText.text = "WOW NEW HIGH SCORE";
			}
		else
			HighScoreText.text = "HighScore:- " + HighScore;
		score = 0;

	}
	
}
