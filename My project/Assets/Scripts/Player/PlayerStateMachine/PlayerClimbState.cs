using System.Collections;
using UnityEngine;

public class PlayerClimbState : PlayerBaseState
{
    IEnumerator ClimbDelay()
    {
        yield return new WaitForSeconds(.2f);
        Ctx.ClimbDelay = true;
    }
    public PlayerClimbState(PlayerStateMachine ctx, PlayerStateFactory factory) : base(ctx, factory)
    {
        IsRootState = true;
    }

    public override void CheckSwitchState()
    {
        if (!Ctx.IsClimb && Ctx.CharacterController.isGrounded)
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
        else if (Ctx.ChangingScenes)
        {
            SwitchState(Factory.ChangeScene());
        }
    }

    public override void EnterState()
    {
       InitializeSubState();
        Ctx.MainCamera.enabled = false;
        Ctx.ClimbRoutine = Ctx.StartCoroutine(ClimbDelay());
        // May need to change how thw player camera works here
    }

    public override void ExitState()
    {
        Ctx.MainCamera.enabled = true;
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
        CheckSwitchState();
    }
}
