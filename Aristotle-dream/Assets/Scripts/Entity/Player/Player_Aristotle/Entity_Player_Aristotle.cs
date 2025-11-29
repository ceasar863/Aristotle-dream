using NUnit.Framework;
using System;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class Entity_Player_Aristotle : Entity
{
    public static Entity_Player_Aristotle instance { get; private set; }
    public Player_Input player_input { get; private set; }
    public Vector2 movement_input { get; private set; }
    public Ammo_System  ammo_system {get;private set;}

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

    public bool is_aiming { get; private set; }
    
    #region//注册状态
    public Player_Aristotle_Idle_State idle_state { get; private set; }
    public Player_Aristotle_Move_State move_state { get; private set; }
    public Player_Aristotle_Shoot_State shoot_state { get; private set; }
    public Player_Aristotle_Jump_State jump_state { get; private set; }
    public Player_Aristotle_Fall_State fall_state { get; private set; }
    public Player_Aristotle_Aim_State aim_state { get; private set; }
    public Player_Aristotle_Grab_State grab_state { get; private set; }
    public Player_Aristotle_Run_State run_state { get; private set; }
    public Player_Aristotle_Dead_State dead_state { get; private set; }

    #endregion

    protected override void Awake()
    {
        base.Awake();

        if(instance != null && instance != this)//玩家单例
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }

        camera_follow_effect = FindFirstObjectByType<Camera_Follow_Effect>();
        ammo_system = GetComponent<Ammo_System>();
        player_input = new Player_Input();
        
    }

    protected override void Start()
    {
        base.Start();
        entity_center.GetComponent<SpriteRenderer>().enabled = false;

        idle_state = new Player_Aristotle_Idle_State(this, state_machine, "Idle");
        move_state = new Player_Aristotle_Move_State(this, state_machine, "Move");
        shoot_state = new Player_Aristotle_Shoot_State(this, state_machine, "Shoot");
        jump_state = new Player_Aristotle_Jump_State(this, state_machine, "Jump_Fall");
        fall_state = new Player_Aristotle_Fall_State(this, state_machine, "Jump_Fall");
        aim_state = new Player_Aristotle_Aim_State(this, state_machine, "Aim");
        grab_state = new Player_Aristotle_Grab_State(this, state_machine, "Grab");
        run_state = new Player_Aristotle_Run_State(this, state_machine, "Run");
        dead_state = new Player_Aristotle_Dead_State(this, state_machine, "Dead");

        crosshair = GameObject.Instantiate(ammo_system.crosshair);
        crosshair.gameObject.SetActive(false);

        state_machine.Initiate(idle_state);
    }

    private void OnEnable()
    {
        player_input.Enable();
        player_input.Player.Movement.performed += ctx => movement_input = ctx.ReadValue<Vector2>();
        player_input.Player.Movement.canceled += ctx => movement_input = Vector2.zero;

        player_input.Player.Aim.performed += ctx => is_aiming = true;
        player_input.Player.Aim.canceled += ctx => is_aiming = false;

        player_input.Player.Grab.performed += ctx => Set_Grab_Domain_Visible(true);
        player_input.Player.Grab.canceled += ctx => Set_Grab_Domain_Visible(false);
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

    private Vector3 Direction_To_Mouse()
    {
        Vector3 player_position = entity_center.transform.position;
        Vector3 direction = world_mouse_position - player_position;
        return direction.normalized;
    }
    #endregion


    public override void Set_Velocity(float x_velocity , float y_velocity)
    {
        Handle_Flip();
        rb.linearVelocity = new Vector2(x_velocity, y_velocity);
    }

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

    public void Try_Grab_Monster()
    {
        if (ammo_system.Get_Current_Bullet() != null)
        {
            Debug.Log("弹匣内已有子弹");
            return;
        }

        bool target_valid = false;
        RaycastHit2D target = Physics2D.Raycast(entity_center.transform.position, Direction_To_Mouse(), grab_radius, what_is_grab_target|what_is_ground);


        if (target.collider != null)
        {
            if ((1 << target.collider.gameObject.layer) != what_is_ground)
            {
                crosshair.transform.position = target.collider.transform.position;
                target_valid = true;
            }
        }

        if (target_valid) crosshair.SetActive(true);
        else crosshair.SetActive(false);

        if (player_input.Player.Shoot.WasPressedThisFrame())
        {
            if (target.collider == null)
            {
                Debug.Log("无可抓取目标");
                return;
            }

            if ((1 << target.collider.gameObject.layer) == what_is_ground)
            {
                Debug.Log("中间有障碍物！");
                return;
            }

            //进入抓捕状态
            state_machine.Change_State(grab_state);

            //成功抓取转化为弹药补给给玩家
            //Bullet bullet = Bullet_Pool_Manager.instance.Spawn_Item_Object() 
            GameObject bullet = target.collider.gameObject.GetComponentInParent<Enemy>().Was_Grabed();
            ammo_system.Set_Bullet(bullet);

            //被捕获后销毁目标物体
            Destroy(target.collider.GetComponentInParent<Enemy>().gameObject);
            crosshair.SetActive(false);
        }
    }

    private void Set_Grab_Domain_Visible(bool flag)
    {
        entity_center.GetComponent<SpriteRenderer>().enabled = flag;
    }

    public override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        //抓取范围的可视化
        //Gizmos.color = Color.blue;
        //Vector3 position = entity_center.transform.position;
        //Gizmos.DrawWireSphere(position , grab_radius);
    }
}
