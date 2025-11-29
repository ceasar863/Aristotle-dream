using NUnit.Framework;
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

    protected Collider2D[] targets_be_hitted;

    [Header("Damage Details")]
    [SerializeField] protected int max_num_coulde_hit;
    [SerializeField] protected float damage;
    [SerializeField] protected float max_distance;

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
        targets_be_hitted = new Collider2D[max_num_coulde_hit];
    }

    protected virtual void Update()
    {
        if(Get_Enemies_Around()>0)//检测飞行途中是否有碰撞
        {
            foreach(Collider2D target in targets_be_hitted)
            {
                if (target == null) break;
                IEntity_Interface entity_interface = target.GetComponent<Entity_Health>();
                entity_interface.Take_Damage(damage);
            }

            this.col.enabled = false;
        }

        if(Vector2.Distance(transform.position , player_aristotle.transform.position)>max_distance)//如果飞出一定范围，那么就销毁子弹
        {
            Debug.Log("已经销毁飞出最大距离的子弹");
            Bullet_Pool_Manager.instance.Recycle_Item_To_Pool(this);
        }
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

    protected int Get_Enemies_Around()
    {
        if(targets_be_hitted==null)
            targets_be_hitted = new Collider2D[max_num_coulde_hit];

        ContactFilter2D contact_fliter = new ContactFilter2D();

        contact_fliter.useLayerMask = true;  // 启用层级过滤
        contact_fliter.useTriggers = true;   // 只检测Trigger
        contact_fliter.layerMask = player_aristotle.what_is_shoot_target; // 只检测敌人层

        return Physics2D.OverlapCollider(col, contact_fliter , targets_be_hitted);
    }

    public virtual void Reinforce()//设置强化机制
    {
      

    }

    public virtual void Bullet_Effect()//设置子弹特性
    {

    }

}
