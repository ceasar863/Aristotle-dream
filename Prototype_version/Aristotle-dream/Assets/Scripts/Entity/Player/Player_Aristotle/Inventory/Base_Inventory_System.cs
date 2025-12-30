using UnityEngine;

public class Base_Inventory_System : MonoBehaviour
{
    [Range(1, 100)]
    [SerializeField] protected int max_storage_num;

    [Header("Inventory Details")]
    public Item_In_Inventory[] inventory_list;
    public Item_In_Inventory current_item;
    public int current_item_slot_id;//当前物品在物品栏的位置

    protected virtual void Awake()
    {
        inventory_list = new Item_In_Inventory[max_storage_num];
    }

    protected virtual void Start()
    {
        current_item = inventory_list[0];
        current_item_slot_id = 0;
    }

    protected virtual void Update()
    {
        if (Player_Input_System.instance.player_input.Player.Use_Item.WasPerformedThisFrame())
        {
            Use_Current_Item();
        }
    }

    public virtual bool Inventory_Is_Full()
    {
        bool result = true;
        for (int i = 0; i < inventory_list.Length; i++)
        {
            if (inventory_list[i] == null || inventory_list[i].the_item == null)
            {
                result = false;
            }
            else if (inventory_list[i].current_num < inventory_list[i].the_item.item_data.stack_limit)
            {
                result = false;
            }
        }
        return result;
    }

    public virtual void Add_Item_To_Inventory(Item item)
    {
        
    }

    public virtual void Use_Current_Item()
    {
        
    }

    public virtual void Try_Consume_One(Item item, int id)
    {
      
    }
}
