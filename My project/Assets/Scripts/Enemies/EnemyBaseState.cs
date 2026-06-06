using UnityEngine;

public abstract class EnemyBaseState
{
    private bool _isRootState = false;
    private GroundEnemyStateMachine _ctx;
    private GroundEnemyStateFactory _factory;
    private EnemyBaseState _currentSubState;
    private EnemyBaseState _currentSuperState;

    protected bool IsRootState { set  { _isRootState = value; } }
    protected GroundEnemyStateMachine Ctx { get { return _ctx; } }
    protected GroundEnemyStateFactory Factory { get { return _factory; } }
    public EnemyBaseState(GroundEnemyStateMachine ctx, GroundEnemyStateFactory factory)
    {
        _ctx = ctx; 
        _factory = factory;
    }
    public abstract void EnterState();

    public abstract void UpdateState();

    public abstract void ExitState();

    public abstract void CheckSwitchState();

    public abstract void InitializeSubState();
    public void UpdateStates()
    {
        UpdateState();
        if (_currentSubState != null)
        {
            _currentSubState.UpdateStates();
        }
    }

    protected void SwitchState(EnemyBaseState newState)
    {
        ExitState();

        newState.EnterState();

        if (_isRootState)
        {
            Ctx.CurrentState = newState;
        }
        else
        {
            if (_currentSuperState != null)
            {
                _currentSuperState.SetSubState(newState);
            }
        }
    }

    protected void SetSuperState(EnemyBaseState newSuperState)
    {
        _currentSuperState = newSuperState;
    }

    protected void SetSubState(EnemyBaseState newSubState)
    {
        _currentSubState = newSubState;
        newSubState.SetSuperState(this);
    }

}
