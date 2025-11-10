using UnityEngine;

public class Object_Bullet_Stone : Bullet
{
    public float speel = 10f;
    public Vector2 direction;

    protected override void Awake()
    {
        base.Awake();
        bullet_sort = Bullet_Sort_List.LineBullet;
    }

    protected override void Start()
    {
        Set_Direction(player_aristotle.shoot_dire);
        Set_Bullet_Velocity(player_aristotle.shoot_velocity);

        if (!player_aristotle.is_facing_right) 
            anim.transform.Rotate(0, 180, 0); 
        sr.transform.right = rb.linearVelocity;
    }

    protected override void Update()
    {
        base.Update();
    }

    public override Vector2 Predict_Trajectory(Vector2 direction, float t)
    {
        float distance = player_aristotle.shoot_velocity * t;
        Vector2 next_point = distance * direction.normalized;

        return next_point + (Vector2)player_aristotle.shoot_point.transform.position;
    }
}
