using UnityEngine;

public class Enemy_Skeleton : Enemy
{
    #region
    public Enemy_Skeleton_Idle_State skeleton_idle_state { get; private set; }
    public Enemy_Skeleton_Move_State skeleton_move_state { get; private set; }


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

        state_machine.Initiate(skeleton_idle_state);
    }
    protected override void Update()
    {
        base.Update();
    }

    [ContextMenu("Test")]
    public void Test()
    {
        state_machine.Change_State(skeleton_idle_state);
    }
}
