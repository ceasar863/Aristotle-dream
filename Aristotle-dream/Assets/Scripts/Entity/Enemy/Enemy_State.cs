using UnityEngine;

public class Enemy_State : Entity_State
{
    public Enemy_State(Entity entity, State_Machine state_machine, string name) : base(entity, state_machine, name)
    {
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
