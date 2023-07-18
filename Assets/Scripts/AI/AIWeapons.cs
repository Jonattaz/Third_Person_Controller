using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIWeapons : MonoBehaviour{

    RaycastWeapon currentWeapon;
    Animator animator;
    MeshSockets sockets;

    void Start(){
        animator = GetComponent<Animator>();    
        sockets = GetComponent<MeshSockets>();    
    }

    public void Equip(RaycastWeapon weapon){
        currentWeapon = weapon;
        sockets.Attach(weapon.transform, MeshSockets.SocketId.Spine);
    }

    public void ActiveWeapon(){
        animator.SetBool("Equip", true);
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
}
