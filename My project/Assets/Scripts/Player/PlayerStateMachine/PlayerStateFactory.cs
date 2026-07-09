using System.Collections.Generic;

enum PlayerStates
{
    move,
    grounded,
    jump,
    fall,
    dash,
    groundPound,
    still,
    wall,
    wallJump,
    knockback,
    bounce,
    climb,
    changeScene
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
        _states[PlayerStates.wall] = new PlayerWallState(_context, this);
        _states[PlayerStates.wallJump] = new PlayerWallJumpState(_context, this);
        _states[PlayerStates.knockback] = new PlayerKnockbackState(_context, this);
        _states[PlayerStates.bounce] = new PlayerBounceState(_context, this);
        _states[PlayerStates.climb] = new PlayerClimbState(_context, this);
        _states[PlayerStates.changeScene] = new PlayerChangeSceneState(_context, this);
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
    public PlayerBaseState Wall()
    {
        return _states[PlayerStates.wall];
    }
    public PlayerBaseState WallJump()
    {
        return _states[PlayerStates.wallJump];
    }
    public PlayerBaseState Knockback()
    {
        return _states[PlayerStates.knockback];
    }
    public PlayerBaseState Bounce()
    {
        return _states[PlayerStates.bounce];
    }
    public PlayerBaseState Climb()
    {
        return _states[PlayerStates.climb];
    }
    public PlayerBaseState ChangeScene()
    {
        return _states[PlayerStates.changeScene];
    }
}
