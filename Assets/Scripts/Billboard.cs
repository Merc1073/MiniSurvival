using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{


    Transform cameraObject;

    private void Start()
    {
        cameraObject = GameObject.FindGameObjectWithTag("MainCamera").transform;
    }


    void LateUpdate()
    {
        transform.LookAt(transform.position + cameraObject.forward);
    }

}
