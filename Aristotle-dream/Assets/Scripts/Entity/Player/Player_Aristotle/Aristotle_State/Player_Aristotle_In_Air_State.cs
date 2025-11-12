using UnityEngine;

public class Player_Aristotle_In_Air_State : Player_State
{
    public Player_Aristotle_In_Air_State(Entity_Player_Aristotle player_aristotle, State_Machine state_machine, string name) : base(player_aristotle, state_machine, name)
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
            player_aristotle.Set_Velocity(player_aristotle.move_speed*0.76f , rb.linearVelocity.y);   
    }

    public override void Exit()
    {
        base.Exit();
    }

}
