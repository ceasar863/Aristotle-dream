using Badtime;
using System.Collections;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public Rigidbody2D rb { get; private set; }
    public SpriteRenderer sr { get; private set; }
    public Animator anim { get; private set; }
    public State_Machine state_machine { get; private set; }

    public LayerMask what_is_ground;

    public float ground_check_distance;
    protected bool is_on_ground;
    protected bool is_in_air;
    protected bool is_dead;
    protected bool is_uping;
    protected bool is_falling;
    protected bool is_facing_right=true;
    protected int facing_dir=1;

    protected Entity_Stats stat;

    [Header("Objects")]
    public GameObject entity_center;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponentInChildren<SpriteRenderer>();
        anim = GetComponentInChildren<Animator>();
        stat = GetComponentInChildren<Entity_Stats>();
        state_machine = new State_Machine();
    }

    protected virtual void Start()
    {
        ground_check_distance=2.2f;
        is_facing_right = true;
        is_on_ground = true;
        is_dead = false;
        is_falling = false;
        is_uping = false;
        facing_dir = 1;
    }

    protected virtual void Update()
    {
        anim.SetFloat("Y_Velocity", rb.linearVelocity.y);
        Update_Falling_And_Uping();

        is_on_ground = Physics2D.Raycast(entity_center.transform.position, Vector2.down, ground_check_distance, what_is_ground);
        is_in_air = !is_on_ground;

        state_machine.Update_State();
    }

    private void Update_Falling_And_Uping()
    {
        if (rb.linearVelocity.y > 0)
        {
            is_uping = true;
            is_falling = false;
        }
        else if (rb.linearVelocity.y < 0)
        {
            is_uping = false;
            is_falling = true;
        }
        else
        {
            is_uping = false;
            is_falling = false;
        }
    }

    public void Flip()
    {
        transform.gameObject.transform.Rotate(0, 180, 0);;
        is_facing_right = !is_facing_right;
        facing_dir = -facing_dir;
    }

    public virtual void Handle_Flip()
    {
        if (is_facing_right && rb.linearVelocity.x < 0)
        {
            Flip();
        }
        else if (!is_facing_right && rb.linearVelocity.x > 0)
        {
            Flip();
        }
    }

    public virtual void OnDrawGizmos()
    {
        //地面检测线
        Gizmos.color = Color.red;
        Vector2 position = entity_center.transform.position;
        Gizmos.DrawLine(position, new Vector2(position.x, position.y - ground_check_distance));

    }

    public virtual void Set_Velocity(float x_velocity, float y_velocity)
    {
        rb.linearVelocity = new Vector2(x_velocity, y_velocity);
        Handle_Flip();
    }

    public virtual void Set_Velocity(float velocity, Vector2 direction)
    {
        rb.linearVelocity = direction * velocity;
        Handle_Flip();
    }

    public virtual void Set_Velocity(float velocity , Vector2 angle , int direction)
    {
        angle.Normalize();
        rb.linearVelocity = new Vector2(angle.x * direction * velocity, angle.y*velocity);
        Handle_Flip();
    }

    public virtual void Set_Velocity_Zero()
    {
        rb.linearVelocity = Vector2.zero;
        Handle_Flip();
    }

    //public virtual void Take_Damage(float damage, GameObject attacker = null)
    //{

    //}

    //public virtual void Die()
    //{

    //    Destroy(gameObject);
    //}

    public virtual void Set_Dead_State(bool flag)
    {
        is_dead = flag;
    }

    public T Get_Parameter<T>(Entity_Attribute tar)
    {
        string target="";
        switch(tar)
        {
            case Entity_Attribute.is_on_ground:
                target = "is_on_ground";
                break;
            case Entity_Attribute.is_in_air:
                target = "is_in_air";
                break;
            case Entity_Attribute.is_dead:
                target = "is_dead";
                break;
            case Entity_Attribute.is_uping:
                target = "is_uping";
                break;
            case Entity_Attribute.is_facing_right:
                target = "is_facing_right";
                break;
            case Entity_Attribute.is_falling:
                target = "is_falling";
                break;
            case Entity_Attribute.facing_dir:
                target = "facing_dir";
                break;
        }

        object value = target switch
        {
            "is_facing_right" => is_facing_right,
            "is_on_ground" => is_on_ground,
            "is_in_air" => is_in_air,
            "facing_dir" => facing_dir,
            "is_dead" => is_dead,
            "is_uping" => is_uping,
            "is_falling" => is_falling,
            _ => null
        };

        if (value == null)
        {
            Debug.LogError("不存在该属性字段 -- " + target);
            return default;
        }

        if (!(value is T))
        {
            Debug.LogError("类型不匹配 -- " + target);
            return default;
        }

        return (T)value;
    }
}

public enum Entity_Attribute
{
    is_on_ground,
    is_in_air,
    is_dead,
    is_uping,
    is_falling,
    is_facing_right,
    facing_dir,
}