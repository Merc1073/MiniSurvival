using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{

    [Header("Time variables")]
    public float clock;
    public float healthClock;
    float delayTime;

    [Header("Health variables")]
    public float currentPlayerHealth;
    public float maxPlayerHealth;
    public int playerHealthHungryDecrease;

    [Header("Hunger variables")]
    public float currentPlayerHunger;
    public float maxPlayerHunger;
    public float playerHungerDecrease;

    [Header("Stamina variables")]
    public float currentStamina = 100f;
    public float maxStamina;

    [Header("Speed variables")]
    public float speed;
    public float sprintMultiplier;
    public float originalSpeed;

    [Header("Change in speed variables")]
    public float sprintIncrease;
    public float sprintDecrease;

    [Header("Speed cooldown variables")]
    public float sprintCooldown;
    public float sprintDefaultCooldown;

    [Header("Distance to objects")]
    public float distanceToGrass;
    public float distanceToTwigs;

    [Header("HUD slider variables")]
    public float staminaSliderValue = 100f;
    public float originalStaminaSliderValue;
    public float interactSliderValue = 0f;
    public float originalInteractSliderValue;

    [Header("Iteraction variables")]
    public float interactCooldown = 100f;
    public float originalGrabCooldown;

    [Header("Drag physics variables")]
    public float originalDrag;
    public float affectedDrag;

    [Header("Booleans variables")]
    public bool isSprintPressed = false;
    public bool sprintCooldownActive = false;


    bool interactCooldownActive = false;
    bool canMove = true;

    [Header("Movement variables")]
    public Vector3 playerVelocity;

     Vector3 noMovement;
    
    private Item itemToPickup;

    public Item grassItem;
    public Item twigsItem;

    public Vector3 movement;

    Rigidbody rb;

    Transform This;

    GameObject lastCollectedItem;

    [Header("Transforms")]
    public Transform camera;
    public Transform cameraPivot;

    [Header("Objects")]
    public InteractionBar interactBar;
    public StaminaBar staminaBar;
    public HealthBar healthBar;
    public HungerBar hungerBar;

    public Vector3 cam2Forward;
    public Vector3 cam2Right;


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


        float horizontalMovement = Input.GetAxisRaw("Horizontal");
        float verticalMovement = Input.GetAxisRaw("Vertical");

        float mouseWheelInput = Input.GetAxis("Mouse ScrollWheel");

        Vector3 camForward = camera.forward;
        Vector3 camRight = camera.right;
        Vector3 camUp= camera.up;

        cam2Forward = camera.forward;
        cam2Right = camera.right;

        camForward.y = 0;
        camRight.y = 0;


        Vector3 forwardRelative = verticalMovement * camForward;
        Vector3 rightRelative = horizontalMovement * camRight;

        Vector3 forwardCameraRelative = mouseWheelInput * camForward;
        Vector3 rightCameraRelative = mouseWheelInput * camRight;

        Vector3 moveDirection = forwardRelative + rightRelative;


        if (canMove == true)
        {
            movement = new Vector3(moveDirection.x, 0, moveDirection.z).normalized;
        }

        rb.AddForce(movement * speed * Time.deltaTime);

        

        //if (mouseWheelInput > 0f)
        //{
        //    cameraPivot.transform.position += mouseWheelInput * (cameraDirection) * 20;
        //}

        //if (mouseWheelInput < 0f)
        //{
        //    cameraPivot.transform.position += mouseWheelInput * (-cameraDirection) * 20;
        //}


        if (Input.GetKeyDown(KeyCode.F))
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

            sprintCooldown = sprintDefaultCooldown;

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

    void MovePlayerRelativeToCamera()
    {
        float playerVerticalInput = Input.GetAxis("Vertical");
        float playerHorizontalInput = Input.GetAxis("Horizontal");

        Vector3 forward = Camera.main.transform.forward;
        Vector3 right = Camera.main.transform.right;
        forward.y = 0;
        right.y = 0;
        forward = forward.normalized;
        right = right.normalized;

        Vector3 forwardRelativeVerticalInput = playerVerticalInput * forward;
        Vector3 rightRelativeVerticalInput = playerHorizontalInput * right;

        Vector3 cameraRelativeMovement = forwardRelativeVerticalInput + rightRelativeVerticalInput;

        transform.Translate(cameraRelativeMovement * Time.deltaTime * 20, Space.World);


    }

}
