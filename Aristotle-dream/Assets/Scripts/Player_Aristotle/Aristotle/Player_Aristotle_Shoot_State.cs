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
        if (trigger_called)
            state_machine.Change_State(player_aristotle.idle_state);
    }

    public override void Exit()
    {
        base.Exit();
    }
}
