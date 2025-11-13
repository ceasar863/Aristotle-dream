using UnityEngine;

public class Enemy_Skeleton_Move_State : Enemy_Skeleton_State
{

    public Enemy_Skeleton_Move_State(Entity entity, State_Machine state_machine, string name) : base(entity, state_machine, name)
    {
    }

    public override void Enter()
    {
        base.Enter();
        if (skeleton.next_wall || skeleton.next_brim)
        {
            skeleton.Flip();
        }
    }

    public override void Update()
    {
        base.Update();
        timer -= Time.deltaTime;
        skeleton.Set_Velocity(skeleton.move_speed*skeleton.Get_Parameter<int>("facing_dir") , skeleton.rb.linearVelocity.y);

        if (skeleton.next_wall || skeleton.next_brim)
        {
            state_machine.Change_State(skeleton.skeleton_idle_state);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }

}
