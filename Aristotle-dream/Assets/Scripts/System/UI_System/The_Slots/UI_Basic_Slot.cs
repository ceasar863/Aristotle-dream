using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class UI_Basic_Slot : MonoBehaviour
{
    //一些存储系统的信息
    protected Consumable_Inventory_System consumable_inventory;
    protected Weapon_Inventory_System weapon_inventory;
    
    [Header("Slot Details")]
    [SerializeField] protected TextMeshProUGUI stack_number_text;
    [SerializeField] protected Item_In_Inventory item_in_slot;

    [SerializeField] protected Item_In_Inventory default_item;
    public Button interactable_button;

    protected virtual void Awake()
    {
        interactable_button = GetComponentInChildren<Button>();
        stack_number_text = GetComponentInChildren<TextMeshProUGUI>();
        item_in_slot = default_item;
    }

    protected virtual void Start()
    {
        consumable_inventory = CommonUtils.Get_Sort_Inventory(Item_Type.Consumable) as Consumable_Inventory_System;
        weapon_inventory = CommonUtils.Get_Sort_Inventory(Item_Type.Weapon) as Weapon_Inventory_System;
    }

    protected virtual void OnEnable()
    {
        
    }

    protected virtual void OnDisable()
    {
        
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)//显示选中高光
    {
        
    }
    
    protected virtual void OnTriggerExit2D(Collider2D collision)//取消选中高光
    {
        
    }

    public virtual void Update_UI_Slot(Item_In_Inventory item)
    {
        this.item_in_slot = item;

        interactable_button.image.sprite = this.item_in_slot.the_item.item_data.item_icon;
        stack_number_text.text = this.item_in_slot.current_num==0? " " : this.item_in_slot.current_num.ToString();
    }
}
