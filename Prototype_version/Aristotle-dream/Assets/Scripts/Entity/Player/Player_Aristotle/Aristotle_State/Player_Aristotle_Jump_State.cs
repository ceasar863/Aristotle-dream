using UnityEngine;

public class Player_Aristotle_Jump_State : Player_Aristotle_In_Air_State
{
    public Player_Aristotle_Jump_State(Player_Aristotle player_aristotle, State_Machine state_machine, string name) : base(player_aristotle, state_machine, name)
    {

    }

    public override void Enter()
    {
        base.Enter();
        player_aristotle.Set_Velocity(0, player_aristotle.jump_force);
    }

    public override void Update()
    {
        base.Update();
        if(player_aristotle.rb.linearVelocity.y < 0)
            state_machine.Change_State(player_aristotle.fall_state);

    }

    public override void Exit()
    {
        base.Exit();
    }
}
