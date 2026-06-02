using UnityEngine;

public class PlayerWallJumpState : PlayerBaseState
{
    public PlayerWallJumpState(PlayerStateMachine ctx, PlayerStateFactory factory) : base(ctx, factory)
    {
        IsRootState = true;
    }

    public override void CheckSwitchState()
    {
        if (Ctx.MovementVelocity == Vector3.zero)
        {
            SwitchState(Factory.Fall());
        }
        else if (Ctx.CharacterController.isGrounded)
        {
            SwitchState(Factory.Grounded());
        }
        else if (Ctx.IsGroundPounding)
        {
            SwitchState(Factory.GroundPound());
        }
        else if (Ctx.IsDashing)
        {
            SwitchState(Factory.Dash());
        }
        else if (Ctx.OnWall)
        {
            SwitchState(Factory.Wall());
        }
    }

    public override void EnterState()
    {
        InitializeSubState();
        Ctx.WallJump = false;
        HandleWallJump();
    }

    public override void ExitState()
    {

    }

    public override void InitializeSubState()
    {
        SetSubState(Factory.Still());
    }

    public override void UpdateState()
    {
        CheckSwitchState();
    }

    private void HandleWallJump()
    {
       Vector3 wallJumpForce = Ctx.Wall.normal * Ctx.WalkSpeed + Ctx.transform.up * Ctx.InitialJumpVelocities[1];
        Ctx.MovementVelocity = wallJumpForce;

        Ctx.Invoke(nameof(Ctx.ResetWallJump), Ctx.MaxJumpTime / 3);
    }
}
