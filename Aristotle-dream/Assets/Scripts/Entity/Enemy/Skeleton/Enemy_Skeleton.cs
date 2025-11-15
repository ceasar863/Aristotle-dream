using UnityEngine;

public class Enemy_Skeleton : Enemy
{
    #region
    public Enemy_Skeleton_Idle_State skeleton_idle_state { get; private set; }
    public Enemy_Skeleton_Move_State skeleton_move_state { get; private set; }
    public Enemy_Skeleton_Combat_State skeleton_combat_state { get; private set; } 
    public Enemy_Skeleton_Attack_State skeleton_attack_state { get; private set; }

    #endregion
    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();

        skeleton_idle_state = new Enemy_Skeleton_Idle_State(this, state_machine, "Idle");
        skeleton_move_state = new Enemy_Skeleton_Move_State(this, state_machine, "Move");
        skeleton_combat_state = new Enemy_Skeleton_Combat_State(this, state_machine, "Combat");
        skeleton_attack_state = new Enemy_Skeleton_Attack_State(this, state_machine, "Attack");

        state_machine.Initiate(skeleton_idle_state);
    }

    protected override void Update()
    {
        base.Update();
    }
}
