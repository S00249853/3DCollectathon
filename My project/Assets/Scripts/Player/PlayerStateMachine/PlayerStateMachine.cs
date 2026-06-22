using System;
using System.Collections;
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
    private bool _freezeMovement;
    Vector3 _cameraRelativeMovement;

    //Editable Variables
    [Header("Settings")]
    [SerializeField] private float walkSpeed;
    [SerializeField] private float DashSpeed;
    [SerializeField] private float dashForce;
    [SerializeField] private float dashUpwardForce;
    [SerializeField] private float dashDuration;
    [SerializeField] private float dashCd;
    [SerializeField] private float FlipCd;

    //Gravity Variables
    private float _gravity = -9.81f;

    //Jumping Variables
    private bool _isJumping;
    private bool _isJumpPressed;
    private bool _shouldJump;
    private bool _wallJump;
    private float _initialJumpVelocity;
    private float _maxJumpHeight = 2f;
    private float _maxJumpTime = 1f;
    private float _jumpBufferTimer;
    private int _jumpCount = 0;
    Dictionary<int, float> _initialJumpVelocities = new Dictionary<int, float>();
    Dictionary<int, float> _jumpGravities = new Dictionary<int, float>();
    Coroutine _currentJumpResetRoutine = null;
    Coroutine _jumpBufferRoutine = null;

    //Miscellanious Variables
    public bool CanSideflip;
    public bool _onWall;
    [SerializeField]private float _dashCdTimer;
    float _rotationFactorPerFrame = 15.0f;
    private Vector3 _wallJumpForce;
    private float _flipCdTimer;
    [SerializeField]private bool _isDashing;
    [SerializeField]private bool _isCrouching;
    [SerializeField] private bool _isGroundPounding;
    private ControllerColliderHit _wall;
    private Vector3 _hurtDirection;
    private bool _isHurt;
    private float _bounceAmount;
    private bool _isBounce;
    Coroutine _flickerRoutine = null;
    Coroutine _invunerableRoutine = null;
    Coroutine _coyoteRoutine = null;
    MeshRenderer _meshRenderer;
    private bool _invunerable;


    //State Variables
    PlayerBaseState _currentState;
    PlayerStateFactory _states;

    //Getters and Setters
    public CharacterController CharacterController { get { return _characterController; } }
    public PlayerBaseState CurrentState { get { return _currentState; } set { _currentState = value; } }
    public MeshRenderer MeshRenderer { get { return _meshRenderer; } set { _meshRenderer = value; } }
    public Coroutine CurrentJumpResetRoutine { get {  return _currentJumpResetRoutine; } set { _currentJumpResetRoutine = value; } }
    public Coroutine FlickerRoutine { get { return _flickerRoutine; } set { _flickerRoutine = value; } }
    public Coroutine InvunerableRoutine { get { return _invunerableRoutine; } set { _invunerableRoutine = value; } } 
    public Coroutine CoyoteRoutine { get { return _coyoteRoutine; } set { _coyoteRoutine = value; } }
   public Dictionary<int, float> InitialJumpVelocities { get { return _initialJumpVelocities; } }
    public Dictionary<int, float> JumpGravities { get { return _jumpGravities; } }
    public int JumpCount {  get { return _jumpCount; } set { _jumpCount = value; } }
    public bool IsJumping { set { _isJumping = value; } }
    public bool IsJumpPressed { get { return _isJumpPressed; } }
    public bool IsMovementPressed {  get { return _isMovementPressed; } }
    public bool IsCrouching { get {  return _isCrouching; } set { _isCrouching = value; } }
    public bool IsGroundPounding { get { return _isGroundPounding; } set { _isGroundPounding = value; } }
    public bool IsDashing { get { return _isDashing; } set  { _isDashing = value; } }
    public bool IsHurt { get { return _isHurt; } set { _isHurt = value; } }
    public bool IsBounce { get { return _isBounce; } set { _isBounce = value; } }
    public bool OnWall { get { return _onWall; } set { _onWall = value; } }
    public bool WallJump { get { return _wallJump; } set { _wallJump = value; } }
    public bool ShouldJump { get { return _shouldJump; } set { _shouldJump = value; } }
    public bool FreezeMovement { get { return _freezeMovement; } set { _freezeMovement = value; } }
    public bool Invunerable { get { return _invunerable; } set { _invunerable = value; } }
    public float CurrentMovementY { get { return _movement.y; } set { _movement.y = value; } }
    public float CurrentMovementX { get { return _movement.x; } set { _movement.x = value; } }
    public float CurrentMovementZ { get { return _movement.z; } set { _movement.z = value; } }
    public float AppliedMovementY { get { return _appliedMovement.y; } set { _appliedMovement.y = value; } }
    public float AppliedMovementX { get { return _appliedMovement.x; } set { _appliedMovement.x = value; } }
    public float AppliedMovementZ { get { return _appliedMovement.z; } set { _appliedMovement.z = value; } }
    public float Gravity { get { return _gravity; } }
    public float WalkSpeed { get { return walkSpeed; } }
    public float JumpBufferTimer { get { return _jumpBufferTimer; } set { _jumpBufferTimer = value; } } 
    public float DashCdTimer { get { return _dashCdTimer; } set { _dashCdTimer = value; } }
    public float DashCd { get { return dashCd; } }
    public float DashForce { get { return dashForce; } }
    public float DashUpwardForce {  get { return dashUpwardForce; } }
    public float DashDuration { get { return dashDuration; } }
    public float MaxJumpTime { get { return _maxJumpTime; } }
    public float BounceAmount { get { return _bounceAmount; } set { _bounceAmount = value; } }
    public Vector3 MovementVelocity { get { return _movementVelocity; } set { _movementVelocity = value; } }
    public Vector3 AppliedMovement { get { return _cameraRelativeMovement; } set { _cameraRelativeMovement = value; } }
    public Vector3 HurtDirection { get { return _hurtDirection; } set { _hurtDirection = value; } }
    public Vector2 MovementInput { get { return _movementInput; } }
    public ControllerColliderHit Wall { get { return _wall; } }

    private void Awake()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
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
        if (obj.started)
        {
            _jumpBufferTimer = .15f;
            if (_onWall)
            {
                _wallJump = true;
            }
        }
    }

    public void OnCrouch(InputAction.CallbackContext obj)
    {
        _isCrouching = obj.ReadValueAsButton();
        if (!_characterController.isGrounded)
        {
            _isGroundPounding = true;
        }
    }

    public void OnDash(InputAction.CallbackContext obj)
    {
        if (_dashCdTimer <= 0)
        {
            IsDashing = true;
        }
      
    }

    public void OnHurt(Vector3 hurtDirection)
    {
        if (!_invunerable)
        {
            _hurtDirection = hurtDirection;
            _isHurt = true;
        }
    }

    public void OnBounce(float bounceAmount)
    {
        _bounceAmount = bounceAmount;
        _isBounce = true;
    }

    public void ResetDash()
    {
        Debug.Log("Reset Dash called");
        IsDashing = false;
        Debug.Log("Reset Dash ended");
    }

    private void HandleRotation()
    {
        Vector3 positionToLookAt;

        positionToLookAt.x = _cameraRelativeMovement.x;
        positionToLookAt.y = 0;
        positionToLookAt.z = _cameraRelativeMovement.z;

        Quaternion currentRotation = transform.rotation;

        if (_onWall)
        {
            Quaternion targetRotation = Quaternion.LookRotation(_wall.normal);

            transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, 20f * Time.deltaTime);
        }

        else if (_isMovementPressed)
        {
            Quaternion targetRotation = Quaternion.LookRotation(positionToLookAt);

            transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, _rotationFactorPerFrame * Time.deltaTime);
        }
    }
    private void HandleTimers()
    {
        if (_dashCdTimer > 0)
        {
            _dashCdTimer -= Time.deltaTime;
        }
        if (_jumpBufferTimer > 0)
        {
            _jumpBufferTimer -= Time.deltaTime;
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

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (!_characterController.isGrounded && hit.normal.y < 0.1f && !_isDashing && hit.gameObject.tag != "NonStick")
        {
            _wall = hit;
            _onWall = true;
        }

        if (hit.normal.y > 0.8f && !_isHurt)
        {
            IStompable stomped = hit.gameObject.GetComponent<IStompable>();
            if (stomped != null)
            {
                stomped.Stomped();
               if (hit.gameObject.tag == "Enemy")
                {
                    OnBounce(10f);
                }
            }

            if (hit.gameObject.tag == "Spring")
            {
                OnBounce(30f);
            }
        }

        if (hit.normal.y > 0.6f && hit.gameObject.tag == "Cannonball")
        {
            OnBounce(10f);
            hit.gameObject.SetActive(false);
        }
    }
    private void Update()
    {
        HandleRotation();
        _currentState.UpdateStates();

        if (!IsMovementPressed)
        {
            JumpCount = 0;
        }

        _cameraRelativeMovement = ConvertToCameraSpace(_appliedMovement);
        _characterController.Move((_cameraRelativeMovement + _movementVelocity) * Time.deltaTime);
        HandleTimers();
    }
}
