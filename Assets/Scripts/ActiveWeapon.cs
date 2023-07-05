using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Animations;

public class ActiveWeapon : MonoBehaviour
{
    public bool shooting;
    public Transform crossHairTarget;
    public UnityEngine.Animations.Rigging.Rig handIK;
    public Transform weaponParent;
    public Transform weaponLeftGrip;
    public Transform weaponRightGrip;

    RaycastWeapon weapon;
    Animator anim;
    AnimatorOverrideController overrides;

    // Start is called before the first frame update
    void Start(){
        anim = GetComponent<Animator>();
        overrides = anim.runtimeAnimatorController as AnimatorOverrideController;

        weapon = GetComponentInChildren<RaycastWeapon>();
        RaycastWeapon existingWeapon = GetComponentInChildren<RaycastWeapon>();
        if(existingWeapon){
            Equip(existingWeapon);
        }


    }

    // Update is called once per frame
    void Update(){
        
        if(weapon){
            handIK.weight = 1.0f;
            anim.SetLayerWeight(1, 1.0f);
            Invoke(nameof(SetAnimationDelayed), 0.001f);

            if(Input.GetButtonDown("Fire1") || shooting){
                weapon.StartFiring();
            }

            if(weapon.isFiring){
                weapon.UpdateFiring(Time.deltaTime);
            }

            weapon.UpdateBullets(Time.deltaTime);

            if(!shooting){
                weapon.StopFiring();
            }
        }else{
            handIK.weight = 0.0f;
            anim.SetLayerWeight(1, 0.0f);
        }
    }

    void SetAnimationDelayed(){
        overrides["EmptyAnimation"] = weapon.weaponAnimation;
    }

    public void Equip(RaycastWeapon newWeapon){
        
        if(weapon){
            Destroy(weapon.gameObject);
        }

        weapon = newWeapon;
        weapon.raycastDestination = crossHairTarget;
        weapon.transform.parent = weaponParent;
        weapon.transform.localPosition = Vector3.zero;
        weapon.transform.localRotation = Quaternion.identity;
        
    }

    [ContextMenu("Save weapon pose")]
    void SaveWeaponPose(){
        GameObjectRecorder recorder = new GameObjectRecorder(gameObject);
        recorder.BindComponentsOfType<Transform>(weaponParent.gameObject, false);
        recorder.BindComponentsOfType<Transform>(weaponLeftGrip.gameObject, false);
        recorder.BindComponentsOfType<Transform>(weaponRightGrip.gameObject, false);
        recorder.TakeSnapshot(0.0f);
        recorder.SaveToClip(weapon.weaponAnimation);
        UnityEditor.AssetDatabase.SaveAssets();

    }
}
