using Badtime;
using System;
using UnityEngine;

public class Animation_Trigger : MonoBehaviour
{
    public event Action On_Finished;
    public event Action On_Start_Movement;
    public event Action On_Stop_Movement;
    public event Action On_Attack_Action;
    public event Action On_Min_Hold_Passed;
    public event Action On_Shoot_Bullet;
    public event Action<Attack_Phases> On_Enter_Attack_Phase;

    private void Animation_Trigger_Weapon() => On_Finished?.Invoke();
    private void Start_Movement_Trigger() => On_Start_Movement?.Invoke();
    private void Stop_Movement_Trigger() => On_Stop_Movement?.Invoke();
    private void Attack_Action_Trigger()=> On_Attack_Action?.Invoke();
    private void MinHold_Passed_Trigger() => On_Min_Hold_Passed?.Invoke();
    private void Shoot_Bullet() => On_Shoot_Bullet?.Invoke();
    private void Enter_Attack_Phase(Attack_Phases phase) => On_Enter_Attack_Phase?.Invoke(phase);


    /*---------------------------------------------------------------*/
    private Entity entity;
    private State_Machine state_machine;

    private void Start()
    {
        entity = GetComponentInParent<Entity>();
        state_machine = entity.state_machine;
    }

    public void Do_Animation_Trigger()
    {
        state_machine.current_state.Animation_Trigger();
    }

    //public void Freeze_Character()
    //{
    //    player_aristotle.is_freeze = true;
    //    player_aristotle.rb.gravityScale = 0;
    //    player_aristotle.rb.linearVelocity = new Vector2(0, 0);
    //}

    //public void UnFreeze_Character()
    //{
    //    player_aristotle.is_freeze = false;
    //    player_aristotle.rb.gravityScale = player_aristotle.gravity_scale;
    //}

    //public void Set_Drop_Velocity()
    //{
    //    player_aristotle.rb.linearVelocity = new(0 , -player_aristotle.player_drop_force);
    //}

    public void Just_Do_It()
    {
        entity.state_machine.current_state.Should_Do_it();
    }

    public void Freeze_Anim()
    {
        entity.anim.speed = 0f;
    }

    public void UnFreeze_Anim()
    {
        entity.anim.speed = 1f;
       
    }
}
