using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{

    public float clock;
    public float healthClock;
    float delayTime;

    public float currentPlayerHealth;
    public float maxPlayerHealth;
    public int playerHealthHungryDecrease;

    public float currentPlayerHunger;
    public float maxPlayerHunger;
    public float playerHungerDecrease;

    public float speed;
    public float sprintMultiplier;
    public float originalSpeed;

    public float distanceToGrass;
    public float distanceToTwigs;

    public float currentStamina = 100f;
    public float maxStamina;

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

    private Item itemToPickup;

    public Item grassItem;
    public Item twigsItem;

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

        if(Input.GetKeyDown(KeyCode.E))
        {
            UseSelectedItem();

            currentPlayerHunger += 25;
        }

        else if(Input.GetKeyUp(KeyCode.E))
        {
            
        }

        clock += Time.deltaTime;
        healthClock += Time.deltaTime;

        

        staminaBar.slider.value = currentStamina;
        interactBar.slider.value = interactSliderValue;
        healthBar.slider.value = currentPlayerHealth;
        hungerBar.slider.value = currentPlayerHunger;

        playerVelocity = rb.velocity;

        currentPlayerHunger -= playerHungerDecrease * Time.deltaTime;

        //-----------------------------------

        if (healthClock >= delayTime)
        {
            healthClock = 0f;

            if (currentPlayerHunger <= 0f)
            {
                currentPlayerHealth -= playerHealthHungryDecrease;
            }
        }

        if(currentPlayerHunger <= 0f)
        {
            currentPlayerHunger = 0f;
        }

        if(currentPlayerHunger >= maxPlayerHunger)
        {
            currentPlayerHunger = maxPlayerHunger;
        }

        if(currentPlayerHealth <= 0f)
        {
            currentPlayerHealth = 0f;
        }

        if(currentPlayerHealth > 100f)
        {
            currentPlayerHealth = maxPlayerHealth;
        }

        if (currentStamina == 0f)
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
            currentStamina -= (sprintDecrease * Time.deltaTime);
        }

        else if (isSprintPressed == false && sprintCooldownActive == false)
        {
            currentStamina += (sprintIncrease * Time.deltaTime);
        }


        if (currentStamina > maxStamina)
        {
            currentStamina = maxStamina;
        }

        else if (currentStamina < 0f)
        {
            currentStamina = 0f;
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





        if(This.GetChild(0).GetComponent<Detection>().targetGrass != null)
        {
            Vector3 targetGrass = This.GetChild(0).GetComponent<Detection>().targetGrass.position;
            distanceToGrass = Vector3.Distance(transform.position, targetGrass);

            if(distanceToGrass <= 2f && Input.GetKeyDown(KeyCode.Space))
            {
                interactBar.borderImage.enabled = true;
                interactBar.fillImage.enabled = true;

                interactCooldownActive = true;
                lastCollectedItem = This.GetChild(0).GetComponent<Detection>().targetGrassObject;
                itemToPickup = grassItem;
            }
        }

        if (This.GetChild(0).GetComponent<Detection>().targetTwigs != null)
        {
            Vector3 targetTwigs = This.GetChild(0).GetComponent<Detection>().targetTwigs.position;
            distanceToTwigs = Vector3.Distance(transform.position, targetTwigs);

            if (distanceToTwigs <= 2f && Input.GetKeyDown(KeyCode.Space))
            {
                interactBar.borderImage.enabled = true;
                interactBar.fillImage.enabled = true;

                interactCooldownActive = true;
                lastCollectedItem = This.GetChild(0).GetComponent<Detection>().targetTwigsObject;
                itemToPickup = twigsItem;
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
        bool canAdd = InventoryManager.instance.AddItem(itemToPickup);
        if(canAdd)
        {
            Object.Destroy(interactedObject);
            canMove = true;
            rb.drag = 10;
            interactBar.borderImage.enabled = false;
            interactBar.fillImage.enabled = false;
        }

    }

    public void GetSelectedItem()
    {
        Item receivedItem = InventoryManager.instance.GetSelectedItem(false);

        if(receivedItem != null)
        {
            Debug.Log("Item in slot: " + receivedItem);
        }

        else
        {
            Debug.Log("No item in slot.");
        }
    }

    public void UseSelectedItem()
    {
        Item receivedItem = InventoryManager.instance.GetSelectedItem(true);

        if (receivedItem != null)
        {
            Debug.Log(receivedItem + " consumed.");
        }

        else
        {
            Debug.Log("No item in slot to use.");
        }
    }

}
