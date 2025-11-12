using UnityEngine;

public class Object_Bullet_Small_Stone : Bullet
{
    private float speed_rate;

    protected override void Awake()
    {
        base.Awake();
        bullet_sort = Bullet_Sort_List.ArcBullet;
        speed_rate = speed_reinforce_rate;
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
        Vector2 initial_velocity = direction*speed;
        Vector2 gravity_effect = 0.5f * Physics2D.gravity * rb.gravityScale * (t * t);
        Vector2 predicted_point = (initial_velocity * t) + gravity_effect;

        return predicted_point + (Vector2)player_aristotle.shoot_point.transform.position;
    }

    public override void Reinforce()
    {
        float delta_time = Time.deltaTime;
        timer += delta_time;

        if (timer > reinforce_interval)
        {
            float new_speed = speed * (1 + speed_rate);
            speed = Mathf.Clamp(new_speed, 0, max_speed);
            speed_rate *= 0.95f;
            timer = 0;
        }
    }


}
