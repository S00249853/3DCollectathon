using UnityEngine;

public class PlayerBounceState : PlayerBaseState
{
    public PlayerBounceState(PlayerStateMachine ctx, PlayerStateFactory factory) : base(ctx, factory)
    {
        IsRootState = true;
    }

    public override void CheckSwitchState()
    {
        if (Ctx.CharacterController.isGrounded || Ctx.IsDead)
        {
            SwitchState(Factory.Grounded());
        }
        else if (Ctx.IsDashing)
        {
            SwitchState(Factory.Dash());
        }
        else if (Ctx.IsGroundPounding)
        {
            SwitchState(Factory.GroundPound());
        }
        else if (Ctx.OnWall)
        {
            SwitchState(Factory.Wall());
        }
        else if (Ctx.IsHurt)
        {
            SwitchState(Factory.Knockback());
        }
        else if (Ctx.IsClimb)
        {
            SwitchState(Factory.Climb());
        }
        else if (Ctx.StopMoving)
        {
            SwitchState(Factory.ChangeScene());
        }
    }

    public override void EnterState()
    {
        InitializeSubState();
        Ctx.CanDash = true;
        HandleBounce();
    }

    public override void ExitState()
    {
        Ctx.BounceAmount = 0;
        Ctx.IsBounce = false;
    }

    public override void InitializeSubState()
    {
        SetSubState(Factory.Move());
    }

    public override void UpdateState()
    {
        HandleGravity();
        CheckSwitchState();
    }

    void HandleBounce()
    {
        Ctx.CurrentMovementY = Ctx.BounceAmount;
        Ctx.AppliedMovementY = Ctx.BounceAmount;
    }
    private void HandleGravity()
    {
            float fallMultiplier = 2.0f;

            float previousYVelocity = Ctx.CurrentMovementY;
            Ctx.CurrentMovementY = Ctx.CurrentMovementY + (Ctx.JumpGravities[Ctx.JumpCount] * fallMultiplier * Time.deltaTime);
            Ctx.AppliedMovementY = Mathf.Max((previousYVelocity + Ctx.CurrentMovementY) * .5f, -20.0f);

    }
}
