using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Badtime
{
    public class Weapon_Storage : MonoSingle<Weapon_Storage>
    {
       [SerializeField] private List<Weapon_Data_So> storage; 
        private Dictionary<string, Weapon_Data_So> dict;

        protected override void Awake()
        {
            base.Awake();
            dict = new Dictionary<string, Weapon_Data_So>();
            Collect_Weapon_Data();
        }

        private void Collect_Weapon_Data()//约定优于配置
        {
            for (int i = 0; i < storage.Count; i++)
            {
                Weapon_Data_So weapon = storage[i];
                string name = weapon.weapon_name;
                dict.Add(name, weapon);
            }
        }

        public Weapon_Data_So Get_Weapon_Data(string weapon_name)
        {
            dict.TryGetValue(weapon_name , out Weapon_Data_So data);
            return data;
        }
    }
}
