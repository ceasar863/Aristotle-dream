using UnityEngine;

public class Item : MonoBehaviour
{
    public Item_Data_So item_data;
    public int stack_num;

    public virtual void Use_Effect()
    {
        Debug.Log("Use the item!");
    }

    public virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null && collision.gameObject.layer == LayerMask.NameToLayer("Player") && !Entity_Player_Aristotle.instance.GetComponent<Inventory_System>().Inventory_Is_Full())
        {
            collision.gameObject.GetComponent<Inventory_System>().Add_Item_To_Inventory(this, stack_num);
            gameObject.SetActive(false);
        }
    }
}
