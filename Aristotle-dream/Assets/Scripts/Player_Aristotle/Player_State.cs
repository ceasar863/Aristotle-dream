using UnityEngine;

public class Player_State
{
    protected Entity_Player_Aristotle player_aristotle;
    protected Rigidbody2D rb;
    protected SpriteRenderer sr;
    protected Animator anim;
    protected State_Machine state_machine;

    protected float timer;
    protected string state_name;
    protected bool trigger_called;

    public Player_State(Entity_Player_Aristotle player_aristotle, State_Machine state_machine, string name)
    {
        this.player_aristotle = player_aristotle;
        this.state_machine = state_machine;
        this.rb = player_aristotle.rb;
        this.sr = player_aristotle.sr;
        this.anim = player_aristotle.anim;
        this.state_name = name;
    }

    public void Awake()
    {
        player_aristotle = Entity_Player_Aristotle.instance;
        state_machine = player_aristotle.state_machine;
    }

    public virtual void Enter()
    {
        player_aristotle.anim.SetBool(state_name, true);
        trigger_called = false;
    }

    public virtual void Update()
    {
        timer -= Time.deltaTime;
    }

    public virtual void Exit()
    {
        player_aristotle.anim.SetBool(state_name, false);
    }

    public void Animation_Trigger()
    {
        trigger_called = true;
    }

    public bool Get_Trigger()
    {
        return trigger_called;
    }
}
