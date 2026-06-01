using System.Collections.Generic;

enum PlayerStates
{
    move,
    grounded,
    jump,
    fall,
    dash,
    groundPound,
    still
}

public class PlayerStateFactory 
{
    PlayerStateMachine _context;
    Dictionary<PlayerStates, PlayerBaseState> _states = new Dictionary<PlayerStates, PlayerBaseState>();
    public PlayerStateFactory(PlayerStateMachine currentContext)
    {
        _context = currentContext;
        _states[PlayerStates.move] = new PlayerMoveState(_context, this);
        _states[PlayerStates.grounded] = new PlayerGroundedState(_context, this);
        _states[PlayerStates.jump] = new PlayerJumpState(_context, this);
        _states[PlayerStates.fall] = new PlayerFallState(_context, this);
        _states[PlayerStates.dash] = new PlayerDashState(_context, this);
        _states[PlayerStates.groundPound] = new PlayerGroundPoundState(_context, this);
        _states[PlayerStates.still] = new PlayerStillState(_context, this);
    }

    public PlayerBaseState Move()
    {
        return _states[PlayerStates.move];
    }

    public PlayerBaseState Jump()
    {
        return _states[PlayerStates.jump];
    }

    public PlayerBaseState Grounded()
    {
        return _states[PlayerStates.grounded];
    }

    public PlayerBaseState Fall()
    {
        return _states[PlayerStates.fall];
    }

    public PlayerBaseState Dash()
    {
        return _states[PlayerStates.dash];
    }
    public PlayerBaseState GroundPound()
    {
        return _states[PlayerStates.groundPound];
    }
    public PlayerBaseState Still()
    {
        return _states[PlayerStates.still];
    }
}
