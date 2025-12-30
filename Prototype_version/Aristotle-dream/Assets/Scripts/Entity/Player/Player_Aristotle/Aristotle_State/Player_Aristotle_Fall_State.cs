using UnityEngine;

public class Player_Aristotle_Fall_State : Player_Aristotle_In_Air_State
{
    public Player_Aristotle_Fall_State(Player_Aristotle player_aristotle, State_Machine state_machine, string name) : base(player_aristotle, state_machine, name)
    {

    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();
        if(player_aristotle.Get_Parameter<bool>(Entity_Attribute.is_on_ground))
            state_machine.Change_State(player_aristotle.idle_state);

    }

    public override void Exit()
    {
        base.Exit();
    }
}
