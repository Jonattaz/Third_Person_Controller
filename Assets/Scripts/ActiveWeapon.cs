using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Animations;

public class ActiveWeapon : MonoBehaviour
{
    public enum WeaponSlot{
        Primary = 0,
        Secondary = 1 
    }
    public bool shooting;
    public Transform crossHairTarget;
    public UnityEngine.Animations.Rigging.Rig handIK;
    public Transform weaponLeftGrip;
    public Transform weaponRightGrip;
    public Animator rigController;
    public Cinemachine.CinemachineFreeLook playerCamera;
    public AmmoWidget ammoWidget; 
    public Transform[] weaponSlots;


    RaycastWeapon[] equippedWeapons = new RaycastWeapon[2];
    int activeWeaponIndex;
    bool isHolstered = false;

    // Start is called before the first frame update
    void Start(){

        RaycastWeapon existingWeapon = GetComponentInChildren<RaycastWeapon>();
        if(existingWeapon){
            Equip(existingWeapon);
        }
    }

    public RaycastWeapon GetActiveWeapon(){
        return GetWeapon(activeWeaponIndex);
    }

    RaycastWeapon GetWeapon(int index){
        if(index < 0 || index >= equippedWeapons.Length){   
            return null;
        }

        return equippedWeapons[index];
    }

    // Update is called once per frame
    void Update(){
        
        var weapon = GetWeapon(activeWeaponIndex);
        if(weapon && !isHolstered){

            if(Input.GetButtonDown("Fire1") || shooting){
                weapon.StartFiring();
            }

            if(weapon.isFiring){
                weapon.UpdateFiring(Time.deltaTime);
            }

            if(!shooting){
                weapon.StopFiring();
            }
             weapon.UpdateBullets(Time.deltaTime);
        }
        

         if(Input.GetKeyDown(KeyCode.X)){
            ToggleActiveWeapon();
        }

        if(Input.GetKeyDown(KeyCode.Alpha1)){
            SetActiveWeapon(WeaponSlot.Primary);
        }

        if(Input.GetKeyDown(KeyCode.Alpha2)){
            SetActiveWeapon(WeaponSlot.Secondary);

        }
    }

    public void Equip(RaycastWeapon newWeapon){
        int weaponSlotIndex = (int)newWeapon.weaponSlot;
        var weapon = GetWeapon(weaponSlotIndex);

        if(weapon){
            Destroy(weapon.gameObject);
        }

        weapon = newWeapon;
        weapon.raycastDestination = crossHairTarget;
        weapon.recoil.playerCamera = playerCamera;
        weapon.recoil.rigController = rigController;
        weapon.transform.SetParent(weaponSlots[weaponSlotIndex], false);
        equippedWeapons[weaponSlotIndex] = weapon;

        SetActiveWeapon(newWeapon.weaponSlot);
        ammoWidget.Refresh(weapon.ammoCount);
    }

    void ToggleActiveWeapon(){
        bool isHolstered = rigController.GetBool("holsterWeapon");
        if(isHolstered){
            StartCoroutine(ActivateWeapon(activeWeaponIndex));
        }else{
            StartCoroutine(HolsterWeapon(activeWeaponIndex));
        }
    }

    void SetActiveWeapon(WeaponSlot weaponSlot){
        int holsterIndex = activeWeaponIndex;
        int activateIndex = (int)weaponSlot;

        if(holsterIndex == activateIndex){
            holsterIndex = -1;
        }

        StartCoroutine(SwitchWeapon(holsterIndex, activateIndex));
    }

    IEnumerator SwitchWeapon(int holsterIndex, int activateIndex){
        yield return StartCoroutine(HolsterWeapon(holsterIndex));
        yield return StartCoroutine(ActivateWeapon(activateIndex));
        activeWeaponIndex = activateIndex;
    }

    IEnumerator HolsterWeapon(int index){
        isHolstered = true;
        var weapon = GetWeapon(index);
        if(weapon){
            rigController.SetBool("holsterWeapon", true);
            do{
                yield return new WaitForEndOfFrame();
            }while(rigController.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f);
        }
    }

    IEnumerator ActivateWeapon(int index){
        var weapon = GetWeapon(index);
        if(weapon){
            rigController.SetBool("holsterWeapon", false);
            rigController.Play("equip_" + weapon.weaponName);
            do{
                yield return new WaitForEndOfFrame();
            }while(rigController.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f);
        }
        isHolstered = false;
    }

}
