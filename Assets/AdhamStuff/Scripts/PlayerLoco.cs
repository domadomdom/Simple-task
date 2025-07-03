using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerLoco : MonoBehaviour
{
    Vector3 moverDir;
    private Transform cameraObj;
    AnimatorManager animatorManager;
    PlayerManager playerManager;
    InputManager inputManager;
    Rigidbody rb;
    public float inAirTimer;
    public float leapingVelocity;
    public float fallingVelocity;
    public float maxDistance = 1f;
    public float rayCastHeightOffset = 0.5f;
    public LayerMask groundLayer;
    public bool isSprinting;
    public float walkingSpeed = 1.5f;
    public float runningSpeed = 5f;
    public float sprintingSpeed = 7f;
    public float rotationSpeed = 20f;
    public bool isGrounded;
    public bool isJumping;
    public float jumpHeight = 3;
    public float gravityIntensity = -15;
    private void Awake()
    {
        playerManager = GetComponent<PlayerManager>();
        inputManager = GetComponent<InputManager>();
        rb = GetComponent<Rigidbody>();
        animatorManager = GetComponent<AnimatorManager>();
        cameraObj = Camera.main.transform;
    }
    public void HandleAllMovements()
    {
        HandleFallingAndLanding();
        if(playerManager.isInteracting)
        {
            return;
        }
        PlayerMover();
        RotateMe();
    }
    private void PlayerMover()
    {
        if(isJumping)
        {
            return;
        }
        moverDir = cameraObj.forward * inputManager.verticalInput;
        moverDir = moverDir + cameraObj.right * inputManager.horizontalInput;
        moverDir.Normalize();
        moverDir.y = 0;
        if (isSprinting)
        {
            moverDir = moverDir * sprintingSpeed;
        }
        else
        {


            if (inputManager.moveAmount >= 0.5f)
            {
                moverDir = moverDir * runningSpeed;
            }
            else
            {
                moverDir = moverDir * walkingSpeed;

            }
        }

        if (isGrounded && !isJumping)
        {
            Vector3 movementVelocity = moverDir;
            rb.linearVelocity = movementVelocity;
        }


    }
    private void RotateMe()
    {
        if(isJumping)
        {
            return;
        }
        Vector3 targetDir = Vector3.zero;

        targetDir = cameraObj.forward * inputManager.verticalInput;
        targetDir = targetDir + cameraObj.right * inputManager.horizontalInput;
        targetDir.Normalize();
        targetDir.y = 0;
        if (targetDir == Vector3.zero)
        {
            targetDir = transform.forward;
        }
        Quaternion targetRotation = Quaternion.LookRotation(targetDir);
        Quaternion playerRotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);


        if (isGrounded && !isJumping)
        {
            transform.rotation = playerRotation;
        }
    }
    private void HandleFallingAndLanding()
    {
        RaycastHit hit;
        Vector3 rayCastOrigin = transform.position;
        Vector3 targetPosition;
        rayCastOrigin.y = rayCastOrigin.y + rayCastHeightOffset;
        targetPosition = transform.position;
        if(!isGrounded && !isJumping)
        {
            if(!playerManager.isInteracting)
            {
                animatorManager.PlayTargetAnimation("Falling", true);
            }

            inAirTimer = inAirTimer + Time.deltaTime;
            rb.AddForce(transform.forward * leapingVelocity);
            rb.AddForce(-Vector3.up * fallingVelocity * inAirTimer);
        }
        if(Physics.SphereCast(rayCastOrigin, 0.2f, -Vector3.up, out hit, maxDistance, groundLayer)) 
        {
            if(!isGrounded && !playerManager.isInteracting)
            {
                animatorManager.PlayTargetAnimation("Land", true);
            }
            Vector3 rayCastHitPoint = hit.point;
            targetPosition.y = rayCastHitPoint.y;
            inAirTimer = 0f;
            isGrounded = true;
        }
        else
        {
            isGrounded = false; 
        }

        if(isGrounded && !isJumping) 
        {
            if(playerManager.isInteracting || inputManager.moveAmount > 0f)
            {
                transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime / 0.1f);
            }
            else
            {
                transform.position = targetPosition;
            }
        }
    }
    public void HandleJumping()
    {
        if(isGrounded)
        {
            animatorManager.animator.SetBool("isJumping", true);
            animatorManager.PlayTargetAnimation("Jump", false);

            float jumpingVelocity = Mathf.Sqrt(-2 * gravityIntensity * jumpHeight);
            Vector3 playerVelocity = moverDir;
            playerVelocity.y = jumpingVelocity;
            rb.linearVelocity = playerVelocity;
        }
    }
}
