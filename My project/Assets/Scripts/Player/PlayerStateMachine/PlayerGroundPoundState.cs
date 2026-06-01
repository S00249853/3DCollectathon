using UnityEngine;

public class PlayerGroundPoundState : PlayerBaseState
{
    private float _delay = 0.25f;
    private float _delayTimer;
    public PlayerGroundPoundState(PlayerStateMachine ctx, PlayerStateFactory factory) : base(ctx, factory)
    {
        IsRootState = true;
    }

    public override void CheckSwitchState()
    {
        if (Ctx.CharacterController.isGrounded)
        {
            SwitchState(Factory.Grounded());
        }
    }

    public override void EnterState()
    {
        InitializeSubState();
        _delayTimer = _delay;
    }

    public override void ExitState()
    {
        Ctx.IsGroundPounding = false;
    }

    public override void InitializeSubState()
    {
        SetSubState(Factory.Still());
    }

    public override void UpdateState()
    {
        if (_delayTimer > 0)
        {
            Ctx.AppliedMovementY = 0;
            _delayTimer -= Time.deltaTime;
        }
        else
        {
            Ctx.AppliedMovementX = 0;
            Ctx.AppliedMovementY = -15;
            Ctx.AppliedMovementZ = 0;
        }
        CheckSwitchState();

    }
}
