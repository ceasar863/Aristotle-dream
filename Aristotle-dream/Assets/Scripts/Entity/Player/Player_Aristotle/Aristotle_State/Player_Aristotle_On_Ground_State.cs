using UnityEngine;

public class Player_Aristotle_On_Ground_State : Player_State
{
    public Player_Aristotle_On_Ground_State(Entity_Player_Aristotle player_aristotle, State_Machine state_machine, string name) : base(player_aristotle, state_machine, name)
    {


    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();

        if (player_aristotle.player_input.Player.Grab.IsPressed())
            player_aristotle.Try_Grab_Monster();

        if (player_aristotle.is_aiming)
            state_machine.Change_State(player_aristotle.aim_state);

        if(player_aristotle.player_input.Player.Jump.WasPerformedThisFrame())
            state_machine.Change_State(player_aristotle.jump_state);

        if (player_aristotle.rb.linearVelocity.y < 0 && player_aristotle.Get_Parameter<bool>("is_in_air"))
            state_machine.Change_State(player_aristotle.fall_state);
    }

    public override void Exit()
    {
        base.Exit();
    }
}
