using UnityEngine;

public class UI_Child_Shortcut_Slot : UI_Slot
{
    protected UI_Shortcut_Slot shortcut_slot;
    public int child_slot_index;

    protected override void Awake()
    {
        base.Awake();
        shortcut_slot = GetComponentInParent<UI_Shortcut_Slot>();
    }

    protected override void Start()
    {
        base.Start();
    }

    public void Change_Shortcut_Slot()
    {
        player_inventory.current_item = this.item;
        player_inventory.current_item_id = child_slot_index;
        shortcut_slot.Update_UI_Slot(item);
    }
}
