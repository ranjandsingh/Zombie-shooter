using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(Rigidbody))]
public class PlayerControler : NetworkBehaviour
{


    public GameObject bullatPrefab;
    public Transform bulletSpawn;
    public VirtualJoyStick moveJoystick;
    public GameObject BulletFire;
    
   

    // Use this for initialization
    void Start()
    {
        moveJoystick = FindObjectOfType<VirtualJoyStick>();
        

        BulletFire.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        if (!isLocalPlayer)
        {
            return;
            
        }
        float x = Input.GetAxis("Horizontal") * Time.deltaTime * 150.0f;
        float y = Input.GetAxis("Vertical") * Time.deltaTime * 3.0f;
        if (moveJoystick.InputDirection != Vector3.zero)
        {
            x = moveJoystick.InputDirection.x * Time.deltaTime * 150.0f;
            y = moveJoystick.InputDirection.y * Time.deltaTime * 3.0f;
        }


       
        transform.Rotate(0, x, 0);
        transform.Translate(0, 0, y);

        if (Input.GetKeyDown(KeyCode.Space))
            CmdFire();
        BulletFire.SetActive(true);

    }
    

    [Command]
     public void CmdFire()
    {
        //create Bullet from prefab
        GameObject bullet = (GameObject)Instantiate(bullatPrefab, bulletSpawn.position, bulletSpawn.rotation);

        //Add velocity to bullet
        bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * 6.0f;

        //spawn the bullet on clints
        NetworkServer.Spawn(bullet);

        //Destroy The bullet after 2 sec
        Destroy(bullet, 2);
    }



    public override void OnStartLocalPlayer()
    {

        GetComponent<MeshRenderer>().material.color = Color.blue;
        BulletFire.SetActive(true);
        
       
    }

}
