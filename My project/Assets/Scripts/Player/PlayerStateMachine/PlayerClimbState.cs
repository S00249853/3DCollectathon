using System.Collections;
using UnityEngine;

public class PlayerClimbState : PlayerBaseState
{
    IEnumerator ClimbDelay()
    {
        yield return new WaitForSeconds(.2f);
        Ctx.ClimbDelay = true;
    }

    IEnumerator CameraReset()
    {
        Ctx.ClimbCamera.enabled = false;
        Ctx.StopMoving = true;
        yield return new WaitForSeconds(.2f);
        Ctx.StopMoving = false;
    }
    public PlayerClimbState(PlayerStateMachine ctx, PlayerStateFactory factory) : base(ctx, factory)
    {
        IsRootState = true;
    }

    public override void CheckSwitchState()
    {
        if (Ctx.IsClimb && Ctx.CharacterController.isGrounded || !Ctx.IsClimb && Ctx.CharacterController.isGrounded || Ctx.IsDead)
        {
            SwitchState(Factory.Grounded());
        }
        else if (!Ctx.IsClimb && !Ctx.CharacterController.isGrounded)
        {
            SwitchState(Factory.Fall());
        }
        else if (Ctx.IsHurt)
        {
            SwitchState(Factory.Knockback());
        }
        else if (Ctx.StopMoving)
        {
            SwitchState(Factory.ChangeScene());
        }
    }

    public override void EnterState()
    {
        Debug.Log("Eing Climb State");
        InitializeSubState();
        Ctx.ClimbCamera.enabled = true;
        Ctx.ClimbRoutine = Ctx.StartCoroutine(ClimbDelay());
        // May need to change how thw player camera works here
    }

    public override void ExitState()
    {
        Debug.Log("Exiting Climb State");
        Ctx.IsClimb = false;
        Ctx.CameraResetRoutine = Ctx.StartCoroutine(CameraReset());
        if (Ctx.ClimbRoutine != null)
        {
            Ctx.StopCoroutine(Ctx.ClimbRoutine);
        }
        Ctx.ClimbDelay = false;  
    }

    public override void InitializeSubState()
    {
        SetSubState(Factory.Move());
    }

    public override void UpdateState()
    {
       if (Ctx.CharacterController.collisionFlags == 0)
        {
            Ctx.IsClimb = false;
        }
        CheckSwitchState();
    }
}
