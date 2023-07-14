using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSmoothRotation : MonoBehaviour
{

    public Transform target;
    public float turnSpeed;
    Quaternion rotationGoal;
    Vector3 direction;

    private void Update()
    {

        direction = (transform.position - target.position).normalized;

        rotationGoal = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotationGoal, turnSpeed);


    }

}
