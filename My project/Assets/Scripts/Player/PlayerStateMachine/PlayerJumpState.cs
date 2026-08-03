using System.Collections;
using UnityEngine;

public class PlayerJumpState : PlayerBaseState
{
    IEnumerator JumpResetRoutine()
    {
        yield return new WaitForSeconds(.1f);
        Ctx.JumpCount = 0;
    }
    public PlayerJumpState(PlayerStateMachine ctx, PlayerStateFactory factory) : base(ctx, factory)
    {
        IsRootState = true;  
    }

    public override void EnterState() {
        InitializeSubState();
        Ctx.JumpBufferTimer = 0;
        if (Ctx.JumpCount == 3)
        {
            Ctx.JumpCount = 0;
        }
        HandleJump();
        Debug.Log("Jump State Entered");
    }

public override void UpdateState() 
    {
        HandleGravity();
        CheckSwitchState();
    }

public override void ExitState() {
        Ctx.CurrentJumpResetRoutine = Ctx.StartCoroutine(JumpResetRoutine());
        if (Ctx.JumpCount == 3)
        {
            Ctx.JumpCount = 0;
        }
    }

public override void CheckSwitchState() { 
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

public override void InitializeSubState() 
    {
            SetSubState(Factory.Move());
    }

   private void HandleJump()
    {
        if (Ctx.JumpCount < 3 && Ctx.CurrentJumpResetRoutine != null)
        {
            Ctx.StopCoroutine(Ctx.CurrentJumpResetRoutine);
        }
        Ctx.IsJumping = true;
        Ctx.JumpCount++;
        Ctx.CurrentMovementY = Ctx.InitialJumpVelocities[Ctx.JumpCount];
        Ctx.AppliedMovementY = Ctx.InitialJumpVelocities[Ctx.JumpCount];
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
