using UnityEngine;

public class UI_Weapon_Slot : UI_Sorts_Of_Slot
{
    protected override void Awake()
    {
        base.Awake();
        slot_manager = GetComponentInChildren<UI_Child_Slots_Manager>();
    }
    protected override void Start()
    {
        base.Start();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
    }

    protected override void OnDisable()
    {
        base.OnDisable();
    }

    public override void UI_Inventory_Slot_Update()
    {
        for (int i = 0; i < weapon_inventory.inventory_list.Length; i++)
        {
            Item_In_Inventory item = weapon_inventory.inventory_list[i];
            if (item == null || item.the_item == null) item = default_item;

            slot_manager.ui_expand_child_slots[i].Update_UI_Slot(item);
        }
    }

    public override void UI_Current_Slot_Update()
    {
        if (weapon_inventory.current_item == null || weapon_inventory.current_item.current_num == 0)
        {
            weapon_inventory.current_item = null;
            Update_UI_Slot(default_item);
            return;
        }

        interactable_button.image.sprite = weapon_inventory.current_item.the_item.item_data.item_icon;
        stack_number_text.text = weapon_inventory.current_item.current_num.ToString();
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
    } 

    protected override void OnTriggerExit2D(Collider2D collision)
    {
        base.OnTriggerExit2D(collision);
    }
    public override void Show_Child_Slots()
    {
        slot_manager.Switch_Slot_Visibility();
    }
}
