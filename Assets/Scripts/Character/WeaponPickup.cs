using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickup : MonoBehaviour{
    public RaycastWeapon weaponFab;

    void OnTriggerEnter(Collider other){
        ActiveWeapon activeWeapon = other.gameObject.GetComponent<ActiveWeapon>();
        if(activeWeapon){   
            RaycastWeapon newWeapon = Instantiate(weaponFab);
            activeWeapon.Equip(newWeapon);
        } 

        AIWeapons aiWeapon = other.gameObject.GetComponent<AIWeapons>();
        if(aiWeapon){   
            RaycastWeapon newWeapon = Instantiate(weaponFab);
            aiWeapon.Equip(newWeapon);
            Destroy(gameObject);
        } 
      


    }
}
