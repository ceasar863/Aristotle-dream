using Unity.VisualScripting;
using UnityEngine;

public class UI_Child_Slots_Manager : MonoBehaviour
{
    private Inventory_System inventory;
    public UI_Child_Shortcut_Slot[] ui_expand_child_slots;
    public bool switch_case;

    private void Awake()
    {
        ui_expand_child_slots = GetComponentsInChildren<UI_Child_Shortcut_Slot>();
    }
    private void Start()
    {
        inventory = Entity_Player_Aristotle.instance.GetComponent<Inventory_System>();
        for (int i = 0; i < inventory.inventory.Length; i++)
            ui_expand_child_slots[i].child_slot_index = i;
        
        switch_case = true;
        Switch_Slot_Visibility();
    }

    public void Switch_Slot_Visibility()
    {
        switch_case = !switch_case;
        for(int i=0; i<inventory.inventory.Length ; i++)
        {
            ui_expand_child_slots[i].gameObject.SetActive(switch_case);
        }

        //foreach(UI_Child_Shortcut_Slot slot in ui_expand_child_slots)
        //{
        //    slot.gameObject.SetActive(switch_case);
        //}
    }
}
