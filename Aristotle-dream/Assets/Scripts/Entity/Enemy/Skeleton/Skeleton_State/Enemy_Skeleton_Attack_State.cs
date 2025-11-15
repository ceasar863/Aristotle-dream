using UnityEngine;

public class Enemy_Skeleton_Attack_State : Enemy_Skeleton_State
{
    public Enemy_Skeleton_Attack_State(Entity entity, State_Machine state_machine, string name) : base(entity, state_machine, name)
    {

    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();
        if (trigger_called)
            state_machine.Change_State(skeleton.skeleton_combat_state);
    }

    public override void Exit()
    {
        base.Exit();
    }
}
