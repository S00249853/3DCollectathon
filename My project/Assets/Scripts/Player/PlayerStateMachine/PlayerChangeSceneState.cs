using UnityEngine;

public class PlayerChangeSceneState : PlayerBaseState
{
    public PlayerChangeSceneState(PlayerStateMachine ctx, PlayerStateFactory factory) : base(ctx, factory)
    {
IsRootState = true;
    }

    public override void CheckSwitchState()
    {
        if (Ctx.StopMoving == false)
        {
           SwitchState(Factory.Grounded());
        }
    }

    public override void EnterState()
    {
        InitializeSubState();
    }

    public override void ExitState()
    {

    }

    public override void InitializeSubState()
    {
        SetSubState(Factory.Still());
    }

    public override void UpdateState()
    {
        //Ctx.CurrentMovementY = 0;
        //Ctx.AppliedMovementY = 0;
        CheckSwitchState();
    }
}
