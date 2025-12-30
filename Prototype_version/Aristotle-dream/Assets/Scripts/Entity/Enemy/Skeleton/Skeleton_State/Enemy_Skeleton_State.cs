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
        if (skeleton.has_checked_player && skeleton.player_target.collider != null && state_machine.current_state != skeleton.skeleton_attack_state)
        {
            if(skeleton.Get_Parameter<bool>(Entity_Attribute.is_dead)==false)
                state_machine.Change_State(skeleton.skeleton_combat_state);
        }

        if (skeleton.GetComponent<Entity_Health>().current_health_percent() <= 0 && skeleton.Get_Parameter<bool>(Entity_Attribute.is_dead)==false)
            state_machine.Change_State(skeleton.skeleton_dead_state);
    }

    public override void Exit()
    {
        base.Exit();
    }
}
