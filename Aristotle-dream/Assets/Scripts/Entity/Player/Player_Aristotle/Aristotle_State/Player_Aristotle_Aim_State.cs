using UnityEngine;

public class Player_Aristotle_Aim_State : Player_State
{
    private int sum_point = 25;
    private float point_interval = 0.07f;
    private Transform[] dots;
    private bool has_dots = false;

    public Player_Aristotle_Aim_State(Entity_Player_Aristotle player_aristotle, State_Machine state_machine, string name) : base(player_aristotle, state_machine, name)
    {
    }

    public override void Enter()
    {
        base.Enter();

        if(has_dots==false)
            dots = Generate_Dots();
    }

    public override void Update()
    {
        base.Update();

        player_aristotle.Handle_Flip();
        Draw_Aim_Line(player_aristotle.shoot_dire);
        
        if (player_aristotle.player_input.Player.Shoot.WasPressedThisFrame() && ammo_system.Get_Current_Bullet()!=null)
            state_machine.Change_State(player_aristotle.shoot_state);

        if (!player_aristotle.is_aiming)
            state_machine.Change_State(player_aristotle.idle_state);
    }

    public override void Exit()
    {
        base.Exit();
        Set_Line_Aable(false);
    }

    #region 关于瞄准线
    private void Draw_Aim_Line(Vector2 direction)
    {
       for(int i=0; i<sum_point; i++)
        {
            var bullet = ammo_system.Get_Current_Bullet();
            if (bullet == null) return;

            dots[i].transform.position = bullet.Predict_Trajectory(direction , point_interval*i);
        }
        Set_Line_Aable(true);
    }

    public void Set_Line_Aable(bool flag)
    {
        foreach (var dot in dots)
            dot.gameObject.SetActive(flag);
    }

    private Transform[] Generate_Dots()
    {
        has_dots = true;
        Transform[] new_dots = new Transform[sum_point];
        for(int i=0; i<sum_point; i++)
        {
            new_dots[i] = GameObject.Instantiate(player_aristotle.aim_line_point , player_aristotle.shoot_point.transform.position , Quaternion.identity).transform;
            new_dots[i].gameObject.SetActive(false);
        }
        return new_dots;
    }
    #endregion
}
