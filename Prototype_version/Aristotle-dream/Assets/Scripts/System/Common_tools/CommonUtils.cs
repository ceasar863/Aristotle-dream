using UnityEngine;

public static class CommonUtils
{
    public static Base_Inventory_System Get_Sort_Inventory(Item_Type type)
    {
        switch(type)
        {
            case Item_Type.Weapon:
                return Weapon_Inventory_System.instance;
            case Item_Type.Consumable:
                return Consumable_Inventory_System.instance;
            default:
                return null;
        }
    }
}
