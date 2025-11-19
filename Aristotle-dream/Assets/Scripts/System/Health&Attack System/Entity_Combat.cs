using UnityEngine;

public class Entity_Combat : MonoBehaviour
{
    private void Awake()
    {


    }


    public void Perform_Attack_Circle(float radius, LayerMask what_is_target , float damage , Transform start_point)
    {
        foreach(var target in Get_Targets_Circle(radius , what_is_target , start_point))
        {
            IEntity_Interface entity_interface = target.GetComponent<Entity_Health>();        
            entity_interface.Take_Damage(damage);
        }
    }

    public Collider2D[] Get_Targets_Circle(float radius , LayerMask what_is_target, Transform start_point)
    {
        return Physics2D.OverlapCircleAll(start_point.position , radius , what_is_target);
    }

}
