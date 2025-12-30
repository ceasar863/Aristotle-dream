using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Entity_Health : MonoBehaviour,IEntity_Interface
{
    [Header("Health Details")]
    [SerializeField] protected float max_health;
    [SerializeField] protected float current_health;
    [SerializeField] protected float hit_interval;//ÊÜ»÷¼ä¸ô
    protected float last_time_be_hitted;

    public void Start()
    {
        Update_Health(current_health);
        last_time_be_hitted = Time.time;
    }

    private void Update_Health(float new_health)
    {
        float old_health = current_health;
        
        current_health = new_health;
        current_health = Mathf.Clamp(current_health, 0, max_health);

        float old_percent = delta_health_percent(old_health);
        float new_percent = delta_health_percent(new_health);

        if(!Mathf.Approximately(old_percent, new_percent))
        {
            Event_Center.Broad_Cast<float,Transform>(Event_Type.Change_Health_Bar, new_percent , this.transform);
        }
    }


    public float Get_Max_Health()
    {
        return max_health;
    }

    public float Get_Current_Health()
    {
        return current_health;
    }

    public float delta_health_percent(float delta_health)
    {
        return delta_health/max_health;
    }

    public float current_health_percent()
    {
        return current_health / max_health;
    }

    public void Clear_Health()
    {
        Update_Health(0);
    }

    public void Fill_Health()
    {
        Update_Health(max_health);
    }

    public void Suffer_Damage(float damage)
    {
        Modify_Health(-damage);
    }

    public void Heal(float heal)
    {
        Modify_Health(heal);
    }

    public void Modify_Health(float modify)
    {
        float new_health = Mathf.Clamp(current_health+modify , 0 , max_health);
        Update_Health(new_health);
    }

    public bool Could_Be_Hitted()
    {
        return (Time.time > last_time_be_hitted + hit_interval )? true : false;
    }

    public void Take_Damage(float damage, GameObject attacker = null)
    {
        if (Could_Be_Hitted())
        {
            last_time_be_hitted = Time.time;
            Suffer_Damage(damage);
        }
    }

    public void Die()
    {
        Enemy_Pool_Manager.instance.Recycle_Item_To_Pool(this.GetComponent<Enemy>());
    }
}
