using Unity.VisualScripting;
using UnityEngine;

public class Entity_State
{
   
    protected Entity entity;
    protected Rigidbody2D rb;
    protected SpriteRenderer sr;
    protected Animator anim;
    protected State_Machine state_machine;

    protected float timer;
    protected string state_name;
    protected bool trigger_called;
    protected bool should_do;
    protected bool have_done;

    public Entity_State(Entity entity , State_Machine state_machine ,string name)
    {
        this.entity = entity;

        this.state_machine = state_machine;
        this.state_name = name;
        this.rb = entity.rb;
        this.sr = entity.sr;
        this.anim = entity.anim;
    }

    public virtual void Enter()
    {
        should_do = false;
        have_done = false;
        entity.anim.SetBool(state_name, true);
        trigger_called = false;
    }

    public virtual void Update()
    {
        timer -= Time.deltaTime;
    }

    public virtual void Exit()
    {
        entity.anim.SetBool(state_name, false);
    }

    public void Should_Do_it()
    {
        should_do = true;
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
