using UnityEngine;

public class Object_Bullet_Stone : Bullet
{
    protected override void Awake()
    {
        base.Awake();
        bullet_sort = Bullet_Sort_List.LineBullet;

    }

    protected override void Start()
    {
        
    }

    protected override void Update()
    {
        base.Update();
    }

    public override void Set_Parameter()
    {
        base.Set_Parameter();
        
    }


    public override Vector2 Predict_Trajectory(Vector2 direction, float t)
    {
        float distance = speed * t;

        Vector2 next_point = distance * direction.normalized;
        return next_point + (Vector2)player_aristotle.shoot_point.transform.position;
    }

    public override void Reinforce()
    {
        float delta_time = Time.deltaTime;
        timer += delta_time;

        if (timer > reinforce_interval)
        {
            float new_speed = speed * (1 + speed_reinforce_rate);
            speed = Mathf.Clamp(new_speed, 0, max_speed);
            timer = 0;
        }
    }
}
