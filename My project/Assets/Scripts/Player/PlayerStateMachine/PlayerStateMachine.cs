using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Cinemachine;
using UnityEngine.SceneManagement;

public class PlayerStateMachine : MonoBehaviour
{

    //Character Controller
    [SerializeField] private CharacterController _characterController;

    //Camera Variables
    [SerializeField] private CinemachineCamera _mainCamera;
    [SerializeField] private CinemachineCamera _climbCamera;

    //Movement Variables
    private Vector2 _movementInput;
    private Vector3 _appliedMovement;
    private Vector3 _cameraRelativeMovement;
    private Vector3 _movement;
    private Vector3 _movementVelocity; 
    private bool _climbDelay;
    private bool _freezeMovement;
    private bool _isMovementPressed;
    private float _currentSpeed;

    //Editable Variables
    [Header("Settings")]
    [SerializeField] private float _walkSpeed;
    [SerializeField] private float _dashSpeed;
    [SerializeField] private float _dashForce;
    [SerializeField] private float _dashUpwardForce;
    [SerializeField] private float _dashDuration;
    [SerializeField] private float _dashCd;
    [SerializeField] private float _flipCd;

    //Gravity Variables
    private float _gravity = -9.81f;

    //Jumping and Bouncing Variables
    private bool _isJumping;
    private bool _isJumpPressed;
    private bool _shouldJump;
    private bool _wallJump;
    private bool _wallJumpBuffer;
    private float _bounceAmount;
    private float _initialJumpVelocity;
    private float _jumpBufferTimer;
    private float _maxJumpHeight = 2f;
    private float _maxJumpTime = 1f;
    private int _jumpCount = 0;
    private Dictionary<int, float> _initialJumpVelocities = new Dictionary<int, float>();
    private Dictionary<int, float> _jumpGravities = new Dictionary<int, float>();
    private Coroutine _coyoteRoutine = null;
    private Coroutine _currentJumpResetRoutine = null;

    //Wall and Climbing Variables
    private bool _onWall;
    private bool _stopClimbing;
    private ControllerColliderHit _wall;
    private ControllerColliderHit _climbableWall;
    private Coroutine _cameraResetRoutine;
    private Coroutine _climbRoutine = null;
    private GameObject _currentWall;

    //Miscellanious Variables
    private float _rotationFactorPerFrame = 15.0f;

    //Dashing Variables
    [SerializeField] private float _dashCdTimer;
    [SerializeField] private bool _isDashing;
    private bool _canDash;

    //State Variables
    private PlayerBaseState _currentState;
    private PlayerStateFactory _states;

    //Scene Variables
    private bool _changingScenes;
    private string _spawnPoint;
    private Transform _checkpoint;

    //Damage Variables
    private bool _invunerable;
    private bool _knockbackInAffect;
    private Coroutine _flickerRoutine = null;
    private Coroutine _invunerableRoutine = null;
    private Coroutine _knockbackRoutine = null;
    private MeshRenderer _meshRenderer;
    private Vector3 _hurtDirection;

    //Miscellanious State Checks
    private bool _isBounce;
    private bool _isClimb;
    private bool _isDead;
    private bool _isGroundPounding;
    private bool _isHurt;

    //Getters and Setters
    public bool CanDash { get { return _canDash; } set { _canDash = value; } }
    public bool ClimbDelay { get { return _climbDelay; } set { _climbDelay = value; } }
    public bool FreezeMovement { get { return _freezeMovement; } set { _freezeMovement = value; } }
    public bool Invunerable { get { return _invunerable; } set { _invunerable = value; } }
    public bool IsBounce { get { return _isBounce; } set { _isBounce = value; } }
    public bool IsClimb { get { return _isClimb; } set { _isClimb = value; } }
    public bool IsDashing { get { return _isDashing; } set { _isDashing = value; } }
    public bool IsDead { get { return _isDead; } set { _isDead = value; } }
    public bool IsGroundPounding { get { return _isGroundPounding; } set { _isGroundPounding = value; } }
    public bool IsHurt { get { return _isHurt; } set { _isHurt = value; } }
    public bool IsJumping { set { _isJumping = value; } }
    public bool IsJumpPressed { get { return _isJumpPressed; } } 
    public bool IsMovementPressed {  get { return _isMovementPressed; } }
    public bool KnockbackInAffect { get { return _knockbackInAffect; } set { _knockbackInAffect = value; } }
    public int JumpCount { get { return _jumpCount; } set { _jumpCount = value; } }
    public bool OnWall { get { return _onWall; } set { _onWall = value; } }
    public bool ShouldJump { get { return _shouldJump; } set { _shouldJump = value; } }
    public bool StopClimbing { get { return _stopClimbing; } set { _stopClimbing = value; } }
    public bool StopMoving { get { return _changingScenes; } set { _changingScenes = value; } }
    public bool WallJump { get { return _wallJump; } set { _wallJump = value; } }
    public bool WallJumpBuffer { get { return _wallJumpBuffer; } set { _wallJumpBuffer = value; } }
    public float AppliedMovementY { get { return _appliedMovement.y; } set { _appliedMovement.y = value; } }
    public float AppliedMovementX { get { return _appliedMovement.x; } set { _appliedMovement.x = value; } }
    public float AppliedMovementZ { get { return _appliedMovement.z; } set { _appliedMovement.z = value; } }
    public float BounceAmount { get { return _bounceAmount; } set { _bounceAmount = value; } }
    public float CurrentMovementY { get { return _movement.y; } set { _movement.y = value; } }
    public float CurrentMovementX { get { return _movement.x; } set { _movement.x = value; } }
    public float CurrentMovementZ { get { return _movement.z; } set { _movement.z = value; } }
    public float DashCd { get { return _dashCd; } }
    public float DashCdTimer { get { return _dashCdTimer; } set { _dashCdTimer = value; } }
    public float DashDuration { get { return _dashDuration; } }
    public float DashForce { get { return _dashForce; } }
    public float DashUpwardForce { get { return _dashUpwardForce; } }
    public float Gravity { get { return _gravity; } }
    public float JumpBufferTimer { get { return _jumpBufferTimer; } set { _jumpBufferTimer = value; } }
    public float MaxJumpTime { get { return _maxJumpTime; } }
    public float WalkSpeed { get { return _walkSpeed; } }
    public string SpawnPoint { get { return _spawnPoint; } set { _spawnPoint = value; } }
    public CharacterController CharacterController { get { return _characterController; } }
    public CinemachineCamera ClimbCamera { get { return _climbCamera; } set { _climbCamera = value; } }
    public CinemachineCamera MainCamera { get { return _mainCamera; } set { _mainCamera = value; } }
    public ControllerColliderHit Wall { get { return _wall; } }
    public Coroutine CameraResetRoutine {  get { return _cameraResetRoutine; } set { _cameraResetRoutine = value; } }
    public Coroutine ClimbRoutine { get { return _climbRoutine; } set { _climbRoutine = value; } }
    public Coroutine CoyoteRoutine { get { return _coyoteRoutine; } set { _coyoteRoutine = value; } }
    public Coroutine CurrentJumpResetRoutine { get { return _currentJumpResetRoutine; } set { _currentJumpResetRoutine = value; } }
    public Coroutine FlickerRoutine { get { return _flickerRoutine; } set { _flickerRoutine = value; } }
    public Coroutine InvunerableRoutine { get { return _invunerableRoutine; } set { _invunerableRoutine = value; } }
    public Coroutine KnockbackRoutine { get { return _knockbackRoutine; } set { _knockbackRoutine = value; } }
    public Dictionary<int, float> InitialJumpVelocities { get { return _initialJumpVelocities; } }
    public Dictionary<int, float> JumpGravities { get { return _jumpGravities; } }
    public GameObject CurrentWall { get { return _currentWall; } }
    public MeshRenderer MeshRenderer { get { return _meshRenderer; } set { _meshRenderer = value; } }
    public PlayerBaseState CurrentState { get { return _currentState; } set { _currentState = value; } }
    public Transform Checkpoint { get { return _checkpoint; } set { _checkpoint = value; } }
    public Vector3 AppliedMovement { get { return _cameraRelativeMovement; } set { _cameraRelativeMovement = value; } }
    public Vector3 HurtDirection { get { return _hurtDirection; } set { _hurtDirection = value; } }
    public Vector3 MovementVelocity { get { return _movementVelocity; } set { _movementVelocity = value; } }
    public Vector3 Velocity { get { return _characterController.velocity; } }
    public Vector2 MovementInput { get { return _movementInput; } }


    private void Awake()
    {
        _climbCamera.enabled = false;
        _meshRenderer = GetComponent<MeshRenderer>();
        _currentSpeed = _walkSpeed;

        _states = new PlayerStateFactory(this);

        _currentState = _states.Grounded();
        _currentState.EnterState();

        SetupJumpVariables();
    }

    private void Start()
    {
        _characterController.Move(_appliedMovement * Time.deltaTime);
        _checkpoint = transform;
        Debug.Log($"Checkpoint is {_checkpoint.position}");
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
        if (!_characterController.isGrounded)
        {
            _isGroundPounding = true;
        }
    }

    public void OnDash(InputAction.CallbackContext obj)
    {
        if (_dashCdTimer <= 0 && _canDash)
        {
            IsDashing = true;
        }
    }

    public void OnHurt(Vector3 hurtDirection, int damage)
    {
        if (!_invunerable)
        {
            _hurtDirection = hurtDirection;
            GameManager.Instance.Health -= damage;
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
        if (!_isDashing)
        {
            Vector3 positionToLookAt;

            positionToLookAt.x = _cameraRelativeMovement.x;
            positionToLookAt.y = 0;
            positionToLookAt.z = _cameraRelativeMovement.z;

            Quaternion currentRotation = transform.rotation;

            if (_onWall)
            {
                Vector3 wallRotation = new Vector3(_wall.normal.x, 0, _wall.normal.z);

                Quaternion targetRotation = Quaternion.LookRotation(wallRotation);

                transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, 20f * Time.deltaTime);
               
            }

            else if (_wallJumpBuffer)
            {
                Vector3 wallRotation = new Vector3(_wall.normal.x, 0, _wall.normal.z);

                Quaternion targetRotation = Quaternion.LookRotation(wallRotation);

                transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, 20f * Time.deltaTime);
           
            }

            else if (_isClimb)
            {
                Vector3 climbRotation = new Vector3(_climbableWall.normal.x, 0, _climbableWall.normal.z);

                Quaternion targetRotation = Quaternion.LookRotation(-climbRotation);

                transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, 20f * Time.deltaTime);
            }

            else if (_isMovementPressed)
            {
                Quaternion targetRotation = Quaternion.LookRotation(positionToLookAt);

                transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, _rotationFactorPerFrame * Time.deltaTime);
            }
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
        if (!_characterController.isGrounded && hit.normal.y < 0.1f && !_isDashing && CharacterController.collisionFlags == CollisionFlags.Sides && hit.gameObject.tag != "NonStick" && hit.gameObject.tag != "Cannonball" && hit.gameObject.tag != "Climbable")
        {
            _wall = hit;
            _currentWall = hit.gameObject;
            _onWall = true;
        }

        if (hit.gameObject.tag == "Climbable" && hit.normal.y < 0.1f && !_stopClimbing)
        {
            _climbableWall = hit;
            _isClimb = true;
        }

        if (_stopClimbing || hit.gameObject.tag == "Climbable" && hit.normal.y > 0.1f || hit.gameObject.tag != "Climbable" && !CharacterController.isGrounded)
        {
            _isClimb = false;
           
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
                Spring spring = hit.gameObject.GetComponent<Spring>();
                OnBounce(spring.Bounciness.BounceAmount);
            }

            if (IsGroundPounding && hit.gameObject.tag == "Cracked")
            {
                Destroy(hit.gameObject);
            }
        }

        if ( hit.normal.y > 0.5f && hit.gameObject.tag == "Cannonball")
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
         
        if (!StopMoving)
        {
            _cameraRelativeMovement = ConvertToCameraSpace(_appliedMovement);
            _characterController.Move((_cameraRelativeMovement + _movementVelocity) * Time.deltaTime);
        }
            HandleTimers();
    }
}
