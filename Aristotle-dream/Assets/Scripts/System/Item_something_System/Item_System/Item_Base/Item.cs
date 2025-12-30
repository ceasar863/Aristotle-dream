using NUnit.Framework;
using System;
using UnityEngine;

public class Item : MonoBehaviour//在游戏场景中实际体化的物体
{
    public Item_Data_So item_data;//物品的基本数据信息
    public int stack_num;//一次会给几个

    protected virtual void Awake()
    {

    }

    protected virtual void Start()
    {

    }

    protected virtual void Update()
    {

    }

    public virtual void Item_Effect()//物品独特效果基本在这里扩展
    {
       
    }

    public Base_Inventory_System Get_Corresponding_Inventory(Item_Type item_type)
    {
        switch(item_type)
        {
            case Item_Type.Weapon:
                return Weapon_Inventory_System.instance;
            case Item_Type.Consumable:
                return Consumable_Inventory_System.instance;
            default:
                throw new Exception("Can't find corresponding inventory!");
        }
    }

    public virtual void OnTriggerEnter2D(Collider2D collision)//物品的拾取判定
    {
        if (collision != null && collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Base_Inventory_System inventory = Get_Corresponding_Inventory(this.item_data.item_type);
            if (inventory == null) throw new Exception("Can't find this inventtory system!");

            inventory.Add_Item_To_Inventory(this);
            Item_Pool_Manager.instance.Recycle_Item_To_Pool(this);
        }
    }
}
