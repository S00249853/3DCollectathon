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
        CheckSwitchState();
        Ctx.AppliedMovementX = Ctx.MovementInput.x * Ctx.WalkSpeed;
        Ctx.AppliedMovementZ = Ctx.MovementInput.y * Ctx.WalkSpeed;
    }

    public override void ExitState() { }

    public override void CheckSwitchState() { }

    public override void InitializeSubState() {}
}
