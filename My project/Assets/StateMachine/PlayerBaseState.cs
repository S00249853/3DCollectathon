

using Unity.Collections;

public abstract class PlayerBaseState
{
    private bool _isRootState = false;
    private PlayerStateMachine _ctx;
    private PlayerStateFactory _factory;
    private PlayerBaseState _currentSubState;
    private PlayerBaseState _currentSuperState;

    protected bool IsRootState { set { _isRootState = value; } }
    protected PlayerStateMachine Ctx { get {  return _ctx; } } 
    protected PlayerStateFactory Factory { get { return _factory; } }
    public PlayerBaseState(PlayerStateMachine ctx, PlayerStateFactory factory)
    {
        _ctx = ctx;
        _factory = factory;
    }
    public abstract void EnterState();

    public abstract void UpdateState();

    public abstract void ExitState();

    public abstract void CheckSwitchState();

    public abstract void InitializeSubState();

    public void UpdateStates(){
    UpdateState();
        if (_currentSubState != null)
        {
            _currentSubState.UpdateStates();
        }
    }

    protected void SwitchState(PlayerBaseState newState) {
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

    protected void SetSuperState(PlayerBaseState newSuperState) {
    _currentSuperState = newSuperState;
    }

    protected void SetSubState(PlayerBaseState newSubState) {
        _currentSubState = newSubState;
        newSubState.SetSuperState(this);
    }
}
