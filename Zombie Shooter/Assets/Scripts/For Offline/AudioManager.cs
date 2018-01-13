﻿using UnityEngine;
using System.Collections;
using System;

public class AudioManager : MonoBehaviour {
	public enum AudioChannel {Master,sfx,music};
	public float masterVolumePercent = .2f;
	public float sfxVolumePercent = 1;
	public float musicVolumePercent = 1f;

	AudioSource sfx2DSource;
	AudioSource[] musicSources;
	int activeMusicSourceIndex;

	public static AudioManager instance;

	Transform audioListener;
	public Transform playerT;
	SoundLibrary library;

	void Awake() {
		if(FindObjectOfType<offlinePlayerController> () != null)
			playerT = FindObjectOfType<offlinePlayerController> ().transform;
		library = GetComponent <SoundLibrary> ();
		if (instance != null)
			Destroy (gameObject);
		else {
			instance = this;
			DontDestroyOnLoad (gameObject);

			musicSources = new AudioSource[2];
			for (int i = 0; i < 2; i++) {
				GameObject newMusicSource = new GameObject ("Music source " + (i + 1));
				musicSources [i] = newMusicSource.AddComponent<AudioSource> ();
				newMusicSource.transform.parent = transform;
			}
			GameObject newSfx2Dsource = new GameObject ("2D sfx source");
			sfx2DSource = newSfx2Dsource.AddComponent<AudioSource> ();
			newSfx2Dsource.transform.parent = transform;

			audioListener = FindObjectOfType<AudioListener> ().transform;

			masterVolumePercent = PlayerPrefs.GetFloat ("master vol",masterVolumePercent);
			musicVolumePercent = PlayerPrefs.GetFloat ("music vol",musicVolumePercent);
			sfxVolumePercent = PlayerPrefs.GetFloat ("sfx vol",sfxVolumePercent);
		}
	}

	void Update() {
		if (playerT != null) {
			audioListener.position = playerT.position;
		}
	}

	public void SetVolume(float volumePercent,AudioChannel channel){
		switch(channel)
		{
		case AudioChannel.Master:
			masterVolumePercent = volumePercent;
			break;
		case AudioChannel.music:
			musicVolumePercent = volumePercent;
			break;
		case AudioChannel.sfx:
			sfxVolumePercent = volumePercent;
			break;
		}
		musicSources [0].volume = musicVolumePercent * masterVolumePercent;
		musicSources [1].volume = musicVolumePercent * masterVolumePercent;

		PlayerPrefs.SetFloat ("master vol",masterVolumePercent);
		PlayerPrefs.SetFloat ("music vol",musicVolumePercent);
		PlayerPrefs.SetFloat ("sfx vol",sfxVolumePercent);
	}
	public void PlayMusic(AudioClip clip, float fadeDuration = 1) {
		activeMusicSourceIndex = 1 - activeMusicSourceIndex;
		musicSources [activeMusicSourceIndex].clip = clip;
		musicSources [activeMusicSourceIndex].Play ();

		StartCoroutine(AnimateMusicCrossfade(fadeDuration));
	}

	public void PlaySound(AudioClip clip, Vector3 pos) {
		if (clip != null) {
			AudioSource.PlayClipAtPoint (clip, pos, sfxVolumePercent * masterVolumePercent);
		}
	}
	public void PlaySound(string soundName, Vector3 pos) {
		PlaySound (library.GetClipFromName (soundName),pos);
	}
	public void PlaySound2D(string soundName) {
		sfx2DSource.PlayOneShot (library.GetClipFromName (soundName), sfxVolumePercent * masterVolumePercent);
	}

	IEnumerator AnimateMusicCrossfade(float duration) {
		float percent = 0;

		while (percent < 1) {
			percent += Time.deltaTime * 1 / duration;
			musicSources [activeMusicSourceIndex].volume = Mathf.Lerp (0, musicVolumePercent * masterVolumePercent, percent);
			musicSources [1-activeMusicSourceIndex].volume = Mathf.Lerp (musicVolumePercent * masterVolumePercent, 0, percent);
			yield return null;
		}
	}
}
