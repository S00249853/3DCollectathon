using UnityEngine;

public class PlayerWallJumpState : PlayerBaseState
{
    public PlayerWallJumpState(PlayerStateMachine ctx, PlayerStateFactory factory) : base(ctx, factory)
    {
        IsRootState = true;
    }

    public override void CheckSwitchState()
    {
        if (Ctx.CharacterController.isGrounded || Ctx.IsDead)
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
        else if (Ctx.IsHurt)
        {
            SwitchState(Factory.Knockback());
        }
        else if (Ctx.IsBounce)
        {
            SwitchState(Factory.Bounce());
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
        HandleWallJump();
        Ctx.CanDash = true;
        Ctx.WallJump = false;
        Ctx.WallJumpBuffer = true;
        Ctx.FreezeMovement = true;
    }

    public override void ExitState()
    {
        Ctx.WallJumpBuffer = false;
        Ctx.FreezeMovement = false;
        Ctx.MovementVelocity = Vector3.zero;
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

    private void HandleWallJump()
    {
       Vector3 wallJumpForce = Ctx.Wall.normal * Ctx.WalkSpeed;

        Ctx.CurrentMovementY = Ctx.InitialJumpVelocities[1] / .9f;
        Ctx.AppliedMovementY = Ctx.InitialJumpVelocities[1] / .9f;
     
        Ctx.MovementVelocity = wallJumpForce;
    }
    private void HandleGravity()
    {
        bool isFalling = Ctx.CurrentMovementY <= 0.0f;
        float fallMultiplier = 2.0f;

        if (isFalling)
        {
            Ctx.WallJumpBuffer = false;
            Ctx.FreezeMovement = false;
            Ctx.MovementVelocity = Vector3.zero;
            float previousYVelocity = Ctx.CurrentMovementY;
            Ctx.CurrentMovementY = Ctx.CurrentMovementY + (Ctx.JumpGravities[1] * fallMultiplier * Time.deltaTime);
            Ctx.AppliedMovementY = Mathf.Max((previousYVelocity + Ctx.CurrentMovementY) * .5f, -20.0f);
        }
        else
        {
            float previousYVelocity = Ctx.CurrentMovementY;
            Ctx.CurrentMovementY = Ctx.CurrentMovementY + (Ctx.JumpGravities[1] * Time.deltaTime);
            Ctx.AppliedMovementY = (previousYVelocity + Ctx.CurrentMovementY) * .5f;
        }
    }
}
