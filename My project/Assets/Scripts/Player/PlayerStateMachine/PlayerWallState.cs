using System.Collections;
using UnityEngine;

public class PlayerWallState : PlayerBaseState
{
    //bool _onWall;
    public PlayerWallState(PlayerStateMachine ctx, PlayerStateFactory factory) : base(ctx, factory)
    {
        IsRootState = true;
    }

    public override void CheckSwitchState()
    {
         if (Ctx.CharacterController.isGrounded || Ctx.IsDead)
        {
            SwitchState(Factory.Grounded());
        }
        else if (Ctx.IsDashing)
        {
            SwitchState(Factory.Dash());
        }
        else if (Ctx.WallJump)
        {
            SwitchState(Factory.WallJump());
        }
        else if (Ctx.IsHurt)
        {
            SwitchState(Factory.Knockback());
        }
        else if (Ctx.IsBounce)
        {
            SwitchState(Factory.Bounce());
        }
        else if (!Ctx.OnWall)
        {
            SwitchState(Factory.Fall());
        }
        else if (Ctx.IsClimb)
        {
            SwitchState(Factory.Climb());
        }
        else if (Ctx.StopMoving)
        {
            SwitchState(Factory.ChangeScene());
        }
    }

    public override void EnterState()
    {
        InitializeSubState();
        //Ctx.StartCoroutine(WallDelayRoutine());
    }

    public override void ExitState()
    {
        Ctx.OnWall = false;
        Debug.Log("Exiting Wall");
    }

    public override void InitializeSubState()
    {
        SetSubState(Factory.Still());
    }

    //private IEnumerator WallDelayRoutine()
    //{
    //    yield return new WaitForSeconds(.5f);
    //    _onWall = true;
    //}

    public override void UpdateState()
    {
        Ctx.AppliedMovementY = -1f;
        //if (_onWall)
        //{
        //    Debug.Log("On Wall");
        //    Ray ray = new Ray(Ctx.transform.position, -Ctx.transform.forward);
        //    Physics.Raycast(ray, out RaycastHit hit, 10f);
        //    if (hit.collider.gameObject != Ctx.CurrentWall || hit.collider.gameObject == null)
        //    {
        //        Debug.Log("Off Wall");
        //        Ctx.OnWall = false;
        //    }
        //}
        CheckSwitchState();
    }
}
