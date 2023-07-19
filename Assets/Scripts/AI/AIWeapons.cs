using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIWeapons : MonoBehaviour{

    RaycastWeapon currentWeapon;
    Animator animator;
    MeshSockets sockets;
    WeaponIK weaponIk;
    Transform currentTarget;


    void Start(){
        animator = GetComponent<Animator>();    
        sockets = GetComponent<MeshSockets>();
        weaponIk = GetComponent<WeaponIK>();    
    }

    public void Equip(RaycastWeapon weapon){
        currentWeapon = weapon;
        sockets.Attach(weapon.transform, MeshSockets.SocketId.Spine);
    }

    public void ActiveWeapon(){
        StartCoroutine(EquipWeapon());
    }

    IEnumerator EquipWeapon(){
        animator.SetBool("Equip", true);
        yield return new WaitForSeconds(0.5f);
        while(animator.GetCurrentAnimatorStateInfo(1).normalizedTime < 1.0f){
            yield return null;
        }

        weaponIk.SetAimTransform(currentWeapon.raycastOrigin);

    }


    public void DropWeapon(){
        if(currentWeapon){
            currentWeapon.transform.SetParent(null);
            currentWeapon.gameObject.GetComponent<BoxCollider>().enabled = true;
            currentWeapon.gameObject.AddComponent<Rigidbody>();
            currentWeapon = null;
        }
    }

    public bool HasWeapon(){
        return currentWeapon != null;
    }

    public void OnAnimationEvent(string eventName){
        if(eventName == "equipWeapon"){
            sockets.Attach(currentWeapon.transform, MeshSockets.SocketId.RightHand);
        }
    }

    public void SetTarget(Transform target){
        weaponIk.SetTargetTransform(target);
        currentTarget = target;
    }

}
