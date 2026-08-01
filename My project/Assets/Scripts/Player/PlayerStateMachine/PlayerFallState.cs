using UnityEngine;

public class PlayerFallState : PlayerBaseState
{
    public PlayerFallState(PlayerStateMachine ctx, PlayerStateFactory factory) : base(ctx, factory)
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
        else if (!Ctx.CharacterController.isGrounded && Ctx.IsGroundPounding)
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
        Debug.Log("Falling");
        InitializeSubState();
    }

    public override void ExitState()
    {
      
    }

    public override void InitializeSubState()
    {
        //if (Ctx.IsMovementPressed)
        //{
        SetSubState(Factory.Move());
        //}
    }

    public override void UpdateState()
    {
        HandleGravity();
        CheckSwitchState();
    }

    void HandleGravity()
    {
        float fallMultiplier = 2.0f;
        float previousYVelocity = Ctx.CurrentMovementY;
        Ctx.CurrentMovementY = Ctx.CurrentMovementY + (Ctx.JumpGravities[Ctx.JumpCount] * fallMultiplier * Time.deltaTime);
        Ctx.AppliedMovementY = Mathf.Max((previousYVelocity + Ctx.CurrentMovementY) * .5f, -20.0f);
    }
}
