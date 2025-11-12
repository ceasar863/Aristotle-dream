using UnityEngine;

public class Bullet : MonoBehaviour
{
    protected Entity_Player_Aristotle player_aristotle;
    protected Rigidbody2D rb { get; private set; }
    protected Animator anim { get; private set; }
    protected Collider2D col { get; private set; }
    protected SpriteRenderer sr { get; private set; }
    protected Bullet_Sort_List bullet_sort;
    protected float timer;

    [Header("Reinforce Parament")]
    [SerializeField] protected float speed;
    [SerializeField] protected float speed_reinforce_rate;
    [SerializeField] protected float max_speed;
    [SerializeField] protected float reinforce_interval;

    protected virtual void Awake()
    {
        player_aristotle = Entity_Player_Aristotle.instance;
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        anim = GetComponentInChildren<Animator>();
        sr = GetComponentInChildren<SpriteRenderer>();
    }

    protected virtual void Start()
    {
        
    }

    protected virtual void Update()
    {
        //检测飞行途中是否有碰撞
        //如果飞出一定范围，那么就销毁子弹
    }

    public virtual void Set_Parameter()//设置子弹射出时候的相关参数
    {
        Set_Direction(player_aristotle.shoot_dire);
        Set_Bullet_Velocity(speed);

        if (!player_aristotle.Get_Parameter<bool>("is_facing_right"))
            anim.transform.Rotate(0, 180, 0);
        sr.transform.right = rb.linearVelocity;
    }

    protected virtual void Set_Direction(Vector2 dire)
    {
        rb.linearVelocity = dire.normalized;
    }

    protected virtual void Set_Bullet_Velocity(float velocity)
    {
        rb.linearVelocity *= velocity;
    }

    public virtual Vector2 Predict_Trajectory(Vector2 direction, float t)
    {
        return Vector2.zero;
    }

    public virtual void Reinforce()//设置强化机制
    {
      

    }

    public virtual void Characsteristic()//设置子弹特性
    {

    }
}
