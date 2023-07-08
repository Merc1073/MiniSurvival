using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public float speed;

    public float distanceToFood;

    public Vector3 movement;

    Rigidbody rb;
    public Transform This;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        This = gameObject.transform;
    }

    void Update()
    {

        if(This.GetChild(0).GetComponent<Detection>().targetFood != null)
        {
            Vector3 targetFood = This.GetChild(0).GetComponent<Detection>().targetFood.position;
            distanceToFood = Vector3.Distance(transform.position, targetFood);
            Debug.Log(distanceToFood);

            if(distanceToFood <= 2f && Input.GetKeyDown(KeyCode.Space))
            {
                Object.Destroy(This.GetChild(0).GetComponent<Detection>().targetFoodObject);

            }

        }
        


        float horizontalMovement = Input.GetAxisRaw("Horizontal");
        float verticalMovement = Input.GetAxisRaw("Vertical");

        movement = new Vector3(horizontalMovement, 0, verticalMovement);


        rb.AddForce(movement * speed);







    }
}
