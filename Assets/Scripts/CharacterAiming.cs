using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class CharacterAiming : MonoBehaviour
{

    public float turnSpeed = 15;
    public float aimDuration = 0.3f;

    public Rig aimLayer;

    Camera mainCamera;
    
    // Start is called before the first frame update
    void Start(){
        mainCamera = Camera.main;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void FixedUpdate(){
        float yamCamera = mainCamera.transform.rotation.eulerAngles.y;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, yamCamera, 0)
            , turnSpeed * Time.deltaTime);        
    }

    void LateUpdate(){
        
        if(aimLayer){
            /*if(Input.GetMouseButton(1)){
                aimLayer.weight += Time.deltaTime / aimDuration;
            }else{
                aimLayer.weight -= Time.deltaTime / aimDuration;
            }*/

            aimLayer.weight = 1.0f;
        }

       
        /*if(Input.GetButtonUp("Fire1")){
            weapon.StopFiring();
        }*/

    }
}
