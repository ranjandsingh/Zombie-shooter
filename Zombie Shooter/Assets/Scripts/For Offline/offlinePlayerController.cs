using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(GunConroller))]
public class offlinePlayerController : LivingEntity
{
     GunConroller Mygunctrlr;
	Vector3 velocity;
	Rigidbody myRigidbody;
    public VirtualJoyStick moveJoystick;
    public float playerSpeed;
    public VirtualJoyStick rotateJoystick;
    Vector3 input02;
    public float rotationOffset;

    protected override void Start()
    {
        base.Start();
		myRigidbody = GetComponent<Rigidbody> ();
        Mygunctrlr = GetComponent<GunConroller>();
        
	}

    void Update()
    {

        float x = Input.GetAxis("Horizontal") * Time.deltaTime * playerSpeed;
        float y = Input.GetAxis("Vertical") * Time.deltaTime * playerSpeed;
        if (moveJoystick.InputDirection != Vector3.zero)
        {
            x = moveJoystick.InputDirection.x * Time.deltaTime * playerSpeed;
            y = moveJoystick.InputDirection.y * Time.deltaTime * playerSpeed;
        }
        velocity = new Vector3(x, 0, y);
        if (Input.GetKey(KeyCode.Space))
			Mygunctrlr.OnTriggerHold ();

        

    }

	public void FixedUpdate() {
		myRigidbody.MovePosition (myRigidbody.position + velocity );

        if (rotateJoystick.InputDirection != Vector3.zero)
        {
            input02 = new Vector3(rotateJoystick.InputDirection.x, rotateJoystick.InputDirection.y, 0);           
        }
        Vector3 diffrence = input02 - Vector3.zero;
        diffrence.Normalize();
        float rotZ = Mathf.Atan2(diffrence.y, diffrence.x) * Mathf.Rad2Deg;       
        transform.rotation = Quaternion.Euler(0f, -(rotZ-rotationOffset), 0f);
		if (transform.position.y < -10)
			Die ();

	}
}


