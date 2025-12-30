using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

namespace Badtime
{
    [CreateAssetMenu(fileName = "New_Weapon_Data" , menuName = "Data/Weapon Data/Basic Weapon Data" , order =0)]
    public class Weapon_Data_So : ScriptableObject
    {

        [field: SerializeField] public RuntimeAnimatorController runtime_animator_controller { get; private set; }
        [field:SerializeField] public int number_of_attacks { get; private set; }

        //[SerializeReference]：Unity 会完整序列化 Weapon_Sprite_Component_Data 的类型信息和所有字段，在 Inspector 面板能看到子类的具体数据，运行时也能正确还原为子类对象。
        [field:SerializeReference] public List<Component_Data> component_datas { get; private set; }

        public T Get_Data<T>()
        {
            /*   
            OfType<T>筛选列表里所有属于 T 类型（比如 Weapon_Sprite_Component_Data）的元素；
            FirstOrDefault()：只取筛选结果里的第一个，如果没找到该类型，就返回 null（避免报错）；
            */
            return component_datas.OfType<T>().FirstOrDefault();
        }

        public List<Type> GetAll_Dependencies()
        {
            return component_datas.Select(component => component.component_dependency).ToList();
        }

        public void Add_Data(Component_Data data)
        {
            if (component_datas.FirstOrDefault(t => t.GetType() == data.GetType()) != null)
            {
                Debug.Log($"You already have {data.GetType()} , aren't you :p?");
                return;
            }
            component_datas.Add(data);
        }
    }
}
