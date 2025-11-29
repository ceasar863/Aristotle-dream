using System;
using UnityEngine;

[Serializable]
public class Item_In_Inventory//插槽内的物品数据
{
    public Item item;//物品
    public int current_num;//当前插槽内叠了几个

    public Item_In_Inventory(Item item, int num)
    {
        this.item = item;
        current_num = num;
    }

    public bool IF_Max(int stack_num)
    {
        if (current_num + stack_num > item.item_data.stack_limit) return true;
        return false;
    }
}
