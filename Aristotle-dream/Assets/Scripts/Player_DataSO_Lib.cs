using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Player_DataSO_Lib", menuName = "Game/Player DataSO Lib")]
public class Player_DataSO_Lib : ScriptableObject
{
    private Dictionary<string, object> data_dict = new Dictionary<string, object>();

   public void Save_Property(string key , object value)
    {
        data_dict[key] = value;
    }

    public object Load_Property(string key)
    {
        data_dict.TryGetValue(key, out object value);
        return value;
    }
}
