using UnityEngine;

public class InputManager : MonoBehaviour
{

    Locomotion inputActions;
    PlayerLoco playerLoco; 
    AnimatorManager animatorManager;
    public Vector2 moveInput;
    public float moveAmount;
    public float horizontalInput;
    public float verticalInput;
    public Vector2 cameraInput;
    public float cameraInputX;
    public float cameraInputY;
    public bool shift_Input;
    public bool jump_Input;
    private void OnEnable()
    {
        if (inputActions == null)
        {
            inputActions = new Locomotion();
            inputActions.PlayerMovement.Movement.performed += i => moveInput = i.ReadValue<Vector2>();
            inputActions.PlayerMovement.Camera.performed += i => cameraInput = i.ReadValue<Vector2>();
            inputActions.PlayerActions.Shift.performed += i => shift_Input = true;
            inputActions.PlayerActions.Shift.canceled += i => shift_Input = false;
            inputActions.PlayerActions.Jump.performed += i => jump_Input = true;
        }
        inputActions.Enable();
    }
    private void Awake()
    {
        animatorManager = GetComponent<AnimatorManager>();
        playerLoco = GetComponent<PlayerLoco>();
    }
    private void OnDisable()
    {
        inputActions.Disable();
    }
    public void HandleAllInputs()
    {
        MovementInput();
        HandleSprintingInput();
        HandleJumpingInput();
    }
    private void MovementInput()
    {
        verticalInput = moveInput.y;
        horizontalInput = moveInput.x;
        cameraInputX = cameraInput.x;
        cameraInputY = cameraInput.y;
        moveAmount = Mathf.Clamp01(Mathf.Abs(horizontalInput) + Mathf.Abs(verticalInput));
        animatorManager.UpdateAnimatorValues(0, moveAmount, playerLoco.isSprinting);
    }
    private void HandleSprintingInput()
    {
        if (shift_Input && moveAmount > 0.5f)
        {
            playerLoco.isSprinting = true;
        }
        else
        {
            playerLoco.isSprinting = false;
        }
    }
    public void HandleJumpingInput()
    {
        if(jump_Input)
        {
            jump_Input = false;
            playerLoco.HandleJumping();
        }
    }
}
