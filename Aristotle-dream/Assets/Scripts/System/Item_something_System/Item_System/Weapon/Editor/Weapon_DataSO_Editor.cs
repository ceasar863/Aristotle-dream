using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace Badtime
{
    [CustomEditor(typeof(Weapon_Data_So))]//绑定自定义编辑器到Weapon_Data_So类型
    public class Weapon_DataSO_Editor : Editor
    {
        /****************************************************************
         * Type 是 C# 中描述「数据类型本身」的类，
         *存储了类型的名称、结构、继承关系等元数据
         ****************************************************************/
        private static List<Type> data_comp_types = new List<Type>();
        private Weapon_Data_So weapon_data_so;

        private bool show_force_update_button;
        private bool show_add_component_button;

        public void OnEnable()
        {
            weapon_data_so = target as Weapon_Data_So;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if(GUILayout.Button("Set Number Of Attack"))
            {
                foreach (var item in weapon_data_so.component_datas)
                {
                    item.Initalize_Attack_Data(weapon_data_so.number_of_attacks);
                }
            }

            show_add_component_button = EditorGUILayout.Foldout(show_add_component_button, "Add Components");
            if(show_add_component_button)
            {
                foreach(var data_comp_type in data_comp_types)
                {
                    if(GUILayout.Button(data_comp_type.Name))
                    {
                        var component_data = Activator.CreateInstance(data_comp_type) as Component_Data;
                        if (component_data == null) return;

                        weapon_data_so.Add_Data(component_data);
                        component_data.Initalize_Attack_Data(weapon_data_so.number_of_attacks);

                        EditorUtility.SetDirty(weapon_data_so);
                    }
                }
            }

            show_force_update_button = EditorGUILayout.Foldout(show_force_update_button, "Force Updates");
            if (show_force_update_button)
            {
                if (GUILayout.Button("Force Update Component Names"))
                {
                    foreach (var item in weapon_data_so.component_datas)
                    {
                        item.Set_Component_Name();
                    }
                }

                if (GUILayout.Button("Force Update Attack Names"))
                {
                    foreach (var item in weapon_data_so.component_datas)
                    {
                        item.Set_Attack_Data_Names();
                    }
                }
            }
        }

        [DidReloadScripts]
        private static void On_Recomplie()//约定优于配置
        {
            //获取当前命名空间里的所有程序集
            System.Reflection.Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();

            /*********************************************************
             * SelectMany会遍历程序集里的每一个类都放于一个Type[]一维数组，
             * assembly => assembly.GetTypes()指明需要提取出Type
             * 相对应的有一个Select，是每一个Type[]的数组，也就是二维的
             * -------------------------------------------------------
             * IEnumerable<Type> types 是所有程序集类型的可枚举契约，
             * 此时并没有实际读取任何 Type 数据（懒加载特性）；只有
             * 当执行 Where() 筛选或 foreach 遍历时，才会创建 IEnumerator<Type> 枚举器，
             * 逐个读取 Type 并执行逻辑～ 这样设计能减少内存占用，提升执行效率
             *********************************************************/
            IEnumerable<Type> types = assemblies.SelectMany(assembly => assembly.GetTypes());

            /*************************************
             *用where触发IEnumerbale的遍历，
             * 传入lambda函数type.IsSubclassOf说明
             * 提取的是某个类及其子类
             *************************************/
            IEnumerable<Type> flitered_types = types.Where(type => type.IsSubclassOf(typeof(Component_Data)) && !type.ContainsGenericParameters/*排除泛型类*/ && type.IsClass);

            //筛选完了可以加入到List里面了
            data_comp_types = flitered_types.ToList();
        }
    }
}
