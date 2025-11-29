using UnityEngine;

public class Item_Heal_Bottle : Item
{
    public float heal_amount;

    public override void Item_Effect()
    {
        base.Item_Effect();
        Entity_Player_Aristotle.instance.GetComponent<Entity_Health>().Heal(heal_amount);
    }

    public override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
    }
}
