using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{

    public float clock;
    public float healthClock;
    float delayTime;

    public float playerHealth;
    public int playerHealthHungryDecrease;

    public float playerHunger;
    public float playerHungerDecrease;

    public float speed;
    public float sprintMultiplier;
    public float originalSpeed;

    public float distanceToFood;

    public float stamina = 100f;

    public float sprintIncrease;
    public float sprintDecrease;

    public float sprintCooldown;
    public float sprintDefaultCooldown;

    public float staminaSliderValue = 100f;
    public float originalStaminaSliderValue;
    public float interactSliderValue = 0f;
    public float originalInteractSliderValue;

    public float interactCooldown = 100f;
    public float originalGrabCooldown;

    public float originalDrag;
    public float affectedDrag;

    public bool isSprintPressed = false;
    public bool sprintCooldownActive = false;

    public Vector3 playerVelocity;
    public Vector3 noMovement;

    bool interactCooldownActive = false;
    bool canMove = true;

    public Vector3 movement;

    Rigidbody rb;

    Transform This;

    GameObject lastCollectedItem;

    public InteractionBar interactBar;
    public StaminaBar staminaBar;
    public HealthBar healthBar;
    public HungerBar hungerBar;


    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        This = gameObject.transform;

        interactBar.borderImage.enabled = false;
        interactBar.fillImage.enabled = false;

        //noMovement = new Vector3(0, 0, 0);
        delayTime = 1f;

    }

    void Update()
    {

        clock += Time.deltaTime;
        healthClock += Time.deltaTime;

        

        staminaBar.slider.value = stamina;
        interactBar.slider.value = interactSliderValue;
        healthBar.slider.value = playerHealth;
        hungerBar.slider.value = playerHunger;

        playerVelocity = rb.velocity;

        playerHunger -= playerHungerDecrease * Time.deltaTime;

        //-----------------------------------

        if (healthClock >= delayTime)
        {
            healthClock = 0f;

            if (playerHunger <= 0f)
            {
                playerHealth -= playerHealthHungryDecrease;
            }
        }

        if(playerHunger <= 0f)
        {
            playerHunger = 0f;
        }

        if(playerHealth <= 0f)
        {
            playerHealth = 0f;
        }

        if (stamina == 0f)
        {
            sprintCooldownActive = true;

            speed = originalSpeed;
        }

        if (sprintCooldown <= 0f)
        {
            sprintCooldownActive = false;
        }


        if (sprintCooldownActive == true)
        {
            sprintCooldown -= 1f * Time.deltaTime;
        }

        else if (sprintCooldownActive == false)
        {
            sprintCooldown = sprintDefaultCooldown;
        }


        if(sprintCooldown < 0f)
        {
            sprintCooldown = 0f;
        }


        //-----------------------------------


        if (Input.GetKeyDown(KeyCode.LeftShift))
        {

            isSprintPressed = true;

            speed *= sprintMultiplier;

        }
        

        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {

            sprintCooldownActive = true;

            isSprintPressed = false;

            speed = originalSpeed;

        }



        //-----------------------------------



        if (isSprintPressed == true)
        {
            stamina -= (sprintDecrease * Time.deltaTime);
        }

        else if (isSprintPressed == false && sprintCooldownActive == false)
        {
            stamina += (sprintIncrease * Time.deltaTime);
        }


        if (stamina > 100f)
        {
            stamina = 100f;
        }

        else if (stamina < 0f)
        {
            stamina = 0f;
        }



        //-----------------------------------


        //Cancel interacting when moving.

        if (interactCooldownActive == true)
        {
            if(Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D))
            {
                interactBar.borderImage.enabled = false;
                interactBar.fillImage.enabled = false;

                canMove = true;
                interactCooldown = originalGrabCooldown;
                interactSliderValue = originalInteractSliderValue;

                rb.drag = originalDrag;
                interactCooldownActive = false;
            }

            else
            {
                rb.drag = affectedDrag;
                canMove = false;
                interactCooldown -= 100f * Time.deltaTime;
                interactSliderValue += 100f * Time.deltaTime;
            }
            

        }




        //-----------------------------------





        if(This.GetChild(0).GetComponent<Detection>().targetFood != null)
        {
            Vector3 targetFood = This.GetChild(0).GetComponent<Detection>().targetFood.position;
            distanceToFood = Vector3.Distance(transform.position, targetFood);
            //Debug.Log(distanceToFood);

            if(distanceToFood <= 2f && Input.GetKeyDown(KeyCode.Space))
            {
                interactBar.borderImage.enabled = true;
                interactBar.fillImage.enabled = true;

                interactCooldownActive = true;
                lastCollectedItem = This.GetChild(0).GetComponent<Detection>().targetFoodObject;
            }
        }

        if (interactCooldown <= 0f)
        {
            interactCooldownActive = false;
            interactCooldown = originalGrabCooldown;
            interactSliderValue = originalInteractSliderValue;
            Harvesting(lastCollectedItem);
        }

        float horizontalMovement = Input.GetAxisRaw("Horizontal");
        float verticalMovement = Input.GetAxisRaw("Vertical");

        if(canMove == true)
        {
            movement = new Vector3(horizontalMovement, 0, verticalMovement);
        }


        rb.AddForce(movement * speed * Time.deltaTime);


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
