using UnityEngine;

public class Enemy : Entity
{
    public Bullet_Sort_List bullet_sort;
    public Bullet bullet;
    public float dead_destroy_time;

    [Header("Move Details")]
    public float move_speed;
    public float idle_time;
    public float combat_time;
    public float combat_chase_speed;

    [Header("Check Boundary")]
    [SerializeField] private float wall_check_distance;
    [SerializeField] private float brim_check_distance;
    [SerializeField] private float check_player_distance;
    [SerializeField] private float enemy_attack_distance;
    

    public RaycastHit2D player_target { get; private set; }
    public bool has_checked_player { get; private set; }
    public bool could_attack;
    public bool next_brim { get; private set; }
    public bool next_wall { get; private set; }


    [Space]
    [SerializeField] private LayerMask what_is_player;
    [SerializeField] private GameObject brim_check_point;
    [SerializeField] private GameObject wall_check_point;


    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
        Try_Detect_Player();
        next_brim = !Physics2D.Raycast(brim_check_point.transform.position, Vector2.down, brim_check_distance, what_is_ground);
        next_wall = Physics2D.Raycast(wall_check_point.transform.position, Vector2.right * facing_dir, wall_check_distance, what_is_ground);
        could_attack = Physics2D.Raycast(transform.position, Vector2.right * facing_dir, enemy_attack_distance, what_is_player);
    }

    private void Try_Detect_Player()
    {
        player_target = Physics2D.Raycast(transform.position, Vector2.right * facing_dir, check_player_distance, what_is_player|what_is_ground);

        if (player_target.collider != null)
            has_checked_player = (1 << player_target.collider.gameObject.layer == what_is_player);
        else has_checked_player = false;
    }

    public virtual Bullet Was_Grabed()
    {
        return bullet;
    }

    public override void Set_Velocity(float x_velocity, float y_velocity)
    {
        base.Set_Velocity(x_velocity, y_velocity);
        Handle_Flip();
    }

    public override void Handle_Flip()
    {
        base.Handle_Flip();
    }

    public virtual float Get_Horizonal_Distance_To_Player(Entity_Player_Aristotle player)
    {
        if (player == null) return Mathf.Infinity;
        return Mathf.Abs(player.transform.position.x - transform.position.x);
    }

    public virtual int Turn_To_Player(Entity_Player_Aristotle player)
    {
        int right = 1, left = -1;
        if (transform.position.x < player.transform.position.x) return right;
        else return left;
    }
    public override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.color = Color.violet;
        Vector2 player_check_end_point = new Vector2(transform.position.x + check_player_distance*facing_dir, transform.position.y);
        Gizmos.DrawLine(transform.position, player_check_end_point);

        Gizmos.color = Color.green;
        //Ç½±Ú¼ì²âÏß
        Vector2 wall_end_point = new Vector2(wall_check_point.transform.position.x + wall_check_distance*facing_dir, wall_check_point.transform.position.y );
        Gizmos.DrawLine(wall_check_point.transform.position , wall_end_point);

        //ÐüÑÂ¼ì²âÏß
        Vector2 brim_end_point = new Vector2(brim_check_point.transform.position.x , brim_check_point.transform.position.y - brim_check_distance);
        Gizmos.DrawLine(brim_check_point.transform.position, brim_end_point);

        Gizmos.color = Color.black;
        //¹¥»÷·¶Î§¼ì²â
        Vector2 attack_end_point = new Vector2(transform.position.x + enemy_attack_distance * facing_dir, transform.position.y);
        Gizmos.DrawLine(transform.position, attack_end_point);
    }

}
