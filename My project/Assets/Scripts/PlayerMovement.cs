using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField]private CharacterController characterController;

    //public bool IsCrouching {  get; private set; }
    public bool IsJumping { get; private set; }
    //public bool IsSprinting { get; private set; }

    public Vector3 Velocity { get { return characterController.velocity; } }

    private Vector2 movementInput;
    private Vector3 movementVelocity;
    private Vector3 movement;
    private Vector3 lastMovement;
    private float currentSpeed;

    [Header("Settings")]
    [SerializeField] private float WalkSpeed;
    //[SerializeField] private float SprintSpeed;
    //[SerializeField] private float CrouchSpeed;
    [SerializeField] private float DashSpeed;
    [SerializeField] private float CrouchHeight;
    [SerializeField] private float JumpHeight;
    [SerializeField] private float DashForce;
    [SerializeField] private float DashUpwardForce;
    [SerializeField] private float DashDuration;
    [SerializeField] private float DashCd;
    [SerializeField] private float Gravity = 2.81f;
    [SerializeField] private float WallJumpHeight;
    public int BaseJumpCount;
    private float DashCdTimer;
    private float NormalHeight;
    public int JumpCount;
    private Vector3 WallJumpForce;

    private bool canWallJump;

    private void Awake()
    {
        //characterController = GetComponent<CharacterController>();

        NormalHeight = characterController.height;
        currentSpeed = WalkSpeed;
    }

    public void OnMove(InputAction.CallbackContext obj)
    {
        movementInput = obj.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext obj)
    {
        JumpCount--;

        if (CanJump())
        {
            JumpCount = BaseJumpCount;
        }
        if (JumpCount > 0)
        {
            movementVelocity.y = Mathf.Sqrt(JumpHeight * 2F * Gravity);
        }

        if (canWallJump)
        {
            movementVelocity.y = Mathf.Sqrt(WallJumpHeight * 2F * Gravity);
            movementVelocity.x = WallJumpForce.x;
        }

    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (!characterController.isGrounded && hit.normal.y < 0.1f)
        {
            canWallJump = true;
            WallJumpForce = hit.normal * WalkSpeed;
        }
        else
        {
            canWallJump = false;
        }
    }

    public bool CanJump()
    {
        return characterController.isGrounded;
    }

    public void OnDash(InputAction.CallbackContext obj)
    {

        if (DashCdTimer > 0)
        {
            return;
        }
        else
        {
            DashCdTimer = DashCd;

            currentSpeed = DashSpeed;
            Vector3 forceToApply = transform.forward * DashForce + transform.up * DashUpwardForce;

            movementVelocity = forceToApply;

            Invoke(nameof(ResetDash), DashDuration);

        }

    }
    private void ResetDash()
    {
        currentSpeed = WalkSpeed;
        movementVelocity = Vector3.zero;
    }

    //public void OnSprint(InputAction.CallbackContext obj)
    //{
    //    IsSprinting = !IsSprinting;

    //    if (!IsCrouching)
    //    {
    //        currentSpeed = IsSprinting ? SprintSpeed : WalkSpeed;
    //    }
    //}

    //public void OnCrouch(InputAction.CallbackContext obj)
    //{
    //    IsCrouching = !IsCrouching;
    //    characterController.height = IsCrouching ? CrouchHeight : NormalHeight;
    //    currentSpeed = IsCrouching ? CrouchSpeed : WalkSpeed;
    //}

    private void Update()
    {
        if (characterController.isGrounded && movementVelocity.y < 0
            )
        {
            movementVelocity.y = -1f;
        }
        else 
        {
            movementVelocity.y -= Gravity * Time.deltaTime;
        }

        if (DashCdTimer > 0)
        {
            DashCdTimer -= Time.deltaTime;
        }

        movement = transform.right * movementInput.x + transform.forward * movementInput.y;
        movementVelocity.y -= Gravity * Time.deltaTime;

        characterController.Move(((movement * currentSpeed) + movementVelocity) * Time.deltaTime);
        
    }
}
