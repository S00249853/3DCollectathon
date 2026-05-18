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

    public Vector2 movementInput;
    private Vector2 previousMovement;
    private Vector3 movementVelocity;
    private Vector3 movement;
    private Vector3 appliedMovement;
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
    public bool CanSideflip;
    public bool CanWallJump;

    private Vector3 WallJumpForce;


    [SerializeField] private float FlipCd;
    private float FlipCdTimer;

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
            movementVelocity.y = movementVelocity.y + (JumpGravities[JumpCount] * fallMultiplier * Time.deltaTime);
            appliedMovement.y = Mathf.Max((previousYVelocity + movementVelocity.y) * .5f, -20.0f);
       
        }
        else
        {
            float previousYVelocity = movementVelocity.y;
            movementVelocity.y = movementVelocity.y + (JumpGravities[JumpCount] * Time.deltaTime);
            appliedMovement.y = (previousYVelocity + movementVelocity.y) * .5f;
        
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
            movementVelocity.y = InitialJumpVelocities[JumpCount];
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

        if (CanWallJump)
        {
            CanWallJump = false;
            movementVelocity = WallJumpForce;
            Invoke(nameof(ResetWallJump), MaxJumpTime / 2);
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (!characterController.isGrounded && hit.normal.y < 0.1f)
        {
            CanWallJump = true;
            WallJumpForce = hit.normal * WalkSpeed + transform.up * InitialJumpVelocities[1];
        }
        else
        {
            CanWallJump = false;
        }
    }

    private void ResetWallJump()
    {
        movementVelocity = Vector3.zero;
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

        if (FlipCdTimer > 0)
        {
            FlipCdTimer -= Time.deltaTime;
        }
        //  transform.Rotate(Vector3.up * movement.x);

        previousMovement = movementInput;

        if (previousMovement == -movementInput)
        {
            if (FlipCdTimer > 0)
            {
                return;
            }
            else
            {
                FlipCdTimer = FlipCd;
                Debug.Log("Can Sideflip test confirmed");
                JumpCount = 2;
            }
            
        }

        movement = transform.right * movementInput.x + transform.forward * movementInput.y;

        appliedMovement.x = movementVelocity.x;
        appliedMovement.z = movementVelocity.z;

        characterController.Move(((movement * currentSpeed) + appliedMovement) * Time.deltaTime);

        HandleGravity();
        HandleJump();
    }
}
