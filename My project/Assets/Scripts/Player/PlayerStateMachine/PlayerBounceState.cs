using UnityEngine;

public class PlayerBounceState : PlayerBaseState
{
    public PlayerBounceState(PlayerStateMachine ctx, PlayerStateFactory factory) : base(ctx, factory)
    {
        IsRootState = true;
    }

    public override void CheckSwitchState()
    {
        if (Ctx.CharacterController.isGrounded)
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
        else if (Ctx.ChangingScenes)
        {
            SwitchState(Factory.ChangeScene());
        }
    }

    public override void EnterState()
    {
        InitializeSubState();
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
        bool isFalling = Ctx.CurrentMovementY <= 0.0f || !Ctx.IsJumpPressed;
        float fallMultiplier = 2.0f;

        if (isFalling)
        {
            float previousYVelocity = Ctx.CurrentMovementY;
            Ctx.CurrentMovementY = Ctx.CurrentMovementY + (Ctx.JumpGravities[Ctx.JumpCount] * fallMultiplier * Time.deltaTime);
            Ctx.AppliedMovementY = Mathf.Max((previousYVelocity + Ctx.CurrentMovementY) * .5f, -20.0f);
        }
        else
        {
            float previousYVelocity = Ctx.CurrentMovementY;
            Ctx.CurrentMovementY = Ctx.CurrentMovementY + (Ctx.JumpGravities[Ctx.JumpCount] * Time.deltaTime);
            Ctx.AppliedMovementY = (previousYVelocity + Ctx.CurrentMovementY) * .5f;
        }
    }
}
