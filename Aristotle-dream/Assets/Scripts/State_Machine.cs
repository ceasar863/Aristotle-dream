using UnityEngine;

public class State_Machine
{
    private Entity_Player_Aristotle player_tenshi = Entity_Player_Aristotle.instance;
    public Player_State current_state { get; private set; }

    public void Initiate()
    {
        current_state = player_tenshi.idle_state;
        current_state.Enter();
    }

    public void Update_State()
    {
        current_state.Update();
    }

    public void Change_State(Player_State new_state)
    {
        current_state.Exit();
        current_state = new_state;
        current_state.Enter();
    }

}
