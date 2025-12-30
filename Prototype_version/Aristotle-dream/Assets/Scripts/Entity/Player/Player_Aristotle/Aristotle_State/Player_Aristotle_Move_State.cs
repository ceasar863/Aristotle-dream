using UnityEngine;

public class Player_Aristotle_Move_State : Player_Aristotle_On_Ground_State
{
    public Player_Aristotle_Move_State(Player_Aristotle player_tenshi, State_Machine state_machine, string name) : base(player_tenshi, state_machine, name)
    {

    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();
        player_aristotle.Set_Velocity(player_aristotle.move_speed*player_aristotle.movement_input.x , rb.linearVelocity.y);

        if (player_aristotle.movement_input.x == 0)
            state_machine.Change_State(player_aristotle.idle_state);

        if (Player_Input_System.instance.player_input.Player.Run.IsPressed())
            state_machine.Change_State(player_aristotle.run_state);
    }

    public override void Exit()
    {
        base.Exit();
    }
}
