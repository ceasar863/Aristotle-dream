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

    public float ground_check_distance = 1.5f;
    protected bool is_facing_right;
    protected bool is_on_ground;
    protected bool is_in_air;
    protected int facing_dir;

    [Header("Objects")]
    public GameObject entity_center;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponentInChildren<SpriteRenderer>();
        anim = GetComponentInChildren<Animator>();
        state_machine = new State_Machine();
    }

    protected virtual void Start()
    {
        ground_check_distance = 1.5f;
        is_facing_right = true;
        is_on_ground = true;
        facing_dir = 1;
    }

    protected virtual void Update()
    {
        anim.SetFloat("Y_Velocity", rb.linearVelocity.y);
        is_on_ground = Physics2D.Raycast(entity_center.transform.position, Vector2.down, ground_check_distance, what_is_ground);
        is_in_air = !is_on_ground;

        state_machine.Update_State();
    }

    protected void Flip()
    {
        anim.transform.Rotate(0, 180, 0);
    }

    public virtual void Handle_Flip()
    {
        if (is_facing_right && rb.linearVelocity.x < 0)
        {
            is_facing_right = false;
            facing_dir = -facing_dir;
            Flip();
        }
        else if (!is_facing_right && rb.linearVelocity.x > 0)
        {
            is_facing_right = true;
            facing_dir = -facing_dir;
            Flip();
        }
    }

    public virtual void OnDrawGizmos()
    {
        //地面检测线
        Vector3 position = entity_center.transform.position;
        Gizmos.DrawLine(position, new Vector3(position.x, position.y - ground_check_distance, position.z));

    }

    public T Get_Parameter<T>(string target)
    {
        object value = target switch
        {
            "is_facing_right" => is_facing_right,
            "is_on_ground" => is_on_ground,
            "is_in_air" => is_in_air,
            "facing_dir" => facing_dir,
            _ => null
        };

        if(value == null)
        {
            Debug.LogError("不存在该属性字段 -- " + target);
            return default;
        }

        if(!(value is T))
        {
            Debug.LogError("类型不匹配 -- " + target);
            return default;
        }

        return (T)value;
    }
}
