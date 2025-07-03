using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    Animator animator;
    InputManager inputManager;
    PlayerLoco playerLocomotion;
    CameraManager cameraManager;
    public bool isInteracting;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        inputManager = GetComponent<InputManager>();
        playerLocomotion = GetComponent<PlayerLoco>();
        cameraManager = FindObjectOfType<CameraManager>();
    }

    private void Update()
    {

        inputManager.HandleAllInputs(); 
    }

    private void FixedUpdate()
    {
        playerLocomotion.HandleAllMovements();
    }
    private void LateUpdate()
    {
        cameraManager.HandleAllCameraMovement();
        isInteracting = animator.GetBool("IsInteracting");
        playerLocomotion.isJumping = animator.GetBool("isJumping");
        animator.SetBool("isGrounded", playerLocomotion.isGrounded);
    }
}
