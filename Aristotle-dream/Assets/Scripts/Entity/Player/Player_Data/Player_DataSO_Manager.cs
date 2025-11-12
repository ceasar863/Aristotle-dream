using System;
using System.Reflection;
using UnityEngine;

public class Player_DataSO_Manager : MonoBehaviour
{
    public static Player_DataSO_Manager instance { get; private set; }
    public Player_DataSO_Lib lib;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }

        if(lib==null)
        {
            lib = ScriptableObject.CreateInstance<Player_DataSO_Lib>();       }
    }


    /// <summary>
    /// 保存组件类的数据
    /// </summary>
    /// <param name="component"></param>
    public void Save_Component_Data(Component component)
    {
        if (component == null || lib == null) return;

        Type component_type = component.GetType();
        string component_key_prefix = $"{component.name}_{component.GetInstanceID()}";

        FieldInfo[] fields = component_type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        foreach(FieldInfo field in fields)
        {
            if(field.GetCustomAttribute<SaveDataAttribute>()!=null)
            {
                string field_key = $"{component_key_prefix}_{field.Name}";
                object field_value = field.GetValue(component);
                lib.Save_Property(field_key, field_value);
            }
        }
    }

    /// <summary>
    /// 加载组件类的数据
    /// </summary>
    /// <param name="component"></param>
    public void Load_Component_Data(Component component)
    {
        if (component == null || lib == null) return;
        Type component_type = component.GetType();
        string component_key_prefix = $"{component.name}_{component.GetInstanceID()}";

        FieldInfo[] fields = component_type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        foreach (FieldInfo field in fields)
        {
            if (field.GetCustomAttribute<SaveDataAttribute>() != null)
            {
                string field_key = $"{component_key_prefix}_{field.Name}";
                object field_value = lib.Load_Property(field_key, field.FieldType);
                if (field_value != null) field.SetValue(component, field_value);
            }
        }
    }

    /// <summary>
    /// 保存任意对象的数据
    /// </summary>
    /// <param name="target_name"></param>
    /// <param name="target"></param>
    public void Save_Object_Data(string target_name , object target)
    {
        if (target == null || lib == null || string.IsNullOrEmpty(target_name)) return;
        lib.Save_Property(target_name, target);
    }

    /// <summary>
    /// 加载任意对象的数据
    /// </summary>
    /// <param name="target_name"></param>
    /// <param name="target_type"></param>
    public void Load_Object_Data(string target_name , Type target_type)
    {
        if (lib == null || string.IsNullOrEmpty(target_name) || target_type == null) return;
        lib.Load_Property(target_name, target_type);
    }
}
