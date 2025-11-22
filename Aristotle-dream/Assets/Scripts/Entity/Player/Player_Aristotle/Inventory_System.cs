using System;
using UnityEngine;

public class Inventory_System : MonoBehaviour
{
    [Range(1,100)]
    [SerializeField] private int max_storage_num;

    [Header("Inventory Details")]
    public Item_In_Inventory[] inventory;
    public Item_In_Inventory current_item;
    public int current_item_id;


    private void Awake()
    {
        inventory = new Item_In_Inventory[max_storage_num];
    }

    private void Start()
    {
        current_item = inventory[0];
    }

    private void Update()
    {
        if (Entity_Player_Aristotle.instance.player_input.Player.Use_Item.WasPerformedThisFrame())
        {
            Use_Current_Item();
        }
    }

    public bool Inventory_Is_Full()
    {
        bool result = true;
        for(int i=0; i < inventory.Length; i++)
        {
            if(inventory[i] == null || inventory[i].item == null)
            {
                result = false;
            }
            else if (inventory[i].current_num < inventory[i].item.item_data.stack_limit)
            {
                result = false;
            }
        }
        return result;
    }

    public void Add_Item_To_Inventory(Item item , int stack_num)
    {
        for (int i = 0; i < inventory.Length; i++)
        {
            if (inventory[i]==null || inventory[i].item == null)
            {
                inventory[i] = new Item_In_Inventory(item, stack_num);

                Event_Center.Broad_Cast(Event_Type.Update_Item_Slot);
                break;
            }
            else if (inventory[i].item != null && inventory[i].item.item_data.item_name == item.item_data.item_name && !inventory[i].IF_Max(stack_num))
            {
                inventory[i].current_num+=stack_num;

                Event_Center.Broad_Cast(Event_Type.Update_Item_Slot);
                break;
            }
        }
    }
    public void Use_Current_Item()
    {
        if(current_item == null || current_item.item==null)
        {
            Debug.Log("You have no item!!!");
            return;
        }
        current_item.item.Use_Effect();
        Try_Consume_One(current_item.item , current_item_id);
        Event_Center.Broad_Cast(Event_Type.Update_Item_Slot);
    }

   public void Try_Consume_One(Item item)
    {
        for(int i=0; i<inventory.Length; i++)
        {
            if (inventory[i]!=null && inventory[i].item!=null && inventory[i].current_num-1>=1)
            {
                inventory[i].current_num -= 1;
                break;
            }
            else if (inventory[i] != null && inventory[i].item!=null && inventory[i].current_num-1==0)
            {
                inventory[i].item = null;
                inventory[i].current_num = 0;
                Destroy(item.gameObject);
                break;
            }
        }
    }

    public void Try_Consume_One(Item item , int id)
    {
        if (inventory[id] != null && inventory[id].item != null && inventory[id].current_num - 1 >= 1)
        {
            inventory[id].current_num -= 1;
        }
        else if (inventory[id] != null && inventory[id].item != null && inventory[id].current_num - 1 == 0)
        {
            inventory[id].item = null;
            inventory[id].current_num = 0;
            Destroy(item.gameObject);
        }
    }
}