using System;
using UnityEngine;

public class Weapon_Inventory_System : Base_Inventory_System
{
    public static Weapon_Inventory_System instance;

    protected override void Awake()
    {
        base.Awake();
        Monosingle();
    }

    protected override void Start()
    {
        base.Start();
    }
    protected override void Update()
    {
        //base.Update();
    }

    private void Monosingle()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    /// <summary>
    /// 得到新的武器装备
    /// </summary>
    /// <param name="new_weapon"></param>
    public override void Add_Item_To_Inventory(Item new_weapon)
    {
        for(int i=0; i<inventory_list.Length ; i++)
        {
            Item_In_Inventory weapon = inventory_list[i];
            if(weapon == null || weapon.the_item == null)
            {
                weapon = new Item_In_Inventory(new_weapon,1);
                Debug.Log("Get a new weapon！");
                return;
            }
        }

        Debug.Log("The inventory is full!");
    }

    /// <summary>
    /// 丢弃一把武器，不能是默认的枪
    /// </summary>
    /// <param name="id"> 传入的是背包格子的ID编号 </param>
    public void Discard_One_Weapon(int id)
    {
        //从格子中删除信息:
        Item_In_Inventory weapon = inventory_list[id];
        weapon.the_item = null; weapon.current_num = 0;

        //用对象池在人物附近生成该物体:

    }

    /// <summary>
    /// 装备当前武器，绑定当前武器的所需输入
    /// </summary>
    public void Equip_Current_Weapon()
    {
        Weapon weapon = current_item.the_item as Weapon;
        weapon.Equip();
    }

    /// <summary>
    /// 卸下装备，解除当前装备的输入
    /// </summary>
    public void Unequip_Current_Weapon()
    {

        Weapon weapon = current_item.the_item as Weapon;
        weapon.Unequip();
    }
}
