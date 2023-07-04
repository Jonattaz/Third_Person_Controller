using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAiming : MonoBehaviour
{

    public float turnSpeed = 15;
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
}
