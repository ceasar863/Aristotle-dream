using UnityEngine;

public class Player_State : Entity_State
{
    protected Entity_Player_Aristotle player_aristotle;
    protected Ammo_System ammo_system;

    public Player_State(Entity entity, State_Machine state_machine, string name) : base(entity, state_machine, name)
    {
        player_aristotle = (Entity_Player_Aristotle)entity;
        this.ammo_system = player_aristotle.ammo_system;
    }

    public override void Enter()
    {
        base.Enter();
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
