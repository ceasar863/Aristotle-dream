using UnityEngine;

public class UI_Sorts_Of_Slot : UI_Basic_Slot
{
    protected UI_Child_Slots_Manager slot_manager;
    public Item_Type item_type; 

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

    public virtual void UI_Inventory_Slot_Update()
    {
     
    }

    public virtual void UI_Current_Slot_Update()
    {
       
    }

    public virtual void Show_Child_Slots()
    {
        slot_manager.Switch_Slot_Visibility();
    }
}