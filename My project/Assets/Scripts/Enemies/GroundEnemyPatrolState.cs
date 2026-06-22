
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundEnemyPatrolState : EnemyBaseState
{
    int _currentPoint = 0;
    float _moveCdTimer;
    float _moveCd = 2f;
    public GroundEnemyPatrolState(GroundEnemyStateMachine ctx, GroundEnemyStateFactory factory) : base(ctx, factory)
    {
        IsRootState = true;
    }
    public override void CheckSwitchState()
    {
       if (Vector3.Distance(Ctx.PlayerLocation.position, Ctx.transform.position) <= 15 && Vector3.Distance(Ctx.Home, Ctx.PlayerLocation.position) < Ctx.HomeRadius && !Ctx.PlayerMachine.IsHurt)
        {
            SwitchState(Factory.Chase());
        }
    }

    public override void EnterState()
    {
        _moveCdTimer = _moveCd;
    }

    public override void ExitState()
    {
        Ctx.Agent.isStopped = false;
    }

    public override void InitializeSubState()
    {
        
    }


    public override void UpdateState()
    {
        if (_moveCdTimer > 0 && Ctx.Agent.isStopped)
        {
            _moveCdTimer -= Time.deltaTime;
        }
        Patrol();
        CheckSwitchState();
    }
    void Patrol()
    {
        if (Vector3.Distance(Ctx.Home, Ctx.transform.position) >= Ctx.HomeRadius)
        {
            Ctx.Agent.SetDestination(Ctx.Home);
        }
        else
        {
            Ctx.Agent.SetDestination(Ctx.WayPoints[_currentPoint].position);

            Vector3 distanceToWalkPoint = Ctx.transform.position - Ctx.WayPoints[_currentPoint].position;

            if (distanceToWalkPoint.magnitude < 2.5f)
            {
                Ctx.Agent.isStopped = true;

                if (_moveCdTimer > 0)
                {
                    return;
                }
                else
                {
                    Ctx.Agent.isStopped = false;
                    _moveCdTimer = _moveCd;
                    if (_currentPoint == Ctx.WayPoints.Length - 1)
                    {
                        _currentPoint = 0;
                    }

                    else
                    {
                        _currentPoint++;
                    }
                }

            }
        }
    }
}
