using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

public class GameObject_Pool_Manager<T> : MonoBehaviour where T:MonoBehaviour
{
    public static GameObject_Pool_Manager<T> instance;
    protected Dictionary<GameObject, List<T>> item_pools;

    protected Action<T> action;
    [SerializeField] protected List<Warm_Prefab_And_Count<T>> warm_to_prefabs;

    private void Awake()
    {
        if(instance !=null && instance!=this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this; 
            DontDestroyOnLoad(gameObject);
        }

        item_pools = new Dictionary<GameObject, List<T>>();
    }

    private void Start()
    {
        foreach(Warm_Prefab_And_Count<T> target_prefab in warm_to_prefabs)
        {
            Warm_Pool(target_prefab.prefab, target_prefab.count);
        }
    }

    public virtual T Get_Item_Object_From_Pool(GameObject target_prefab , Vector3 spawn_position , int num=1)
    {
        T target_item = null;

        //没有预制体，无法生成
        if (target_prefab == null)
        {
            throw new Exception(string.Format($"You don't have the target prefab named {target_prefab}!"));
        }

        //检查是否存在该类物品,不存在就创建一个对象池
        if (!item_pools.ContainsKey(target_prefab))
        {
            item_pools[target_prefab] = new List<T>();
        }

        //如果存在闲置的实例，有就复用
        List<T> item_pool = item_pools[target_prefab];
        foreach(T item in item_pool)
        {
            if(item!=null && !item.gameObject.activeSelf)
            {
                target_item = item;
                break;
            }
        }
        
        //没有现造一个填充进对象池
        if(target_item == null)
        {
            GameObject new_item = Instantiate(target_prefab, spawn_position, Quaternion.identity);
            target_item = new_item.GetComponent<T>();
            item_pool.Add(target_item);
        }

        //设置相关属性
        target_item.gameObject.SetActive(true);
        target_item.transform.position = spawn_position;

        return target_item;
    }

    public T Spawn_Single_Object(GameObject target_prefab, Vector3 spawn_position)
    {
        List<T> item_pool = item_pools.ContainsKey(target_prefab) ? item_pools[target_prefab] : new List<T>();

        GameObject new_item = Instantiate(target_prefab, spawn_position, Quaternion.identity);
        T target_item = new_item.GetComponent<T>();
        item_pool.Add(target_item);

        return target_item;
    }

    protected void Warm_Pool(GameObject obj , int warm_count)
    {
        if (obj == null || warm_count <= 0) return;
        for(int i=0; i<warm_count; i++)
        {
            T item = Spawn_Single_Object(obj , transform.position);
            Recycle_Item_To_Pool(item);
        }
    }

    public virtual void Recycle_Item_To_Pool(T item)
    {
        if (item == null) return;
        item.gameObject.SetActive(false);
        item.transform.SetParent(transform);
    }
}

[Serializable]
public class Warm_Prefab_And_Count<T>
{
    public GameObject prefab;
    public int count;
}
