using UnityEngine;

public class Bullet : MonoBehaviour
{
    protected Entity_Player_Aristotle player_aristotle = Entity_Player_Aristotle.instance;
    protected Rigidbody2D rb { get; private set; }
    protected Animator anim { get; private set; }
    protected Collider2D col { get; private set; }
    protected SpriteRenderer sr { get; private set; }
    protected Bullet_Sort_List bullet_sort;

    protected virtual void Awake()
    {
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
}
