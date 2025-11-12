using UnityEngine;

public class Enemy_Skeleton : Enemy
{
    #region
    public Enemy_Skeleton_Idle_State skeleton_idle_state { get; private set; }


    #endregion
    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();

        skeleton_idle_state = new Enemy_Skeleton_Idle_State(this, state_machine, "Idle");

        state_machine.Initiate(skeleton_idle_state);
    }
    protected override void Update()
    {
        base.Update();
    }

}
