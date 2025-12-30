using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;


public class Player_Input_System : MonoSingle<Player_Input_System>
{
    public Player_Input player_input;

    /// <summary>
    /// 记录下每个操作所绑定的所有函数
    /// </summary>
    protected Dictionary<Operation_Mapping_Enum , List<Action<InputAction.CallbackContext>>> key_map;


    protected override void Awake()
    {
        base.Awake();
        
    }

    protected void OnEnable()
    {
        Set_Input_System_Active(true);
    }

    protected void OnDisable()
    {
        Set_Input_System_Active(false);
    }

    protected void Start()
    {
     
    }

    public void Load_Player_Input_System()
    {
        player_input = new Player_Input();
        key_map = new Dictionary<Operation_Mapping_Enum, List<Action<InputAction.CallbackContext>>>();
    }

    /// <summary>
    /// 是否允许玩家进行操作
    /// </summary>
    /// <param name="flag"></param>
    public void Set_Input_System_Active(bool flag)
    {
        if (flag) player_input.Enable();
        else player_input.Disable();
    }

    /// <summary>
    /// 设置某一个操作的启用与否 
    /// </summary>
    /// <param name="type"></param>
    /// <param name="flag"></param>
    public void Set_Single_Key_Active(Operation_Mapping_Enum type , bool flag)
    {
        InputAction action = Get_Input_Action(type);
        if (action == null) return;

        if (flag) action.Enable();
        else action.Disable();
    }


    /// <summary>
    /// 给某个操作设置相应的效果
    /// </summary>
    /// <param name="operation_type"></param>
    /// <param name="func"></param>
    public void Bind_Key_Map(Operation_Mapping_Enum operation_type , Map_Type_Enum map_type ,Action<InputAction.CallbackContext> func)
    {
        if(func==null)
        {
            Debug.LogWarning("需要一个回调函数！");
            return;
        }

        if (!key_map.ContainsKey(operation_type))
        {
            key_map.Add(operation_type, new List<Action<InputAction.CallbackContext>>());
        }

        InputAction action = Get_Input_Action(operation_type);
        if (action == null) return;

        if (map_type == Map_Type_Enum.Performed) action.performed += func;
        else if (map_type == Map_Type_Enum.Cancel) action.canceled += func;
        else action.started += func;

            key_map[operation_type].Add(func);
    }

    /// <summary>
    /// 解绑某个操作里的某个函数
    /// </summary>
    /// <param name="operation_type"></param>
    /// <param name="func"></param>
    public void Unbind_Key_Map(Operation_Mapping_Enum operation_type , Action<InputAction.CallbackContext> func)
    {
        if (func == null)
        {
            Debug.Log("需要一个回调函数！");
            return;
        }

        InputAction action = Get_Input_Action(operation_type);
        if (action == null) return;
        action.performed -= func;
        action.canceled -= func;

        key_map[operation_type].Remove(func);
    }


    /// <summary>
    /// 清除某个操作所有绑定的函数
    /// </summary>
    /// <param name="operation_type"></param>
    private void Clear_All_Type_Func(Operation_Mapping_Enum operation_type)
    {
        List<Action<InputAction.CallbackContext>> list = key_map[operation_type];
        
        InputAction action = Get_Input_Action(operation_type);
        if (action == null) return;


        foreach (Action<InputAction.CallbackContext> func in list)
        {
            action.performed -= func;
            action.canceled -= func;
        }
    }

    /// <summary>
    /// 获取某个操作的InputActions引用
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public InputAction Get_Input_Action(Operation_Mapping_Enum type)
    {
        switch (type)
        {
            case Operation_Mapping_Enum.Movement:
                return player_input.Player.Movement;
            case Operation_Mapping_Enum.Shoot:
                return player_input.Player.Shoot;
            case Operation_Mapping_Enum.Jump:
                return player_input.Player.Jump;
            case Operation_Mapping_Enum.Aim:
                return player_input.Player.Aim;
            case Operation_Mapping_Enum.Grab:
                return player_input.Player.Grab;
            case Operation_Mapping_Enum.Use_Item:
                return player_input.Player.Use_Item;
            default:
                throw new Exception(string.Format("You don't have the type {0}!", type));
        }
    }
}
