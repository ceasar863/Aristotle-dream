using UnityEngine;

public class Player_Aristotle_Shoot_State : Player_State
{
    public Player_Aristotle_Shoot_State(Entity_Player_Aristotle player_aristotle, State_Machine state_machine, string name) : base(player_aristotle, state_machine, name)
    {

    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();
        if(should_do && !have_done)
        {
            GameObject.Instantiate(player_aristotle.bullet_prefab,
                              player_aristotle.shoot_point.transform.position,
                              Quaternion.identity);
            have_done = true;
        }

        if (trigger_called)
            state_machine.Change_State(player_aristotle.idle_state);
    }

    public override void Exit()
    {
        base.Exit();
    }
}
