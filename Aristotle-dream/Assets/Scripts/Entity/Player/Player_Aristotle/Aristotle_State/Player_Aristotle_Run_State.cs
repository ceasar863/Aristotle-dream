using UnityEngine;

public class Player_Aristotle_Run_State : Player_Aristotle_On_Ground_State
{
    public Player_Aristotle_Run_State(Entity_Player_Aristotle player_aristotle, State_Machine state_machine, string name) : base(player_aristotle, state_machine, name)
    {

    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();
        player_aristotle.Set_Velocity(player_aristotle.run_speed* player_aristotle.movement_input.x, rb.linearVelocity.y);
        if (!player_aristotle.player_input.Player.Run.IsPressed())
            state_machine.Change_State(player_aristotle.idle_state);
    }

    public override void Exit()
    {
        base.Exit();
    }

}
