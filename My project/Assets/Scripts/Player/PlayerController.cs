using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
//[RequireComponent(typeof(PlayerCamera))]
//[RequireComponent(typeof(PlayerAttack))]
//[RequireComponent(typeof(PlayerInteraction))]
//[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(PlayerStateMachine))]
public class PlayerController : MonoBehaviour
{
    private PlayerInput playerInput;
    //private PlayerMovement playerMovement;
    private PlayerStateMachine playerStateMachine;

  //  private PlayerInteraction playerInteraction;
  //  private PlayerAttack playerAttack;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
       // playerMovement = GetComponent<PlayerMovement>();
        playerStateMachine = GetComponent<PlayerStateMachine>();
      //  playerInteraction = GetComponent<PlayerInteraction>();
     //   PlayerCamera = GetComponent<PlayerCamera>();
      //  playerAttack = GetComponent<PlayerAttack>();
    }

    private void OnEnable()
    {
        BindInputs();
    }

    private void OnDisable()
    {
        UnBindInputs();
    }

    protected virtual void BindInputs()
    {
        playerInput.actions["OpenMenu"].performed += OnOpenMenu;
        playerInput.actions["CloseMenu"].performed += OnCloseMenu;

        playerInput.actions["Move"].performed += playerStateMachine.OnMove;
        playerInput.actions["Move"].canceled += playerStateMachine.OnMove;
        playerInput.actions["Jump"].started += playerStateMachine.OnJump;
        playerInput.actions["Jump"].canceled += playerStateMachine.OnJump;
        playerInput.actions["Crouch"].started += playerStateMachine.OnCrouch;
        playerInput.actions["Crouch"].canceled += playerStateMachine.OnCrouch;
        //playerInput.actions["Sprint"].performed += playerMovement.OnSprint;
        playerInput.actions["Dash"].performed += playerStateMachine.OnDash;

        //   playerInput.actions["Look"].performed += PlayerCamera.OnLook;
        // playerInput.actions["Interact"].performed += playerInteraction.OnInteract;
        // playerInput.actions["Attack"].performed += playerAttack.OnAttack;
    }

    protected virtual void UnBindInputs()
    {
        playerInput.actions["OpenMenu"].performed -= OnOpenMenu;
        playerInput.actions["CloseMenu"].performed -= OnCloseMenu;

        playerInput.actions["Move"].performed -= playerStateMachine.OnMove;
        playerInput.actions["Move"].canceled -= playerStateMachine.OnMove;
        playerInput.actions["Jump"].started -= playerStateMachine.OnJump;
        playerInput.actions["Jump"].canceled -= playerStateMachine.OnJump;
        playerInput.actions["Crouch"].started -= playerStateMachine.OnCrouch;
        playerInput.actions["Crouch"].canceled -= playerStateMachine.OnCrouch;
        //playerInput.actions["Sprint"].performed -= playerMovement.OnSprint;
        playerInput.actions["Dash"].performed -= playerStateMachine.OnDash;

       // playerInput.actions["Look"].performed -= PlayerCamera.OnLook;
     //   playerInput.actions["Interact"].performed -= playerInteraction.OnInteract;
      //  playerInput.actions["Attack"].performed -= playerAttack.OnAttack;
    }

    private void OnCloseMenu(InputAction.CallbackContext obj)
    {
        GameManager.Instance.HideInventory();
        playerInput.SwitchCurrentActionMap("Game");
    }

    private void OnOpenMenu(InputAction.CallbackContext obj)
    {
        GameManager.Instance.ShowInventory();
        playerInput.SwitchCurrentActionMap("Menu");
    }
}
