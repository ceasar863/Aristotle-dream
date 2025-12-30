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
        if (should_do && have_done==false)
        {
            have_done = true;
            skeleton.GetComponent<Entity_Combat>().Perform_Attack_Circle(skeleton.attack_distance, skeleton.what_is_player, skeleton.attack_damage , skeleton.attack_start_point);
        }

        if (trigger_called)
            state_machine.Change_State(skeleton.skeleton_combat_state);
    }

    public override void Exit()
    {
        base.Exit();
    }
}
