using UnityEngine;

public class PlayerDashState : PlayerBaseState
{

    Vector3 _startVelocity;

    public PlayerDashState(PlayerStateMachine ctx, PlayerStateFactory factory) : base(ctx, factory)
    {
        IsRootState = true;
    }

    public override void CheckSwitchState()
    {
       if (!Ctx.IsDashing && Ctx.CharacterController.isGrounded)
        {
           SwitchState(Factory.Grounded());
        }
       else if (!Ctx.IsDashing && !Ctx.CharacterController.isGrounded)
        {
            SwitchState(Factory.Fall());
        }
    }

    public override void EnterState()
    {
        InitializeSubState();
        Debug.Log("Dashing");
        HandleDash();
    }

    public override void ExitState()
    {
        Ctx.MovementVelocity = _startVelocity;
    }

    public override void InitializeSubState()
    {
        SetSubState(Factory.Still());
    }

    public override void UpdateState()
    {
        Ctx.AppliedMovementY = 0;
        CheckSwitchState();
    }

    private void HandleDash()
    {
   
            Ctx.DashCdTimer = Ctx.DashCd;

            _startVelocity = Ctx.MovementVelocity;
            //   _currentSpeed = DashSpeed;
            Vector3 forceToApply = Ctx.CharacterController.transform.forward * Ctx.DashForce + Ctx.transform.up * Ctx.DashUpwardForce;
             
            Ctx.MovementVelocity = forceToApply;

            Ctx.Invoke(nameof(Ctx.ResetDash), Ctx.DashDuration);
            Debug.Log("Dash should be over");
        
    }
}
