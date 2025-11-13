using UnityEngine;

public class Enemy_Skeleton_State : Enemy_State
{
    public Enemy_Skeleton skeleton;

    public Enemy_Skeleton_State(Entity entity, State_Machine state_machine, string name) : base(entity, state_machine, name)
    {

    }

    public override void Enter()
    {
        base.Enter();
        skeleton = enemy.gameObject.GetComponent<Enemy_Skeleton>();
    }

    public override void Update()
    {
        base.Update();
    }

    public override void Exit()
    {
        base.Exit();
    }
}
