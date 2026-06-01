using UnityEngine;

public class PlayerStillState : PlayerBaseState
{
    public PlayerStillState(PlayerStateMachine ctx, PlayerStateFactory factory) : base(ctx, factory)
    {
    }

    public override void CheckSwitchState()
    {
      
    }

    public override void EnterState()
    {

    }

    public override void ExitState()
    {
       
    }

    public override void InitializeSubState()
    {
      
    }

    public override void UpdateState()
    {
        Ctx.AppliedMovementX = 0;
        Ctx.AppliedMovementZ = 0;
        CheckSwitchState();
    }
}
