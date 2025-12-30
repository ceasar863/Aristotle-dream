using UnityEngine;

public class Player_State : Entity_State
{
    protected Player_Aristotle player_aristotle;
    public Player_State(Entity entity, State_Machine state_machine, string name) : base(entity, state_machine, name)
    {
        player_aristotle = (Player_Aristotle)entity;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();
        if (player_aristotle.GetComponent<Entity_Health>().current_health_percent() <= 0)
            state_machine.Change_State(player_aristotle.dead_state);
    }

    public override void Exit()
    {
        base.Exit();
    }
}
