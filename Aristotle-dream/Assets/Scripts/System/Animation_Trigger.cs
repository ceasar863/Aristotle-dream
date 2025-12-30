using UnityEngine;

public class Animation_Trigger : MonoBehaviour
{
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
