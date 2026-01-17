using Badtime;
using System;
using UnityEngine;

public class Weapon_Inventory_System : Base_Inventory_System
{
    public static Weapon_Inventory_System instance;
    private Weapon_Generator generator;//获取到武器生成器

    protected override void Awake()
    {
        base.Awake();
        Monosingle();
        generator = FindFirstObjectByType(typeof(Weapon_Generator)) as Weapon_Generator;
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
            if(inventory_list[i] == null || inventory_list[i].the_item == null)
            {
                inventory_list[i] = new Item_In_Inventory(new_weapon,1);
                Debug.Log("Get a new weapon！");
                Event_Center.Broad_Cast(Event_Type.Update_Weapon_Slot);
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
        Event_Center.Broad_Cast(Event_Type.Update_Weapon_Slot);
        //用对象池在人物附近生成该物体:

    }
    
    public void Change_Weapon(string weapon_name)
    {
        Weapon_Data_So data = Weapon_Storage.instance.Get_Weapon_Data(weapon_name);
        if(data!=null) generator.Change_Weapon(data);
    }
}
