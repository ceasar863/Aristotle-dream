using UnityEngine;

public class Enemy : Entity
{
    public Bullet_Sort_List bullet_sort;
    public Bullet bullet;
    public float idle_time;

    [Header("Move Details")]
    public float move_speed;

    [Header("Check Boundary")]
    [SerializeField] private float wall_check_distance;
    [SerializeField] private float brim_check_distance;
    public bool next_brim;
    public bool next_wall;

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
        next_brim = !Physics2D.Raycast(brim_check_point.transform.position, Vector2.down , brim_check_distance , what_is_ground);
        next_wall = Physics2D.Raycast(wall_check_point.transform.position, Vector2.right * facing_dir, wall_check_distance , what_is_ground);
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

    public override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        //Ç½±Ú¼ì²âÏß
        Vector2 wall_end_point = new Vector2(wall_check_point.transform.position.x + wall_check_distance*facing_dir, wall_check_point.transform.position.y );
        Gizmos.DrawLine(wall_check_point.transform.position , wall_end_point);

        //ÐüÑÂ¼ì²âÏß
        Vector2 brim_end_point = new Vector2(brim_check_point.transform.position.x , brim_check_point.transform.position.y - brim_check_distance);
        Gizmos.DrawLine(brim_check_point.transform.position, brim_end_point);
    }

}
