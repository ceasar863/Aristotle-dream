using JetBrains.Annotations;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.UI;

public class Player_Aristotle : Entity
{
    public static Player_Aristotle instance { get; private set; }
    public Vector2 movement_input { get; private set; }
    public Ammo  ammo_system {get;private set;}

    [Header("Objects")]
    public GameObject aim_line_point;

    [Header("Move Details")]
    public float move_speed = 6f;
    public float jump_force = 16f;
    public float run_speed = 22.5f;

    [Header("Grab Details")]
    public float grab_radius;
    public LayerMask what_is_grab_target;
    public GameObject crosshair;

    [Header("Shoot Details")]
    public LayerMask what_is_shoot_target;
    public GameObject shoot_point;
    public Vector2 shoot_dire;

    public Vector3 world_mouse_position;
    public Camera main_camera;
    private Camera_Follow_Effect camera_follow_effect;
    public bool is_aiming;

    #region
    private Weapon primary_weapon;
    private Weapon secondary_weapon;
    #endregion

    #region//注册状态
    /*----------------------------人物行动相关状态-----------------------------------*/
    public Player_Aristotle_Idle_State idle_state { get; private set; }
    public Player_Aristotle_Move_State move_state { get; private set; }
    public Player_Aristotle_Jump_State jump_state { get; private set; }
    public Player_Aristotle_Fall_State fall_state { get; private set; }
    public Player_Aristotle_Run_State run_state { get; private set; }
    public Player_Aristotle_Dead_State dead_state { get; private set; }

    /*----------------------------发射器相关状态-----------------------------------*/
    public Player_Aristotle_Shoot_State shoot_state { get; private set; }
    public Player_Aristotle_Auxiliary_Shoot_State auxiliary_shoot_state { get; private set; }
    public Player_Aristotle_Aim_State aim_state { get; private set; }
    public Player_Aristotle_Grab_State grab_state { get; private set; }

    #endregion

    protected override void Awake()
    {
        base.Awake();
        Made_As_Single();

        primary_weapon = transform.Find("Primary_Weapon").GetComponent<Weapon>();
        secondary_weapon = transform.Find("Secondary_Weapon").GetComponent<Weapon>();

        camera_follow_effect = FindFirstObjectByType<Camera_Follow_Effect>();
        Player_Input_System.instance.Load_Player_Input_System();
    }

    private void Made_As_Single()//因为继承了Entity的原因，所以单独写个方法做单例吧 :D
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    protected override void Start()
    {
        base.Start();
        entity_center.GetComponent<SpriteRenderer>().enabled = false;

        idle_state = new Player_Aristotle_Idle_State(this, state_machine, "Idle");
        move_state = new Player_Aristotle_Move_State(this, state_machine, "Move");
        shoot_state = new Player_Aristotle_Shoot_State(this, state_machine, "Shoot" , primary_weapon);
        auxiliary_shoot_state = new Player_Aristotle_Auxiliary_Shoot_State(this , state_machine , "Shoot" , secondary_weapon);
        jump_state = new Player_Aristotle_Jump_State(this, state_machine, "Jump_Fall");
        fall_state = new Player_Aristotle_Fall_State(this, state_machine, "Jump_Fall");
        aim_state = new Player_Aristotle_Aim_State(this, state_machine, "Aim");
        grab_state = new Player_Aristotle_Grab_State(this, state_machine, "Grab");
        run_state = new Player_Aristotle_Run_State(this, state_machine, "Run");
        dead_state = new Player_Aristotle_Dead_State(this, state_machine, "Dead");

        state_machine.Initiate(idle_state);
    }

    private void OnEnable()
    {
        {
            Player_Input_System.instance.Bind_Key_Map(Operation_Mapping_Enum.Movement, Map_Type_Enum.Performed, Perform_Move_Input);
            Player_Input_System.instance.Bind_Key_Map(Operation_Mapping_Enum.Movement, Map_Type_Enum.Cancel, Cancel_Move_Input);
        }
    }


    #region 鼠标和摄像机相关
    protected override void Update()
    {
        Update_World_Position();
        shoot_dire = Direction_To_Mouse();

        if (is_falling) camera_follow_effect.Falling_And_Uping_Adjust(true);
        else if (is_uping) camera_follow_effect.Falling_And_Uping_Adjust(false);

        base.Update();
    }

    private void Update_World_Position()
    {
        Vector3 test_position = Input.mousePosition;
        test_position.z = 10;
        world_mouse_position = main_camera.ScreenToWorldPoint(test_position);
    }

    public Vector3 Direction_To_Mouse()
    {
        Vector3 player_position = entity_center.transform.position;
        Vector3 direction = world_mouse_position - player_position;
        return direction.normalized;
    }
    #endregion


    #region 设置速度相关
    public override void Set_Velocity(float x_velocity , float y_velocity)
    {
        rb.linearVelocity = new Vector2(x_velocity, y_velocity);
        Handle_Flip();
    }

    public override void Set_Velocity(float velocity , Vector2 direction)//指定方向的设定速度
    {
        base.Set_Velocity(velocity, direction);
    }

    public override void Set_Velocity(float velocity , Vector2 angle , int direction)//设定特定角度的速度
    {
        base.Set_Velocity(velocity , angle , direction);
    }

    public override void Set_Velocity_Zero()
    {
        base.Set_Velocity_Zero();
    }
    #endregion

    public override void Handle_Flip()
    {
        if (is_facing_right && rb.linearVelocity.x < 0)
        {
            Flip();
            camera_follow_effect.Start_Follow();
        }
        else if (!is_facing_right && rb.linearVelocity.x > 0)
        {
            Flip();
            camera_follow_effect.Start_Follow();
        }

        if (state_machine.current_state == aim_state)
        {
            if (is_facing_right && shoot_dire.x < 0) Flip();
            else if (!is_facing_right && shoot_dire.x > 0) Flip();
        }
    }

    public override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        //抓取范围的可视化
        //Gizmos.color = Color.blue;
        //Vector3 position = entity_center.transform.position;
        //Gizmos.DrawWireSphere(position , grab_radius);
    }

    #region 玩家的输入操作相关的函数
    private void Perform_Move_Input(InputAction.CallbackContext ctx)
    {
        movement_input = ctx.ReadValue<Vector2>();
    }

    private void Cancel_Move_Input(InputAction.CallbackContext ctx)
    {
        movement_input = Vector2.zero;
    }
    #endregion
}
