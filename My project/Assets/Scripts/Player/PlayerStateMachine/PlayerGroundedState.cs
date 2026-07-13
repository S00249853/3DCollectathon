using System.Collections;
using UnityEngine;

public class PlayerGroundedState : PlayerBaseState
{
    bool _notCoyote;
    public PlayerGroundedState(PlayerStateMachine ctx, PlayerStateFactory factory) : base(ctx, factory)
    {
        IsRootState = true;
        _notCoyote = true;
    }

    IEnumerator CoyoteRoutine()
    {
        Ctx.CurrentMovementY = 0;
        Ctx.AppliedMovementY = 0;
        yield return new WaitForSeconds(.15f);
        Debug.Log("Test for error");
        SwitchState(Factory.Fall());
    }

    public override void EnterState() {
        InitializeSubState();
        Ctx.CurrentMovementY = Ctx.Gravity;
        Ctx.AppliedMovementY = Ctx.Gravity;
        Ctx.CanDash = true;
        Debug.Log("Grounded State Entered");
    }

    public override void UpdateState()
    {
        CheckSwitchState();
    }

    public override void ExitState() 
    {
        if (Ctx.CoyoteRoutine != null)
        {
            Ctx.StopCoroutine(Ctx.CoyoteRoutine);
        }
        Ctx.CurrentMovementY = Ctx.Gravity;
        Ctx.AppliedMovementY = Ctx.Gravity;
        _notCoyote = true;
    }

    public override void CheckSwitchState() { 
    if (Ctx.JumpBufferTimer > 0)
        {
            SwitchState(Factory.Jump());
        }
    else if (!Ctx.CharacterController.isGrounded && _notCoyote)
        {
            _notCoyote = false;
            Ctx.CoyoteRoutine = Ctx.StartCoroutine(CoyoteRoutine());
        }
    else if (Ctx.IsDead)
        {
            SwitchState(Factory.Grounded());
        }
        else if (Ctx.IsDashing)
        {
            SwitchState(Factory.Dash());
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

    public override void InitializeSubState() { 
    //if (Ctx.IsMovementPressed)
    //    {
            SetSubState(Factory.Move());
        //}
    }
}
