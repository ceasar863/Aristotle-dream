using System.Collections;
using System.Net.Security;
using UnityEngine;
using UnityEngine.U2D.Animation;

public class Enemy_Skeleton_Idle_State : Enemy_Skeleton_State
{
    public Enemy_Skeleton_Idle_State(Entity entity, State_Machine state_machine, string name) : base(entity, state_machine, name)
    {

    }

    public override void Enter()
    {
        base.Enter();
        timer = enemy.idle_time;
    }

    public override void Update()
    {
        base.Update();
        if (timer < 0)
            state_machine.Change_State(skeleton.skeleton_move_state);
    }

    public override void Exit()
    {
        base.Exit();
    }

}
