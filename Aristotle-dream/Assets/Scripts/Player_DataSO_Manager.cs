using System;
using System.Reflection;
using UnityEngine;

public class Player_DataSO_Manager : MonoBehaviour
{
    public static Player_DataSO_Manager instace { get; private set; }
    public Player_DataSO_Lib lib;

    private void Awake()
    {
        if (instace != null && instace != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instace = this;
            DontDestroyOnLoad(this.gameObject);
        }

        if(lib==null)
        {
            lib = ScriptableObject.CreateInstance<Player_DataSO_Lib>();       }
    }

    public void Save_Component_Data(Component component)
    {
        if (component == null || lib == null) return;

        Type component_type = component.GetType();
        string component_key_prefix = $"{component.name}_{component.GetInstanceID()}";

        FieldInfo[] fields = component_type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        foreach(FieldInfo field in fields)
        {
            if(field.GetCustomAttribute<Save_Data_Attribute>()!=null)
            {
                string field_key = $"{component_key_prefix}_{field.Name}";
                object field_value = field.GetValue(component);
                lib.Save_Property(field_key, field_value);
            }
        }
    }

    public void Load_Component_Data(Component component)
    {
        if (component == null || lib == null) return;
        Type component_type = component.GetType();
        string component_key_prefix = $"{component.name}_{component.GetInstanceID()}";

        FieldInfo[] fields = component_type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        foreach (FieldInfo field in fields)
        {
            if (field.GetCustomAttribute<Save_Data_Attribute>() != null)
            {
                string field_key = $"{component_key_prefix}_{field.Name}";
                object field_value = lib.Load_Property(field_key);
                if (field_value != null) field.SetValue(component , field);
            }
        }
    }
}
