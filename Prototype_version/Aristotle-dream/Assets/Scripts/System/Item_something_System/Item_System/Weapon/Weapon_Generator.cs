using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Badtime
{
    public class Weapon_Generator : MonoBehaviour
    {
        [SerializeField] private Weapon weapon;
        [SerializeField] private Weapon_Data_So data;

        private List<Weapon_Component> component_already_on_weapon = new List<Weapon_Component>();
        private List<Weapon_Component> components_added_to_weapon = new List<Weapon_Component>();
        private List<Type> component_dependencies = new List<Type>();
        private Animator anim;

        private void Start()
        {
            anim = GetComponentInChildren<Animator>();
            Generate_Weapon(data);
        }

        [ContextMenu("Test Generate")]
        private void TestGeneration()
        {
            Generate_Weapon(data);
        }

        public void Generate_Weapon(Weapon_Data_So data)
        {
            weapon.Set_Data(data);

            components_added_to_weapon.Clear();
            component_already_on_weapon.Clear();
            component_dependencies.Clear();

            component_already_on_weapon = GetComponents<Weapon_Component>().ToList();
            component_dependencies = data.GetAll_Dependencies();

            foreach(Type dependency in component_dependencies)
            {
                //查找components_added_to_weapon中是否有该依赖
                //随笔：我觉得可以理解为一种to-do-list,added的就好比待办事项
                //已完成的就不管了
                if (components_added_to_weapon.FirstOrDefault(component => component.GetType() == dependency))
                    continue;

                //随笔：如果有现成的就拿现成的,没有就现造
                Weapon_Component weapon_component = component_already_on_weapon.FirstOrDefault(component => component.GetType() == dependency);
                if (weapon_component == null)
                {
                    weapon_component = gameObject.AddComponent(dependency) as Weapon_Component;
                }

                if (weapon_component.enabled == false) 
                    weapon_component.enabled = true;

                weapon_component.Init();//手动设定生命周期？

                //随笔：待办事项完成+1
                components_added_to_weapon.Add(weapon_component);
            }

            // Except：求集合差集，即 component_already_on_weapon - components_added_to_weapon = 待删除组件
            var components_to_remove = component_already_on_weapon.Except(components_added_to_weapon);
            foreach (var weapon_component in components_to_remove)
                Destroy(weapon_component);

            anim.runtimeAnimatorController = data.runtime_animator_controller;
        }

        public void Change_Weapon(Weapon_Data_So data)
        {
            //现先屏蔽掉原来已有装备的所有相关组件
            foreach (Weapon_Component weapon_component in component_already_on_weapon)
            {
                if (weapon_component == null) continue;
                weapon_component.enabled = false;
            }

            //生成or恢复新武器的组件
            Generate_Weapon(data);
        }
    }
}
