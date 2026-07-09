using UnityEngine;

public class PlayerMoveState : PlayerBaseState
{
    public PlayerMoveState(PlayerStateMachine ctx, PlayerStateFactory factory) : base(ctx, factory)
    {
    }

    public override void EnterState() {
        Debug.Log("Move State Entered");
    }

    public override void UpdateState() {
        if (!Ctx.FreezeMovement && !Ctx.IsClimb || !Ctx.FreezeMovement && Ctx.IsClimb && Ctx.CharacterController.isGrounded && Ctx.ClimbDelay == true)
        {
            Ctx.AppliedMovementX = Ctx.MovementInput.x * Ctx.WalkSpeed;
            Ctx.AppliedMovementZ = Ctx.MovementInput.y * Ctx.WalkSpeed;
        }
        else if (!Ctx.FreezeMovement && Ctx.IsClimb)
        {
            Ctx.AppliedMovementY = Ctx.MovementInput.y * Ctx.WalkSpeed / 2;
            Ctx.AppliedMovementX = Ctx.MovementInput.x * Ctx.WalkSpeed / 2;

        }
            CheckSwitchState();
    }

    public override void ExitState() { }

    public override void CheckSwitchState() { }

    public override void InitializeSubState() {}
}
