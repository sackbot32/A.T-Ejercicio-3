using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerControl : MonoBehaviour
{
    //Components
    private CharacterController controller;
    private Transform cameraTransform;
    //Settings
    [SerializeField]
    private float speed;
    [SerializeField]
    private float runMultiplier;
    [SerializeField]
    private float jumpForce;
    [SerializeField]
    private float gravity = Physics.gravity.y;
        //Sprint drain settings
    [SerializeField]
    private float maxSprintValue;
    [SerializeField]
    private float sprintingDrainingRate;
    [SerializeField]
    private float sprintingDrainingAmmount;
    [SerializeField]
    private Slider sprintBar;

    //Data
    private Vector2 moveInput;
    private Vector2 lookInput;
    private Vector2 velocity;
    private float verticalVelocity;
    private float verticalRotation=0;
    private bool isSprinting;
    //Sprint drain data
    private bool canSprint;
    private float currentSprintValue;
    private bool isDraining;

    [SerializeField]
    private float lookSentitivy = 1f;
    [SerializeField]
    private float maxLookAngle = 90f;


    
    void Start()
    {
        sprintBar.maxValue = maxSprintValue;
        canSprint = true;
        currentSprintValue = maxSprintValue;
        sprintBar.value = currentSprintValue;
        controller = GetComponent<CharacterController>();
        cameraTransform = Camera.main.transform;

        Cursor.lockState = CursorLockMode.Locked;
    }

    
    void Update()
    {
        MovePlayer();

        LookAround();
    }

    /// <summary>
    /// Recieves movement input from inputsystem
    /// </summary>
    /// <param name="context"></param>
    public void Move(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }
    public void Look(InputAction.CallbackContext context)
    {
        lookInput = context.ReadValue<Vector2>();
    }
    public void Jump(InputAction.CallbackContext context)
    {
        print("Jump");
        if (controller.isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
        }
    }

    public void Sprint(InputAction.CallbackContext context)
    {
        if(currentSprintValue > 0 && canSprint)
        {
            isSprinting = context.started || context.performed;
        } else
        {
            isSprinting = false;
        }

        if(!isDraining && isSprinting)
        {
           StartCoroutine(DrainStamina());
        }
    }

    private void MovePlayer()
    {
        if (controller.isGrounded)
        {
            verticalVelocity = 0f;
        } else
        {
            verticalVelocity += gravity * Time.deltaTime;
        }
        controller.Move(new Vector3(0, verticalVelocity, 0) * Time.deltaTime);
        Vector3 moveDir = new Vector3(moveInput.x, 0, moveInput.y);
        moveDir = transform.TransformDirection(moveDir);
        float targetSpeed = isSprinting ? speed * runMultiplier : speed;
        controller.Move(moveDir * targetSpeed * Time.deltaTime);
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    private void LookAround()
    {
        float horizontalRot = lookInput.x * lookSentitivy;
        verticalRotation += -lookInput.y * lookSentitivy;

        verticalRotation = Mathf.Clamp(verticalRotation, -maxLookAngle, maxLookAngle);

        transform.Rotate(Vector3.up * horizontalRot);
        cameraTransform.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);

        

    }

    private IEnumerator DrainStamina()
    {
        isDraining = true;
        while(currentSprintValue > 0f && isSprinting)
        {
            currentSprintValue -= sprintingDrainingAmmount;
            sprintBar.value = currentSprintValue;
            yield return new WaitForSeconds(sprintingDrainingRate);
        }
        if(currentSprintValue > 0f)
        {
            StartCoroutine(RegainStamina());
        } else
        {
            StartCoroutine(RegainFromZero());
        }
    }

    private IEnumerator RegainStamina()
    {
        isDraining = false;
        while (currentSprintValue < maxSprintValue && !isSprinting)
        {
            currentSprintValue += sprintingDrainingAmmount;
            sprintBar.value = currentSprintValue;
            yield return new WaitForSeconds(sprintingDrainingRate);
        }
        if (!isSprinting)
        {
            currentSprintValue = maxSprintValue;
            sprintBar.value = currentSprintValue;
        }
    }

    private IEnumerator RegainFromZero()
    {
        isDraining = false;
        canSprint = false;
        isSprinting = false;
        currentSprintValue = 0;
        while (currentSprintValue < maxSprintValue && !isSprinting)
        {
            currentSprintValue += sprintingDrainingAmmount/2f;
            sprintBar.value = currentSprintValue;
            yield return new WaitForSeconds(sprintingDrainingRate);
        }
        currentSprintValue = maxSprintValue;
        sprintBar.value = currentSprintValue;
        canSprint = true;
    }
   

}
