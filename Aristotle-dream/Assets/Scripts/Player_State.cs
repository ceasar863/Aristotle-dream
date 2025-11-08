using UnityEngine;

public class Player_State
{
    protected Character_Player_Tenshi player_tenshi;
    protected Rigidbody2D rb;
    protected SpriteRenderer sr;
    protected Animator anim;
    protected State_Machine state_machine;
    protected float timer;
    protected string state_name;
    protected bool trigger_called;

    public Player_State(Character_Player_Tenshi player_tenshi , State_Machine state_machine, string name)
    {
        this.player_tenshi = player_tenshi;
        this.state_machine = state_machine;
        this.rb = player_tenshi.rb;
        this.sr = player_tenshi.sr;
        this.anim = player_tenshi.anim;
        this.state_name = name;
    }

    public void Awake()
    {
        player_tenshi = Character_Player_Tenshi.instance;
        state_machine = player_tenshi.state_machine;
    }

    public virtual void Enter()
    {
        player_tenshi.anim.SetBool(state_name, true);
        trigger_called = false;
    }

    public virtual void Update()
    {
        timer -= Time.deltaTime;

        if(state_machine.current_state != player_tenshi.attack_while_drop)
            player_tenshi.Check_Dash_Attack_Or_Just_Attack();
    }

    public virtual void Exit()
    {
        player_tenshi.anim.SetBool(state_name, false);
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
