using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[CreateAssetMenu(fileName = "Player_DataSO_Lib", menuName = "Game/Player DataSO Lib")]
public class Player_DataSO_Lib : ScriptableObject
{
    private Dictionary<string, object> data_dict = new Dictionary<string, object>();
    private Data_Container container = new Data_Container();

    private string Json_File_Path
    {
        get
        {
            return Path.Combine(Application.persistentDataPath, "GameData.json");
        }
    }

    /// <summary>
    /// 切换场景或打开游戏的时候加载数据
    /// </summary>
    private void Load_Data_From_Json_File()
    {
        if(!File.Exists(Json_File_Path))
        {
            container = new Data_Container();
            return;
        }

        try
        {
            string json_content = File.ReadAllText(Json_File_Path);
            container = JsonUtility.FromJson<Data_Container>(json_content);
        }
        catch(System.Exception e)
        {
            Debug.LogError($"加载Json文件失败:{e.Message}");
            container = new Data_Container();
        }
    }
    
    /// <summary>
    /// 切换到下一个场景前，或退出游戏前，又或是自动存档时保存数据
    /// </summary>
    private void Save_Data_To_Json_File()
    {
        try
        {
            string json_content = JsonUtility.ToJson(container, prettyPrint: true);
            File.WriteAllText(Json_File_Path, json_content);
        }
        catch (System.Exception e)
        {
            Debug.Log($"保存Json文件失败: {e.Message}");
        }
    }


    public void Save_Property(string key , object value)
    {
        if (string.IsNullOrEmpty(key) || value == null) return;

        //存到"内存"里
        if (data_dict.ContainsKey(key)) data_dict[key] = value;
        else data_dict.Add(key, value);


        //存储到本地Json文件里
        string json_data = JsonUtility.ToJson(value);
        if (container.data_container.ContainsKey(key))
            container.data_container[key] = json_data;
        else container.data_container.Add(key, json_data);
    }

     public object Load_Property(string key , Type target_type)
    {
        if (string.IsNullOrEmpty(key)) return null;

        //从"内存"里获取
        if(data_dict.TryGetValue(key, out object value))
        {
            //安全措施，避免类型不匹配
            if (target_type.IsAssignableFrom(value.GetType()))
                return value;
        }
        
        //从本地Json文件里获取
        if(container.data_container.TryGetValue(key  ,out string json_data))
        {
            //Json里面只存字符串，所以需要指定类型反序列化来获取存储数据
            object obj = JsonUtility.FromJson(json_data, target_type);
            if(obj !=null)
            {
                data_dict[key] = obj;//加载到内存里
                return obj;
            }
        }

        Debug.LogWarning("并未找到数据" + key);
        return null;
    }

    public void Delete_Property(string key)
    {
        if (data_dict.ContainsKey(key))
            data_dict.Remove(key);

        if (container.data_container.ContainsKey(key))
            container.data_container.Remove(key);
        Save_Data_To_Json_File();
    }

    public void Clear_All_Data()
    {
        data_dict.Clear();
        container.data_container.Clear();
        Save_Data_To_Json_File();

        if (File.Exists(Json_File_Path))
            File.Delete(Json_File_Path);
    }
}
