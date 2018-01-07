using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameUI : MonoBehaviour
{

    public Image fadePlane;
    public GameObject fadeimage;
    public GameObject gameOverUI;
	public GameObject gameUI;

    void Start()
    {
        FindObjectOfType<offlinePlayerController>().OnDeath += OnGameOver;
    }

    void OnGameOver()
    {
		gameUI.SetActive (false);
        fadeimage.SetActive(true);
        StartCoroutine(Fade(Color.clear, Color.black, 1));
        gameOverUI.SetActive(true);
    }

    IEnumerator Fade(Color from, Color to, float time)
    {
        float speed = 1 / time;
        float percent = 0;

        while (percent < 1)
        {
            percent += Time.deltaTime * speed;
            fadePlane.color = Color.Lerp(from, to, percent);
            yield return null;
        }
    }

    // UI Input
    public void StartNewGame()
    {
        SceneManager.LoadScene("offline");
    }

	public void backToMenu(){
		SceneManager.LoadScene("Main Menu");

	}
}