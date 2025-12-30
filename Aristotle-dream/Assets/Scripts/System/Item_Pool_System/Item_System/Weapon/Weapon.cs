using System;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class Weapon : Item
{
    protected List<Opera_Action> weapon_band; //需要绑定的函数集合

    public virtual void Equip()
    {
        foreach (Opera_Action item in weapon_band)
            Player_Input_System.instance.Bind_Key_Map(item.opera_type, item.map_type, item.action);
    }

    public virtual void Unequip()
    {
        foreach (Opera_Action item in weapon_band)
            Player_Input_System.instance.Unbind_Key_Map(item.opera_type, item.action);
    }
}

public class Opera_Action
{
    public Operation_Mapping_Enum opera_type;
    public Map_Type_Enum map_type;
    public Action<InputAction.CallbackContext> action;
}