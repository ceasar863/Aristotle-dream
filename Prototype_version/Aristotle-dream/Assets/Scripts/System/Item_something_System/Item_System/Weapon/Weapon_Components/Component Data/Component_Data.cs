using System;
using UnityEngine;

namespace Badtime
{
    [Serializable]
    public abstract class Component_Data
    {
        //可以修改Inspector的Element1,2,3 ，让编辑更清晰,前提是这得是第一个变量
        //也就是说其前面不能先声明其他[SerializeField]或public变量
        [SerializeField , HideInInspector/*序列化，但是别在Inspector出现:D*/] private string name;

        public Type component_dependency { get; protected set; }

        public Component_Data()
        {
            Set_Component_Name();
            Set_Component_Depencency();
        }

        public void Set_Component_Name() => name = GetType().Name;

        protected abstract void Set_Component_Depencency();

        public virtual void Set_Attack_Data_Names()
        {

        }
        public virtual void Initalize_Attack_Data(int numer_of_attakcs)
        {

        }
    }

    [Serializable]
    public abstract class Component_Data<T> : Component_Data where T:Attack_Data
    {
        [SerializeField] private T[] attack_data;
        public T[] Attack_Data { get=>attack_data ; private set=>attack_data=value; }

        public override void Set_Attack_Data_Names()
        {
            base.Set_Attack_Data_Names();
            for(int i=0; i<Attack_Data.Length; i++)
            {
                Attack_Data[i].Set_Attack_Name(i + 1);
            }
        }

        public override void Initalize_Attack_Data(int number_of_attakcs)
        {
            base.Initalize_Attack_Data(number_of_attakcs);

            int old_len = (Attack_Data!=null)? Attack_Data.Length:0;
            if (old_len == number_of_attakcs) return;


            /****************************************************************
             * 若 newLength 等于原数组长度：不做任何操作，直接返回；
             * ---------------------------------------------------------------
             * 若 newLength 大于原数组长度：创建新数组，复制原数组所有元素，
             * 新数组中超出原长度的部分填充对应类型的 default 值（如 int 为 0，引用类型为 null）；
             * ----------------------------------------------------------------
             * 若 newLength 小于原数组长度：创建新数组，
             * 仅复制原数组前 newLength 个元素，超出部分被丢弃；
             ****************************************************************/
            Array.Resize(ref attack_data, number_of_attakcs);

            if(old_len < number_of_attakcs)
            {
                for(int i=old_len; i<number_of_attakcs; i++)
                {
                    var new_object = Activator.CreateInstance(typeof(T)) as T;
                    attack_data[i] = new_object;
                }
            }

            Set_Attack_Data_Names();
        }
    }
}
