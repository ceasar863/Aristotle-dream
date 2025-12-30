using UnityEngine;


public class Player_Aristotle_Auxiliary_Shoot_State : Player_State
{
    private Weapon weapon;

    public Player_Aristotle_Auxiliary_Shoot_State
        (Entity entity, 
        State_Machine state_machine, 
        string name,
        Weapon weapon) 
        : base(entity, state_machine, name)
    {
        this.weapon = weapon;
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

