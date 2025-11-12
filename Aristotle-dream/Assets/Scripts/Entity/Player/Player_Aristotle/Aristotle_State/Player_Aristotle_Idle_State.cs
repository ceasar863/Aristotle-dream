using UnityEngine;

public class Player_Aristotle_Idle_State : Player_Aristotle_On_Ground_State
{
    public Player_Aristotle_Idle_State(Entity_Player_Aristotle player_tenshi, State_Machine state_machine, string name) : base(player_tenshi, state_machine, name)
    {

    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();

        if (player_aristotle.movement_input.x != 0)
            state_machine.Change_State(player_aristotle.move_state);
    }

    public override void Exit()
    {
        base.Exit();
    }


}
