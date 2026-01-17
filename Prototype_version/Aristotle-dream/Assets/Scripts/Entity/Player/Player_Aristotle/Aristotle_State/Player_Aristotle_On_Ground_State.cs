using UnityEngine;

public class Player_Aristotle_On_Ground_State : Player_State
{
    public Player_Aristotle_On_Ground_State(Player_Aristotle player_aristotle, State_Machine state_machine, string name) : base(player_aristotle, state_machine, name)
    {


    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();

        if (Player_Input_System.instance.player_input.Player.Grab.IsPressed())
        {
            Ammo ammo = player_aristotle.GetComponentInChildren<Ammo>();
            if(ammo!=null)
            {
                state_machine.Change_State(player_aristotle.grab_state);
            }
        }
        else player_aristotle.crosshair.SetActive(false);

        if (Player_Input_System.instance.player_input.Player.Shoot.WasPerformedThisFrame() && Player_Input_System.instance.On_UI_Button==false)
            state_machine.Change_State(player_aristotle.shoot_state);

        if(Player_Input_System.instance.player_input.Player.Jump.WasPerformedThisFrame())
            state_machine.Change_State(player_aristotle.jump_state);
        
        if (player_aristotle.is_aiming)
            state_machine.Change_State(player_aristotle.aim_state);

        if (player_aristotle.rb.linearVelocity.y < 0 && player_aristotle.Get_Parameter<bool>(Entity_Attribute.is_in_air))
            state_machine.Change_State(player_aristotle.fall_state);
    }

    public override void Exit()
    {
        base.Exit();
    }
}
