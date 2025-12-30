using UnityEngine;

public class Player_Aristotle_Dead_State : Player_State
{
    public Player_Aristotle_Dead_State(Entity entity, State_Machine state_machine, string name) : base(entity, state_machine, name)
    {
    }

    public override void Enter()
    {
        base.Enter();
        Player_Input_System.instance.Set_Input_System_Active(false);
    }

    public override void Update()
    {
        base.Update();
    }

    public override void Exit()
    {
        base.Exit();
    }
}
