using UnityEngine;

public class UI_Shortcut_Slot : UI_Slot
{
    protected UI_Child_Slots_Manager slot_manager;

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

    public void UI_Inventory_Slot_Update()
    {
        for(int i=0; i<player_inventory.inventory.Length; i++)
        {
            Item_In_Inventory item = player_inventory.inventory[i];
            if (item==null || item.item == null) item = default_item;

            slot_manager.ui_expand_child_slots[i].Update_UI_Slot(item);
        }

    }

    public virtual void UI_Current_Slot_Update()
    {
        if(player_inventory.current_item == null || player_inventory.current_item.current_num==0)
        {
            player_inventory.current_item = null;
            Update_UI_Slot(default_item);
            return;
        }

        interactable_button.image.sprite = player_inventory.current_item.item.item_data.item_icon;
        stack_number_text.text = player_inventory.current_item.current_num.ToString();
    }

    public void Show_Child_Slots()
    {
        slot_manager.Switch_Slot_Visibility();
    }
}
