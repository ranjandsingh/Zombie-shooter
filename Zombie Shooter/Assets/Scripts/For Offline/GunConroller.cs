using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunConroller : MonoBehaviour {

    public Transform weaponHold;
    public Gun[] allGuns;
    Gun equippedGun;

    void Start()
    {

    }

    public void EquipGun(Gun gunToEquip)
    {
        if (equippedGun != null)
        {
            Destroy(equippedGun.gameObject);
        }
        equippedGun = Instantiate(gunToEquip, weaponHold.position, weaponHold.rotation) as Gun;
        equippedGun.transform.parent = weaponHold;
    }
	public void EquipGun(int weaponindex){
		EquipGun (allGuns [weaponindex]);
		
	}
	public void OnTriggerHold() {
		if (equippedGun != null) {
			equippedGun.OnTriggerHold();
		}
	}

	public void OnTriggerRelease() {
		if (equippedGun != null) {
			equippedGun.OnTriggerRelease();
		}
	}
}
