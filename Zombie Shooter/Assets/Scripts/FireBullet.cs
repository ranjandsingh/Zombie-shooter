using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBullet : MonoBehaviour {
    private PlayerControler player;
   void Start()
    {
        player = GetComponentInParent<PlayerControler>();
    }

   public void assingPlayer()
   {
       player = GetComponentInParent<PlayerControler>();
   }

    public void fire()
    {
        if(player.isLocalPlayer)
        player.CmdFire();
    }
}
