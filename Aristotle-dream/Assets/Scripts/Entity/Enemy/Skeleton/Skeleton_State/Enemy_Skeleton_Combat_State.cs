using UnityEngine;

public class Enemy_Skeleton_Combat_State : Enemy_Skeleton_State
{
    private Entity_Player_Aristotle player;

    public Enemy_Skeleton_Combat_State(Entity entity, State_Machine state_machine, string name) : base(entity, state_machine, name)
    {

    }

    public override void Enter()
    {
        base.Enter();
        timer = skeleton.combat_time;
        
        if(player==null)
            player = skeleton.player_target.collider.GetComponent<Entity_Player_Aristotle>(); ;
    }

    public override void Update()
    {
        base.Update();
        Try_Chase_Player();

        if (skeleton.could_attack)
        {
            skeleton.could_attack = false;
            state_machine.Change_State(skeleton.skeleton_attack_state);
        }

        if (!skeleton.has_checked_player && timer < 0)
            state_machine.Change_State(skeleton.skeleton_idle_state);

        if (skeleton.has_checked_player)
            timer = skeleton.combat_time;
    }

    public override void Exit()
    {
        base.Exit();
    }

    private void Try_Chase_Player()
    {
        int dire = skeleton.Turn_To_Player(player);
        float speed = skeleton.could_attack ? 0.01f : skeleton.combat_chase_speed;
        skeleton.Set_Velocity(speed * dire, skeleton.rb.linearVelocity.y);
    }
}
