using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField]private CharacterController characterController;

    //public bool IsCrouching {  get; private set; }
   
    //public bool IsSprinting { get; private set; }

    public Vector3 Velocity { get { return characterController.velocity; } }

    private Vector2 movementInput;
    private Vector3 movementVelocity;
    private Vector3 movement;
    private Vector3 lastMovement;
    private float currentSpeed;

    [Header("Settings")]
    [SerializeField] private float WalkSpeed;
    [SerializeField] private float DashSpeed;
    [SerializeField] private float DashForce;
    [SerializeField] private float DashUpwardForce;
    [SerializeField] private float DashDuration;
    [SerializeField] private float DashCd;
     private float Gravity = -9.81f;
    private float GroundedGravity = -.05f;
    private float DashCdTimer;
    public bool IsJumping;
    public bool IsJumpPressed;
    private float InitialJumpVelocity;
    private float MaxJumpHeight = 4f;
    private float MaxJumpTime = 1f;
    private int JumpCount = 0;
    Dictionary<int, float> InitialJumpVelocities = new Dictionary<int, float>();
    Dictionary<int, float> JumpGravities = new Dictionary<int, float>();

    Coroutine CurrentJumpResetRoutine = null;
    bool JumpCountdown;

    private void Awake()
    {
        //characterController = GetComponent<CharacterController>();
        currentSpeed = WalkSpeed;

        SetupJumpVariables();
    }

    private void SetupJumpVariables()
    {
        float timeToApex = MaxJumpTime / 2;
        Gravity = (-2 * MaxJumpHeight) / Mathf.Pow(timeToApex, 2);
        InitialJumpVelocity = (2 * MaxJumpHeight) / timeToApex;
        float SecondGravity = (-2 * (MaxJumpHeight + 2)) / Mathf.Pow(timeToApex * 1.25f, 2);
        float SecondJumpVelocity = (2 * (MaxJumpHeight + 2)) / (timeToApex* 1.25f);
        float ThirdGravity = (-2 * (MaxJumpHeight + 4)) / Mathf.Pow(timeToApex * 1.5f, 2);
        float ThirdJumpVelocity = (2 * (MaxJumpHeight + 4)) / (timeToApex * 1.5f);

        InitialJumpVelocities.Add(1, InitialJumpVelocity);
        InitialJumpVelocities.Add(2, SecondJumpVelocity);
        InitialJumpVelocities.Add(3, ThirdJumpVelocity);

        JumpGravities.Add(0, Gravity);
        JumpGravities.Add(1, Gravity);
        JumpGravities.Add(2, SecondGravity);
        JumpGravities.Add(3, ThirdGravity);
    }

    private void HandleGravity()
    {
        bool isFalling = movementVelocity.y <= 0.0f || !IsJumpPressed;
        float fallMultiplier = 2.0f;
        if (characterController.isGrounded && movementVelocity.y <= 0)
        {
            if (JumpCountdown)
            {
                CurrentJumpResetRoutine = StartCoroutine(JumpResetRoutine());
                JumpCountdown = false;
            }
            movementVelocity.y = GroundedGravity;
        }
        else if (isFalling)
        {
            float previousYVelocity = movementVelocity.y;
            float newYVelocity = movementVelocity.y + (JumpGravities[JumpCount] * fallMultiplier * Time.deltaTime);
            float nextYVelocity = Mathf.Max((previousYVelocity + newYVelocity) * .5f, -20.0f);
            movementVelocity.y = nextYVelocity;
        }
        else
        {
            float previousYVelocity = movementVelocity.y;
            float newYVelocity = movementVelocity.y + (JumpGravities[JumpCount] * Time.deltaTime);
            float nextYVelocity = (previousYVelocity + newYVelocity) * .5f;
            movementVelocity.y = nextYVelocity;
        }
    }

    public void OnMove(InputAction.CallbackContext obj)
    {
        movementInput = obj.ReadValue<Vector2>();
    }

    IEnumerator JumpResetRoutine()
    {
        yield return new WaitForSeconds(.5f);
        JumpCount = 0;
    }

    private void HandleJump()
    {
        if (!IsJumping && characterController.isGrounded && IsJumpPressed
       )
        {
            if (JumpCount < 3 && CurrentJumpResetRoutine != null)
            {
                StopCoroutine(CurrentJumpResetRoutine);
            }
            IsJumping = true;
            JumpCount++;
            movementVelocity.y = InitialJumpVelocities[JumpCount] * .5f;
        }
        else if (IsJumping && characterController.isGrounded && !IsJumpPressed
            )
        {
            JumpCountdown = true;
            IsJumping = false;
        }
    }

    public void OnJump(InputAction.CallbackContext obj)
    {
        IsJumpPressed = obj.ReadValueAsButton();
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        //if (!characterController.isGrounded && hit.normal.y < 0.1f)
        //{
        //    canWallJump = true;
        //    WallJumpForce = hit.normal * WalkSpeed;
        //}
        //else
        //{
        //    canWallJump = false;
        //}
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
            Vector3 forceToApply = characterController.transform.forward * DashForce + transform.up * DashUpwardForce;

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



        if (DashCdTimer > 0)
        {
            DashCdTimer -= Time.deltaTime;
        }
      //  transform.Rotate(Vector3.up * movement.x);

        movement = transform.right * movementInput.x + transform.forward * movementInput.y;

        characterController.Move(((movement * currentSpeed) + movementVelocity) * Time.deltaTime);

        HandleGravity();
        HandleJump();
    }
}
