using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class walk : MonoBehaviour
{   

    bool runDown;
    Vector3 moveVec;
    Animator anim;

    void Awake(){
        anim = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        hAxis = Input.GetAxisRaw("Horizontal");
        VAxis = Input.GetAxisRaw("Vertical");
        runDown = Input.GetButtonDown("Run");

        moveVec = new Vector3(hAxis, 0 , vAxis).normalized;        
    }
}
