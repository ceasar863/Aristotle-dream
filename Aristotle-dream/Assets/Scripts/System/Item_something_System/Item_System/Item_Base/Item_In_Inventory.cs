using System;
using UnityEngine;

[Serializable]
public class Item_In_Inventory//插槽内的物品数据
{
    public Item the_item;//物品
    public int current_num;//当前插槽内叠了几个

    public Item_In_Inventory(Item item, int num)
    {
        this.the_item = item;
        current_num = num;
    }

    public bool IF_Max(out int gap)
    {
        if (current_num == the_item.item_data.stack_limit)
        {
            gap = 0;
            return true;
        }
        else
        {
            gap = the_item.item_data.stack_limit - current_num;
            return false;
        }
    }
}
