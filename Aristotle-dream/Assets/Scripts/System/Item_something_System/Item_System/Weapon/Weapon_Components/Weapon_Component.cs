using UnityEngine;

namespace Badtime
{
    public abstract class Weapon_Component : MonoBehaviour
    {
        protected Weapon weapon;
        
        //有点问题：后面会修:p
        //protected Animation_Trigger anim_handler => weapon.anim_handler;
        /*
        等价于
        protected Animation_Trigger anim_handler
        {
            get
            {
                //返回 weapon 对象的 anim_handler 属性值
                return weapon.anim_handler;
            }
        }
        */
        
        protected Animation_Trigger anim_handler;

        protected bool is_attack_active;

        public virtual void Init()
        {

        }

        protected virtual void Awake()
        {
            weapon = GetComponent<Weapon>();
            anim_handler = GetComponentInChildren<Animation_Trigger>();
        }

        protected virtual void Start()
        {
            weapon.on_enter += Handle_Enter;
            weapon.on_exit += Handle_Exit;
        }

        protected virtual void Handle_Enter()
        {
            is_attack_active = true;
        }

        protected virtual void Handle_Exit()
        {
            is_attack_active = false;
        }

        protected virtual void OnEnable()
        {
           
        }

        protected virtual void OnDisable()
        {

        }

        protected virtual void OnDestroy()
        {
            weapon.on_enter -= Handle_Enter;
            weapon.on_exit -= Handle_Exit;
        }
    }

    /****************************************************************************
    *- 规避Unity编辑器不支持未闭合泛型的问题，Weapon_Data的List需要一个统一的类来整合这些组件信息。(划重点！）
    *- 支持双场景扩展（有无专属数据的组件均可便捷继承）
    *- 实现组件统一管理+类型安全，无需强制转换
    ***************************************************************************/
    public abstract class Weapon_Component<T1,T2> : Weapon_Component where T1:Component_Data<T2> where T2:Attack_Data
    {
        protected T1 data;
        protected T2 current_attack_data;

        protected override void Handle_Enter()
        {
            base.Handle_Enter();
            current_attack_data = data.Attack_Data[weapon.current_attack_counter];
        }

        public override void Init()
        {
            base.Init();
            weapon = GetComponent<Weapon>();
            data = weapon.Data.Get_Data<T1>();
        }
    }
}
