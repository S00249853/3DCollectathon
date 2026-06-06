using UnityEngine;

public class GroundEnemyChaseState : EnemyBaseState
{
    public GroundEnemyChaseState(GroundEnemyStateMachine ctx, GroundEnemyStateFactory factory) : base(ctx, factory)
    {
        IsRootState = true;
    }

    public override void CheckSwitchState()
    {
        if (Vector3.Distance(Ctx.Home, Ctx.transform.position) >= Ctx.HomeRadius)
        {
            SwitchState(Factory.Patrol());
        }
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
        Ctx.Agent.SetDestination(Ctx.PlayerLocation.position);
        CheckSwitchState();
    }
}
