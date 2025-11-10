using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class Entity_Player_Aristotle : MonoBehaviour
{
    public static Entity_Player_Aristotle instance { get; private set; }
    public Rigidbody2D rb { get; private set; }
    public SpriteRenderer sr { get; private set; }
    public Animator anim { get; private set; }
    public Player_Input player_input { get; private set; }
    public Vector2 movement_input { get; private set; }
    public State_Machine state_machine { get; private set; }

    [Header("Objects")]
    public GameObject ground_check;
    public GameObject shoot_point;
    public GameObject aim_line_point;

    [Header("Bullet Details")]
    public GameObject bullet_prefab;


    [Header("Move Details")]
    public float move_speed = 6f;
    public float jump_force = 16f;


    [Header("Combat Details")]
    public Camera main_camera;
    public Vector2 shoot_dire;
    public float shoot_velocity;

    public LayerMask what_is_ground;
    public float ground_check_distance = 1.5f;
    public bool is_facing_right { get; private set; }
    public bool is_on_ground { get; private set; }
    public bool is_in_air { get; private set; }
    public bool is_aiming { get; private set; }
    public int facing_dir { get; private set; }
    

    #region
    public Player_Aristotle_Idle_State idle_state { get; private set; }
    public Player_Aristotle_Move_State move_state { get; private set; }
    public Player_Aristotle_Shoot_State shoot_state { get; private set; }
    public Player_Aristotle_Jump_State jump_state { get; private set; }
    public Player_Aristotle_Fall_State fall_state { get; private set; }
    public Player_Aristotle_Aim_State aim_state { get; private set; }

    #endregion
    private void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }

        rb = GetComponent<Rigidbody2D>();
        sr = GetComponentInChildren<SpriteRenderer>();
        anim = GetComponentInChildren<Animator>();
        state_machine = new State_Machine();
        player_input = new Player_Input();
    }

    private void Start()
    {
        idle_state = new Player_Aristotle_Idle_State(this, state_machine, "Idle");
        move_state = new Player_Aristotle_Move_State(this, state_machine, "Move");
        shoot_state = new Player_Aristotle_Shoot_State(this, state_machine, "Shoot");
        jump_state = new Player_Aristotle_Jump_State(this, state_machine, "Jump_Fall");
        fall_state = new Player_Aristotle_Fall_State(this, state_machine, "Jump_Fall");
        aim_state = new Player_Aristotle_Aim_State(this, state_machine, "Aim");

        ground_check_distance = 1.5f;
        is_facing_right = true;
        is_on_ground = true;
        facing_dir = 1;

    state_machine.Initiate();
    }

    private void OnEnable()
    {
        player_input.Enable();
        player_input.Player.Movement.performed += ctx => movement_input = ctx.ReadValue<Vector2>();
        player_input.Player.Movement.canceled += ctx => movement_input = Vector2.zero;

        player_input.Player.Aim.performed += ctx => is_aiming = true;
        player_input.Player.Aim.canceled += ctx => is_aiming = false;

    }

    private void Update()
    {
        anim.SetFloat("Y_Velocity", rb.linearVelocity.y);

        is_on_ground = Physics2D.Raycast(ground_check.transform.position, Vector2.down, ground_check_distance, what_is_ground);
        shoot_dire = main_camera.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        is_in_air = !is_on_ground;
        
        state_machine.Update_State();
    }

    public void Set_Velocity(float x_velocity , float y_velocity)
    {
        Handle_Flip();
        rb.linearVelocity = new Vector2(x_velocity * facing_dir, y_velocity);
    }

    public void Handle_Flip()
    {
        if(is_facing_right && movement_input.x<0)
        {
            is_facing_right = false;
            facing_dir = -facing_dir;
            Flip();
        }
        else if (!is_facing_right && movement_input.x > 0)
        {
            is_facing_right = true;
            facing_dir = -facing_dir;
            Flip();
        }

        if (state_machine.current_state == aim_state)
        {
            if (is_facing_right && shoot_dire.x < 0)
            {
                is_facing_right = false;
                facing_dir = -facing_dir;
                Flip();
            }
            else if (!is_facing_right && shoot_dire.x > 0)
            {
                is_facing_right = true;
                facing_dir = -facing_dir;
                Flip();
            }
        }
    }

    private void Flip()
    {
        anim.transform.Rotate(0, 180, 0);
    }

    public void OnDrawGizmos()
    {
        Vector3 position = ground_check.transform.position;
        Gizmos.DrawLine(position, new Vector3(position.x, position.y - ground_check_distance, position.z));
    }
}
