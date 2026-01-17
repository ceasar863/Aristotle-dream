using Unity.VisualScripting;
using UnityEngine;

public class Player_Aristotle_Shoot_State : Player_State
{
    private Weapon weapon;

    public Player_Aristotle_Shoot_State
        (Player_Aristotle player_aristotle, 
         State_Machine state_machine, 
         string name,
         Weapon weapon)//在构造函数中传入武器  
         :base(player_aristotle, state_machine, name)
    {
        this.weapon = weapon;

        weapon.on_exit += Exit_Handler;
    }

    public override void Enter()
    {
        base.Enter();
        weapon.Enter();
        Event_Center.Broad_Cast(Event_Type.Set_Switch_Weapon_Interactable, false);
    }

    public override void Update()
    {
        base.Update();
        if (Player_Input_System.instance.player_input.Player.Shoot.IsPressed()) weapon.current_input = true;
        else weapon.current_input = false;

        if (trigger_called || weapon.current_input==false) 
            state_machine.Change_State(player_aristotle.idle_state);
    }

  
    public override void Exit()
    {
        base.Exit();
    }

    private void Exit_Handler()
    {
        Animation_Trigger();
    }

    //private void Shoot_Bullet()
    //{
    //    if (should_do && !have_done)
    //    {
    //        ammo_system.Shoot_Bullet(player_aristotle.shoot_point.transform.position);
    //        ammo_system.Clear_Bullet();
    //        have_done = true;
    //    }
    //}
}
