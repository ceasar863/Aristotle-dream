using UnityEngine;

public class Enemy_Skeleton_Dead_State : Enemy_Skeleton_State
{
    public Enemy_Skeleton_Dead_State(Entity entity, State_Machine state_machine, string name) : base(entity, state_machine, name)
    {

    }

    public override void Enter()
    {
        base.Enter();
        timer = skeleton.dead_destroy_time;
        skeleton.Set_Dead_State(true);
    }

    public override void Update()
    {
        base.Update();
        if (timer < 0)
        {
            skeleton.GetComponent<Entity_Health>().Die(); 
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
