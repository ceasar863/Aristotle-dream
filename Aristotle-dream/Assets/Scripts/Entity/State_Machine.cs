using UnityEngine;

public class State_Machine
{
    public Entity_State current_state { get; private set; }

    public void Initiate(Entity_State state)
    {
        current_state = state;
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
