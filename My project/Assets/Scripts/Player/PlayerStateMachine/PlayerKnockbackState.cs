using System.Collections;
using UnityEngine;

public class PlayerKnockbackState : PlayerBaseState
{
    public PlayerKnockbackState(PlayerStateMachine ctx, PlayerStateFactory factory) : base(ctx, factory)
    {
        IsRootState = true;
    }
    IEnumerator FlickerRoutine()
    {
        while (Ctx.Invunerable)
        {
            Ctx.MeshRenderer.enabled = false;
            yield return new WaitForSeconds(.1f);
            Ctx.MeshRenderer.enabled = true;
            yield return new WaitForSeconds(.1f);
        }
    }

    IEnumerator InvunerableRoutine()
    {
        yield return new WaitForSeconds(3f);
        Ctx.Invunerable = false;
    }

    public override void CheckSwitchState()
    {
       if (Ctx.CharacterController.isGrounded && !Ctx.IsHurt)
        {
            SwitchState(Factory.Grounded());
        }
        else if (Ctx.ChangingScenes)
        {
            SwitchState(Factory.ChangeScene());
        }
    }

    public override void EnterState()
    {
        Ctx.Invunerable = true;
        Ctx.FlickerRoutine = Ctx.StartCoroutine(FlickerRoutine());
        Ctx.InvunerableRoutine = Ctx.StartCoroutine(InvunerableRoutine());
        InitializeSubState();
        Ctx.FreezeMovement = true;
        HandleKnockback();
    }

    public override void ExitState()
    {
        Ctx.FreezeMovement = false;
        Ctx.MovementVelocity = Vector3.zero;
        Ctx.HurtDirection = Vector3.zero;
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
    private void HandleKnockback()
    {
        Ctx.CurrentMovementY = 5f;
        Ctx.AppliedMovementY = 5f;

        Ctx.MovementVelocity = Ctx.HurtDirection * 15f;
    }
    private void HandleGravity()
    {
        bool isFalling = Ctx.CurrentMovementY <= 0.0f;
        float fallMultiplier = 2.0f;

        if (isFalling)
        {
            Ctx.IsHurt = false;
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
