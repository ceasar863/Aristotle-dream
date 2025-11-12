using UnityEngine;

public class Enemy : Entity
{
    public Bullet_Sort_List bullet_sort;
    public Bullet bullet;

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }

    public virtual Bullet Was_Grabed()
    {
        return bullet;
    }

}
