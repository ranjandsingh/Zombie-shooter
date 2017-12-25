using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreManager : MonoBehaviour {
	public int coins = 0;
	public int totalCoins;
	public Text CoinText;
	public TextMeshPro tmptext;


	// Use this for initialization
	void Start () {
		
		
	}
	public void CoinCollected(int value){
		coins += value;
		CoinText.text = ""+ coins;
		tmptext.text = "" + coins;
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
