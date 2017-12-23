using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class mainmenu : MonoBehaviour {

	public void Play_now (){
		SceneManager.LoadScene ("offline");
		
	}
	public void MuiltiPlayer (){
		SceneManager.LoadScene ("level01");

	}
}
