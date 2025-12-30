using System;
using UnityEngine;

public class Consumable_Inventory_System : Base_Inventory_System
{
    public static Consumable_Inventory_System instance; 

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
        base.Update();
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

    public override bool Inventory_Is_Full()
    {
        return base.Inventory_Is_Full();
    }

    public override void Add_Item_To_Inventory(Item item)
    {
        base.Add_Item_To_Inventory(item);

        int sum = item.stack_num , single_limit = item.item_data.stack_limit;
        bool have_update = false;

        for (int i = 0; i < inventory_list.Length; i++)
        {
            if (sum==0) break;

            if (inventory_list[i]==null || inventory_list[i].the_item == null)
            {
                int real_count = sum > single_limit ? single_limit : sum;

                inventory_list[i] = new Item_In_Inventory(item , real_count);
                sum -= real_count;
                have_update = true;
            }
            else if (inventory_list[i].the_item != null && inventory_list[i].the_item.item_data.item_name == item.item_data.item_name && !inventory_list[i].IF_Max(out int gap))
            {
                int real_count = sum > gap ? gap : sum;
                inventory_list[i].current_num+=real_count;
                sum -= real_count;
                have_update = true;
            }
        }

        if(have_update) 
            Event_Center.Broad_Cast(Event_Type.Update_Item_Slot);
    }

    public override void Use_Current_Item()
    {
        base.Use_Current_Item();
        if(current_item == null || current_item.the_item==null)
        {
            Debug.Log("You have no item!!!");
            return;
        }
        current_item.the_item.Item_Effect();
        Try_Consume_One(current_item.the_item , current_item_slot_id);
        
        Event_Center.Broad_Cast(Event_Type.Update_Item_Slot);
    }

    public override void Try_Consume_One(Item item , int id)
    {
        base.Try_Consume_One(item,id);
        if (inventory_list[id] != null && inventory_list[id].the_item != null && inventory_list[id].current_num - 1 >= 1)
        {
            inventory_list[id].current_num -= 1;
        }
        else if (inventory_list[id] != null && inventory_list[id].the_item != null && inventory_list[id].current_num - 1 == 0)
        {
            inventory_list[id].the_item = null;
            inventory_list[id].current_num = 0;
        }
    }
}