using Badtime;
using UnityEngine;

public class UI_Child_Shortcut_Slot : UI_Basic_Slot
{
    protected UI_Sorts_Of_Slot sort_of_slot;//各自父格子的信息

    [Space(10)]
    public int child_slot_index;

    protected override void Awake()
    {
        base.Awake();
        sort_of_slot = GetComponentInParent<UI_Sorts_Of_Slot>();
    }

    protected override void Start()
    {
        base.Start();
    }

    public void Change_Consumable_Slot() => Change_Shortcut_Slot(Sort_Of_Slot_Enum.Consumable);
    public void Change_Weapon_Slot() => Change_Shortcut_Slot(Sort_Of_Slot_Enum.Weapon);
    private void Change_Shortcut_Slot(Sort_Of_Slot_Enum sort)//根据类型把当前的物品进行替换
    {
        if (sort == Sort_Of_Slot_Enum.Consumable)
        {
            consumable_inventory.current_item = this.item_in_slot;
            consumable_inventory.current_item_slot_id = child_slot_index;
            sort_of_slot.Update_UI_Slot(item_in_slot);//让父各更新自己的UI
        }
        else if(sort == Sort_Of_Slot_Enum.Weapon)
        {
            weapon_inventory.current_item = this.item_in_slot;
            weapon_inventory.current_item_slot_id = child_slot_index;
            sort_of_slot.Update_UI_Slot(item_in_slot);

            //让Weapon_Inventory去做数据层面的更改
            weapon_inventory.Change_Weapon(weapon_inventory.current_item.the_item.item_data.item_name);
        }
    }
}
