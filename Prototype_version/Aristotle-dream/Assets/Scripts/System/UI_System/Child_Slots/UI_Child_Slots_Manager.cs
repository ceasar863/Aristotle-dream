using Unity.VisualScripting;
using UnityEngine;

public class UI_Child_Slots_Manager : MonoBehaviour
{
    public UI_Child_Shortcut_Slot[] ui_expand_child_slots;
    private Base_Inventory_System inventory;
    private Item_Type item_type;
    private bool switch_case;

    private void Awake()
    {
        ui_expand_child_slots = GetComponentsInChildren<UI_Child_Shortcut_Slot>(true);
        foreach (UI_Child_Shortcut_Slot child_slot in ui_expand_child_slots)
            child_slot.gameObject.SetActive(true);
    }

    private void Start()
    {
        item_type = GetComponentInParent<UI_Sorts_Of_Slot>(true).item_type;
        inventory = CommonUtils.Get_Sort_Inventory(item_type);

        foreach (UI_Child_Shortcut_Slot child_slot in ui_expand_child_slots)
            child_slot.gameObject.SetActive(false);

        for (int i = 0; i < ui_expand_child_slots.Length; i++)
        {
            ui_expand_child_slots[i].child_slot_index = i;
        }

        switch_case = true;
        Switch_Slot_Visibility();
    }

    public void Switch_Slot_Visibility()
    {
        switch_case = !switch_case;
        for(int i=0; i<inventory.inventory_list.Length ; i++)
        {
            ui_expand_child_slots[i].gameObject.SetActive(switch_case);
        }
    }

    public void Switch_Slot_Visibility(bool flag)
    {
        switch_case = flag;
        for (int i = 0; i < inventory.inventory_list.Length; i++)
        {
            ui_expand_child_slots[i].gameObject.SetActive(flag);
        }
    }
}
