using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    //Character Controller
    [SerializeField]private CharacterController _characterController;

    //Movement Variables
    public Vector3 Velocity { get { return _characterController.velocity; } }
    public Vector2 MovementInput;
    private Vector2 _previousMovement;
    private Vector3 _movementVelocity;
    private Vector3 _movement;
    private Vector3 _appliedMovement;
    private float _currentSpeed;
    public bool isMovementPressed;
    Vector3 _cameraRelativeMovement;

    //Editable Variables
    [Header("Settings")]
    [SerializeField] private float WalkSpeed;
    [SerializeField] private float DashSpeed;
    [SerializeField] private float DashForce;
    [SerializeField] private float DashUpwardForce;
    [SerializeField] private float DashDuration;
    [SerializeField] private float DashCd;
    [SerializeField] private float FlipCd;

    //Gravity Variables
    private float _gravity = -9.81f;
    private float _groundedGravity = -.05f;

    //Jumping Variables
    public bool IsJumping;
    public bool IsJumpPressed;
    private float _initialJumpVelocity;
    private float _maxJumpHeight = 2f;
    private float _maxJumpTime = 1f;
    private int _jumpCount = 0;
    Dictionary<int, float> _initialJumpVelocities = new Dictionary<int, float>();
    Dictionary<int, float> _jumpGravities = new Dictionary<int, float>();
    Coroutine _currentJumpResetRoutine = null;
    bool _jumpCountdown;

    //Miscellanious Variables
    public bool CanSideflip;
    public bool CanWallJump;
    private float _dashCdTimer;
    float _rotationFactorPerFrame = 15.0f;
    private Vector3 _wallJumpForce;
    private float _flipCdTimer;

    private void Awake()
    {
        _currentSpeed = WalkSpeed;

        SetupJumpVariables();
    }

    private void SetupJumpVariables()
    {
        float timeToApex = _maxJumpTime / 2;
        _gravity = (-2 * _maxJumpHeight) / Mathf.Pow(timeToApex, 2);
        _initialJumpVelocity = (2 * _maxJumpHeight) / timeToApex;
        float secondGravity = (-2 * (_maxJumpHeight + 2)) / Mathf.Pow(timeToApex * 1.25f, 2);
        float secondJumpVelocity = (2 * (_maxJumpHeight + 2)) / (timeToApex* 1.25f);
        float thirdGravity = (-2 * (_maxJumpHeight + 4)) / Mathf.Pow(timeToApex * 1.5f, 2);
        float thirdJumpVelocity = (2 * (_maxJumpHeight + 4)) / (timeToApex * 1.5f);

        _initialJumpVelocities.Add(1, _initialJumpVelocity);
        _initialJumpVelocities.Add(2, secondJumpVelocity);
        _initialJumpVelocities.Add(3, thirdJumpVelocity);

        _jumpGravities.Add(0, _gravity);
        _jumpGravities.Add(1, _gravity);
        _jumpGravities.Add(2, secondGravity);
        _jumpGravities.Add(3, thirdGravity);
    }

    private void HandleGravity()
    {
        bool isFalling = _movement.y <= 0.0f || !IsJumpPressed;
        float fallMultiplier = 2.0f;
        if (_characterController.isGrounded && _movement.y <= 0)
        {
            if (_jumpCountdown)
            {
                _jumpCountdown = false;
                _currentJumpResetRoutine = StartCoroutine(JumpResetRoutine());
                if (_jumpCount == 3)
                {
                    _jumpCount = 0;
                }
            }
            _movement.y = _groundedGravity;
           // _appliedMovement.y = _groundedGravity;
        }
        else if (isFalling)
        {
            float previousYVelocity = _movement.y;
            _movement.y = _movement.y + (_jumpGravities[_jumpCount] * fallMultiplier * Time.deltaTime);
            _appliedMovement.y = Mathf.Max((previousYVelocity + _movement.y) * .5f, -20.0f);
       
        }
        else
        {
            float previousYVelocity = _movement.y;
            _movement.y = _movement.y + (_jumpGravities[_jumpCount] * Time.deltaTime);
            _appliedMovement.y = (previousYVelocity + _movement.y) * .5f;
        
        }
    }

    private void HandleRotation()
    {
        Vector3 positionToLookAt;

        positionToLookAt.x = _cameraRelativeMovement.x;
        positionToLookAt.y = 0;
        positionToLookAt.z = _cameraRelativeMovement.z;

        Quaternion currentRotation = transform.rotation;

        if(isMovementPressed)
        {
            Quaternion targetRotation = Quaternion.LookRotation(positionToLookAt);

            transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, _rotationFactorPerFrame * Time.deltaTime);
        }
    }

    private void HandleJump()
    {
        if (!IsJumping && _characterController.isGrounded && IsJumpPressed
       )
        {
            if (_jumpCount < 3 && _currentJumpResetRoutine != null)
            {
                StopCoroutine(_currentJumpResetRoutine);
            }
            IsJumping = true;
            _jumpCount++;
            _movement.y = _initialJumpVelocities[_jumpCount];
          //  _appliedMovement.y = _initialJumpVelocities[_jumpCount];
        }
        else if (IsJumping && _characterController.isGrounded && !IsJumpPressed
            )
        {
            _jumpCountdown = true;
            IsJumping = false;
        }
    }

    IEnumerator JumpResetRoutine()
    {
        yield return new WaitForSeconds(.5f);
        _jumpCount = 0;
    }

    public void OnMove(InputAction.CallbackContext obj)
    {
        MovementInput = obj.ReadValue<Vector2>();
        _movement.x = MovementInput.x;
        _movement.z = MovementInput.y;
        isMovementPressed = MovementInput.x != 0 || MovementInput.y != 0;   
    }

    public void OnJump(InputAction.CallbackContext obj)
    {
        IsJumpPressed = obj.ReadValueAsButton();

        if (CanWallJump)
        {
            CanWallJump = false;
            _movementVelocity = _wallJumpForce;
            Invoke(nameof(ResetWallJump), _maxJumpTime / 2);
        }
    }
    private void ResetWallJump()
    {
        _movementVelocity = Vector3.zero;
    }

    public void OnDash(InputAction.CallbackContext obj)
    {

        if (_dashCdTimer > 0)
        {
            return;
        }
        else
        {
            _dashCdTimer = DashCd;

            _currentSpeed = DashSpeed;
            Vector3 forceToApply = _characterController.transform.forward * DashForce + transform.up * DashUpwardForce;

            _movementVelocity = forceToApply;

            Invoke(nameof(ResetDash), DashDuration);

        }

    }
    private void ResetDash()
    {
        _currentSpeed = WalkSpeed;
        _movementVelocity = Vector3.zero;
    }



    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (!_characterController.isGrounded && hit.normal.y < 0.1f)
        {
            CanWallJump = true;
            _wallJumpForce = hit.normal * WalkSpeed + transform.up * _initialJumpVelocities[1];
        }
        else
        {
            CanWallJump = false;
        }
    }

    private Vector3 ConvertToCameraSpace(Vector3 vectorToRotate)
    {
        float currentYValue = vectorToRotate.y;

        Vector3 cameraForeward = Camera.main.transform.forward;
        Vector3 cameraRight = Camera.main.transform.right;

        cameraForeward.y = 0;
        cameraRight.y = 0;

        cameraForeward = cameraForeward.normalized;
        cameraRight = cameraRight.normalized;

        Vector3 cameraForewardZProduct = vectorToRotate.z * cameraForeward;
        Vector3 cameraRightXProduct = vectorToRotate.x * cameraRight;

        Vector3 vectorRotatedToCameraSpace = cameraForewardZProduct + cameraRightXProduct;
        vectorRotatedToCameraSpace.y = currentYValue;
        return vectorRotatedToCameraSpace;
    }

    private void Update()
    {

        HandleRotation();

        if (_dashCdTimer > 0)
        {
            _dashCdTimer -= Time.deltaTime;
        }

        if (_flipCdTimer > 0)
        {
            _flipCdTimer -= Time.deltaTime;
        }

        _previousMovement = MovementInput;

        if (_previousMovement == -MovementInput)
        {
            if (_flipCdTimer > 0)
            {
                return;
            }
            else
            {
                _flipCdTimer = FlipCd;
                Debug.Log("Can Sideflip test confirmed");
                _jumpCount = 2;
            }
            
        }

        //   _movement = transform.right * MovementInput.x + transform.forward * MovementInput.y;

        _appliedMovement.x = _movement.x * WalkSpeed;
        _appliedMovement.z = _movement.z * WalkSpeed;

        _cameraRelativeMovement = ConvertToCameraSpace(_appliedMovement);

        _characterController.Move(((_cameraRelativeMovement) + _movementVelocity) * Time.deltaTime);

        HandleGravity();
        HandleJump();
    }
}
