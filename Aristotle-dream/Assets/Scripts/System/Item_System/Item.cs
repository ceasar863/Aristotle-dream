using UnityEngine;

public class Item : MonoBehaviour//实际体化物体的数据
{
    public Item_Data_So item_data;//物品的基本数据信息
    public int stack_num;//一次会给几个

    public virtual void Use_Effect()//物品的使用效果基本在这里扩展
    {
        Debug.Log("Use the item!");
    }

    public virtual void OnTriggerEnter2D(Collider2D collision)//物品的拾取判定
    {
        if (collision != null && collision.gameObject.layer == LayerMask.NameToLayer("Player") && !Entity_Player_Aristotle.instance.GetComponent<Inventory_System>().Inventory_Is_Full())
        {
            collision.gameObject.GetComponent<Inventory_System>().Add_Item_To_Inventory(this, stack_num);
            gameObject.SetActive(false);
        }
    }
}
