﻿using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour {

	public GameObject mainMenuHolder;
	public GameObject optionsMenuHolder;

	public Slider[] volumeSliders;


	void Start() {

		volumeSliders [0].value = AudioManager.instance.masterVolumePercent;
		volumeSliders [1].value = AudioManager.instance.musicVolumePercent;
		volumeSliders [2].value = AudioManager.instance.sfxVolumePercent;


	}


	public void Play() {
		SceneManager.LoadScene ("offline");
	}

	public void Quit() {
		Application.Quit ();
	}

	public void OptionsMenu() {
		mainMenuHolder.SetActive (false);
		optionsMenuHolder.SetActive (true);
	}

	public void MainMenu() {
		mainMenuHolder.SetActive (true);
		optionsMenuHolder.SetActive (false);
	}


	public void SetMasterVolume(float value) {
		AudioManager.instance.SetVolume (value, AudioManager.AudioChannel.Master);
	}

	public void SetMusicVolume(float value) {
		AudioManager.instance.SetVolume (value, AudioManager.AudioChannel.music);
	}

	public void SetSfxVolume(float value) {
		AudioManager.instance.SetVolume (value, AudioManager.AudioChannel.sfx);
	}

}
