using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerStateMachine : MonoBehaviour
{

    //Character Controller
    [SerializeField] private CharacterController _characterController;

    //Movement Variables
    public Vector3 Velocity { get { return _characterController.velocity; } }
    private Vector2 _movementInput;
    private Vector2 _previousMovement;
    private Vector3 _movementVelocity;
    private Vector3 _movement;
    private Vector3 _appliedMovement;
    private float _currentSpeed;
    private bool _isMovementPressed;
    Vector3 _cameraRelativeMovement;

    //Editable Variables
    [Header("Settings")]
    [SerializeField] private float walkSpeed;
    [SerializeField] private float DashSpeed;
    [SerializeField] private float DashForce;
    [SerializeField] private float DashUpwardForce;
    [SerializeField] private float DashDuration;
    [SerializeField] private float DashCd;
    [SerializeField] private float FlipCd;

    //Gravity Variables
    private float _gravity = -9.81f;

    //Jumping Variables
    private bool _isJumping;
    private bool _isJumpPressed;
    private float _initialJumpVelocity;
    private float _maxJumpHeight = 2f;
    private float _maxJumpTime = 1f;
    private int _jumpCount = 0;
    Dictionary<int, float> _initialJumpVelocities = new Dictionary<int, float>();
    Dictionary<int, float> _jumpGravities = new Dictionary<int, float>();
    Coroutine _currentJumpResetRoutine = null;
    bool _requireNewJumpPress;

    //Miscellanious Variables
    public bool CanSideflip;
    public bool CanWallJump;
    private float _dashCdTimer;
    float _rotationFactorPerFrame = 15.0f;
    private Vector3 _wallJumpForce;
    private float _flipCdTimer;

    //State Variables
    PlayerBaseState _currentState;
    PlayerStateFactory _states;

    //Getters and Setters
    public CharacterController CharacterController { get { return _characterController; } }
    public PlayerBaseState CurrentState { get { return _currentState; } set { _currentState = value; } }
    public Coroutine CurrentJumpResetRoutine { get {  return _currentJumpResetRoutine; } set { _currentJumpResetRoutine = value; } }
   public Dictionary<int, float> InitialJumpVelocities { get { return _initialJumpVelocities; } }
    public Dictionary<int, float> JumpGravities { get { return _jumpGravities; } }
    public int JumpCount {  get { return _jumpCount; } set { _jumpCount = value; } }
    public bool RequireNewJumpPress { get { return _requireNewJumpPress; } set { _requireNewJumpPress = value; } }
    public bool IsJumping { set { _isJumping = value; } }
    public bool IsJumpPressed { get { return _isJumpPressed; } }
    public bool IsMovementPressed {  get { return _isMovementPressed; } }
    public float CurrentMovementY { get { return _movement.y; } set { _movement.y = value; } }
    public float AppliedMovementY { get { return _appliedMovement.y; } set { _appliedMovement.y = value; } }
    public float AppliedMovementX { get { return _appliedMovement.x; } set { _appliedMovement.x = value; } }
    public float AppliedMovementZ { get { return _appliedMovement.z; } set { _appliedMovement.z = value; } }
    public float Gravity { get { return _gravity; } }
    public float WalkSpeed { get { return walkSpeed; } }
    public Vector2 MovementInput { get { return _movementInput; } }

    private void Awake()
    {
        _currentSpeed = walkSpeed;

        _states = new PlayerStateFactory(this);

        _currentState = _states.Grounded();
        _currentState.EnterState();

        SetupJumpVariables();
    }

    private void Start()
    {
        _characterController.Move(_appliedMovement * Time.deltaTime);
    }

    private void SetupJumpVariables()
    {
        float timeToApex = _maxJumpTime / 2;
        float  InitialGravity = (-2 * _maxJumpHeight) / Mathf.Pow(timeToApex, 2);
        _initialJumpVelocity = (2 * _maxJumpHeight) / timeToApex;
        float secondGravity = (-2 * (_maxJumpHeight + 2)) / Mathf.Pow(timeToApex * 1.25f, 2);
        float secondJumpVelocity = (2 * (_maxJumpHeight + 2)) / (timeToApex * 1.25f);
        float thirdGravity = (-2 * (_maxJumpHeight + 4)) / Mathf.Pow(timeToApex * 1.5f, 2);
        float thirdJumpVelocity = (2 * (_maxJumpHeight + 4)) / (timeToApex * 1.5f);

        _initialJumpVelocities.Add(1, _initialJumpVelocity);
        _initialJumpVelocities.Add(2, secondJumpVelocity);
        _initialJumpVelocities.Add(3, thirdJumpVelocity);

        _jumpGravities.Add(0, InitialGravity);
        _jumpGravities.Add(1, InitialGravity);
        _jumpGravities.Add(2, secondGravity);
        _jumpGravities.Add(3, thirdGravity);
    }

    public void OnMove(InputAction.CallbackContext obj)
    {
        _movementInput = obj.ReadValue<Vector2>();
        _movement.x = _movementInput.x;
        _movement.z = _movementInput.y;
        _isMovementPressed = _movementInput.x != 0 || _movementInput.y != 0;
    }

    public void OnJump(InputAction.CallbackContext obj)
    {
        _isJumpPressed = obj.ReadValueAsButton();
        _requireNewJumpPress = false;

        //if (CanWallJump)
        //{
        //    CanWallJump = false;
        //    _movementVelocity = _wallJumpForce;
        //    Invoke(nameof(ResetWallJump), _maxJumpTime / 2);
        //}
    }

    //private void ResetWallJump()
    //{
    //    _movementVelocity = Vector3.zero;
    //}

    public void OnDash(InputAction.CallbackContext obj)
    {

        //    if (_dashCdTimer > 0)
        //    {
        //        return;
        //    }
        //    else
        //    {
        //        _dashCdTimer = DashCd;

        //        _currentSpeed = DashSpeed;
        //        Vector3 forceToApply = _characterController.transform.forward * DashForce + transform.up * DashUpwardForce;

        //        _movementVelocity = forceToApply;

        //        Invoke(nameof(ResetDash), DashDuration);

    }


//}
//private void ResetDash()
//{
//    _currentSpeed = walkSpeed;
//    _movementVelocity = Vector3.zero;
//}

private void HandleRotation()
    {
        Vector3 positionToLookAt;

        positionToLookAt.x = _cameraRelativeMovement.x;
        positionToLookAt.y = 0;
        positionToLookAt.z = _cameraRelativeMovement.z;

        Quaternion currentRotation = transform.rotation;

        if (_isMovementPressed)
        {
            Quaternion targetRotation = Quaternion.LookRotation(positionToLookAt);

            transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, _rotationFactorPerFrame * Time.deltaTime);
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
        _currentState.UpdateStates();

        _cameraRelativeMovement = ConvertToCameraSpace(_appliedMovement);
        _characterController.Move(((_cameraRelativeMovement) + _movementVelocity) * Time.deltaTime);
    }

}
