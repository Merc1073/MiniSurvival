using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{

    public float speed;
    public float distanceToFood;

    public float interactSliderValue = 0f;
    public float originalInteractSliderValue;

    public float grabCooldown = 100f;
    public float originalGrabCooldown;

    bool grabCooldownActive = false;
    bool canMove = true;

    public Vector3 movement;

    Rigidbody rb;

    Transform This;

    GameObject lastCollectedItem;

    public InteractionBar interactBar;


    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        This = gameObject.transform;
        interactBar.borderImage.enabled = false;
        interactBar.fillImage.enabled = false;

    }

    void Update()
    {

        interactBar.slider.value = interactSliderValue;

        if (grabCooldownActive == true)
        {
            if(Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D))
            {
                interactBar.borderImage.enabled = false;
                interactBar.fillImage.enabled = false;

                canMove = true;
                grabCooldown = originalGrabCooldown;
                interactSliderValue = originalInteractSliderValue;

                rb.drag = 10;
                grabCooldownActive = false;
            }

            else
            {
                rb.drag = 50;
                canMove = false;
                grabCooldown -= 100f * Time.deltaTime;
                interactSliderValue += 100f * Time.deltaTime;
            }
            

        }

        if(grabCooldown <= 0f)
        {
            grabCooldownActive = false;
            grabCooldown = originalGrabCooldown;
            interactSliderValue = originalInteractSliderValue;
            Harvesting(lastCollectedItem);
        }

        if(This.GetChild(0).GetComponent<Detection>().targetFood != null)
        {
            Vector3 targetFood = This.GetChild(0).GetComponent<Detection>().targetFood.position;
            distanceToFood = Vector3.Distance(transform.position, targetFood);
            //Debug.Log(distanceToFood);

            if(distanceToFood <= 2f && Input.GetKeyDown(KeyCode.Space))
            {
                interactBar.borderImage.enabled = true;
                interactBar.fillImage.enabled = true;

                grabCooldownActive = true;
                lastCollectedItem = This.GetChild(0).GetComponent<Detection>().targetFoodObject;
            }
        }


        float horizontalMovement = Input.GetAxisRaw("Horizontal");
        float verticalMovement = Input.GetAxisRaw("Vertical");

        if(canMove == true)
        {
            movement = new Vector3(horizontalMovement, 0, verticalMovement);
        }


        rb.AddForce(movement * speed);


    }

    void Harvesting(GameObject interactedObject)
    {
        Object.Destroy(interactedObject);
        canMove = true;
        rb.drag = 10;
        interactBar.borderImage.enabled = false;
        interactBar.fillImage.enabled = false;
    }
}
