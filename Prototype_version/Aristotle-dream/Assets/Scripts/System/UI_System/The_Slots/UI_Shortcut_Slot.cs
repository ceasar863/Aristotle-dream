using UnityEngine;

public class UI_Shortcut_Slot : UI_Sorts_Of_Slot
{
    protected override void Awake()
    {
        base.Awake();
        slot_manager = GetComponentInChildren<UI_Child_Slots_Manager>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        Event_Center.Add_Listener(Event_Type.Update_Item_Slot, UI_Current_Slot_Update);
        Event_Center.Add_Listener(Event_Type.Update_Item_Slot, UI_Inventory_Slot_Update);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        Event_Center.Remove_Listener(Event_Type.Update_Item_Slot, UI_Current_Slot_Update);
        Event_Center.Remove_Listener(Event_Type.Update_Item_Slot, UI_Inventory_Slot_Update);
    }

    protected override void Start()
    {
        base.Start();
        Event_Center.Broad_Cast(Event_Type.Update_Item_Slot);
    }

    public override void UI_Inventory_Slot_Update()
    {
        for(int i=0; i<consumable_inventory.inventory_list.Length; i++)
        {
            Item_In_Inventory item = consumable_inventory.inventory_list[i];
            if (item==null || item.the_item == null) item = default_item;

            slot_manager.ui_expand_child_slots[i].Update_UI_Slot(item);
        }
    }

    public override void UI_Current_Slot_Update()
    {
        if (consumable_inventory.current_item == null || consumable_inventory.current_item.current_num == 0)
        {
            consumable_inventory.current_item = null;
            Update_UI_Slot(default_item);
            return;
        }

        interactable_button.image.sprite = consumable_inventory.current_item.the_item.item_data.item_icon;
        stack_number_text.text = consumable_inventory.current_item.current_num.ToString();
    }
}
