using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public float speed;

    public Vector3 movement;

    Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    void Update()
    {

        //float distanceToFood = transform.position - 

        float horizontalMovement = Input.GetAxisRaw("Horizontal");
        float verticalMovement = Input.GetAxisRaw("Vertical");

        movement = new Vector3(horizontalMovement, 0, verticalMovement);


        rb.AddForce(movement * speed);







    }
}
