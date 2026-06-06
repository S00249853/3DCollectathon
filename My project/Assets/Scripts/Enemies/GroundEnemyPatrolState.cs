using UnityEngine;

public class GroundEnemyPatrolState : EnemyBaseState
{
    public GroundEnemyPatrolState(GroundEnemyStateMachine ctx, GroundEnemyStateFactory factory) : base(ctx, factory)
    {
        IsRootState = true;
    }

    public override void CheckSwitchState()
    {
       if (Vector3.Distance(Ctx.PlayerLocation.position, Ctx.transform.position) <= 15 && Vector3.Distance(Ctx.Home, Ctx.transform.position) < Ctx.HomeRadius)
        {
            SwitchState(Factory.Chase());
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
        if (Vector3.Distance(Ctx.Home, Ctx.transform.position) >= Ctx.HomeRadius)
        {
            Ctx.Agent.SetDestination(Ctx.Home);
        }
        CheckSwitchState();
    }
}
