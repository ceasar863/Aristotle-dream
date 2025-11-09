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

    public State_Machine state_machine { get; private set; }
    public GameObject ground_check;

    public Vector2 movement_input { get; private set; }

    [Header("Move Details")]
    public float move_speed = 6f;
    public float jump_force = 16f;

    public float ground_check_distance = 1.5f;
    public bool is_facing_right = true;
    public bool is_on_ground = true;
    public bool is_in_air;
    public int facing_dir = 1;
    public LayerMask what_is_ground;

    #region
    public Player_Aristotle_Idle_State idle_state { get; private set; }
    public Player_Aristotle_Move_State move_state { get; private set; }
    public Player_Aristotle_Shoot_State shoot_state { get; private set; }
    public Player_Aristotle_Jump_State jump_state { get; private set; }
    public Player_Aristotle_Fall_State fall_state { get; private set; }
   

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

        state_machine.Initiate();
    }

    private void OnEnable()
    {
        player_input.Enable();
        player_input.Player.Movement.performed += ctx => movement_input = ctx.ReadValue<Vector2>();
        player_input.Player.Movement.canceled += ctx => movement_input = Vector2.zero;

    
    }

    private void Update()
    {
        anim.SetFloat("Y_Velocity", rb.linearVelocity.y);

        is_on_ground = Physics2D.Raycast(ground_check.transform.position, Vector2.down, ground_check_distance, what_is_ground);
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
