using System.Collections.Generic;
using UnityEngine;

enum GroundEnemyStates
{
    patrol,
    chase
}
public class GroundEnemyStateFactory
{
    GroundEnemyStateMachine _context;
    Dictionary<GroundEnemyStates, EnemyBaseState> _states = new Dictionary<GroundEnemyStates, EnemyBaseState>();
    public GroundEnemyStateFactory(GroundEnemyStateMachine currentContext)
    {
        _context = currentContext;
        _states[GroundEnemyStates.patrol] = new GroundEnemyPatrolState(_context, this);
        _states[GroundEnemyStates.chase] = new GroundEnemyChaseState(_context, this);

    }

    public EnemyBaseState Patrol()
    {
        return _states[GroundEnemyStates.patrol];
    }

    public EnemyBaseState Chase()
    {

        return _states[GroundEnemyStates.chase];
    }
}
