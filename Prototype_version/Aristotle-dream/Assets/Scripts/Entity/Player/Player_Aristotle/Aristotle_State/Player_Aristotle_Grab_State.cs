using UnityEngine;

public class Player_Aristotle_Grab_State : Player_State
{
    private Ammo ammo;

    public Player_Aristotle_Grab_State(Player_Aristotle player_aristotle, State_Machine state_machine, string name) : base(player_aristotle, state_machine, name)
    {

    }

    public override void Enter()
    {
        base.Enter();
        ammo = player_aristotle.GetComponentInChildren<Ammo>();

        entity.anim.SetBool(state_name, false);
    }

    public override void Update()
    {
        base.Update();

        if(ammo.Try_Grab_Monster())
            entity.anim.SetBool(state_name, true); 

        if(trigger_called || !Player_Input_System.instance.player_input.Player.Grab.IsPressed())
            state_machine.Change_State(player_aristotle.idle_state);
    }

    public override void Exit()
    {
        base.Exit();
        ammo.crosshair.SetActive(false);
    }
}
