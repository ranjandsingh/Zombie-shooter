using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class controler : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		float x = Input.GetAxis("Horizontal") * Time.deltaTime * 50.0f;
		float y = Input.GetAxis("Vertical") * Time.deltaTime * 3.0f;
		transform.Rotate(0, x, 0);
		transform.Translate(0, 0, y);
		
	}
}
