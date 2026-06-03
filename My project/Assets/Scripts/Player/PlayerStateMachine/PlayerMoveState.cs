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
        if (!Ctx.FreezeMovement)
        {
            Ctx.AppliedMovementX = Ctx.MovementInput.x * Ctx.WalkSpeed;
            Ctx.AppliedMovementZ = Ctx.MovementInput.y * Ctx.WalkSpeed;
        }
        CheckSwitchState();
    }

    public override void ExitState() { }

    public override void CheckSwitchState() { }

    public override void InitializeSubState() {}
}
