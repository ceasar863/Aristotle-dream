using UnityEngine;

namespace Badtime
{
    public class Input_Hold : Weapon_Component
    {
        private Animator anim;

        private bool input;
        private bool min_hold_passed;

        protected override void Handle_Enter()
        {
            base.Handle_Enter();
            min_hold_passed = false;
        }
        
        private void Handle_Current_Input_Change(bool new_input)
        {
            input = new_input;
            Debug.Log(input);
            Set_Animator_Parameter();
        }

        private void Handle_Min_Hold_Passed()//在 min_hold 时间未到之前，即使玩家松开输入，也不会取消动画的 Hold 状态，会维持当前持有状态不变」
        {
            min_hold_passed = true;
            Set_Animator_Parameter();
        }

        private void Set_Animator_Parameter()
        {
            if (input)
            {
                anim.SetBool("Hold", input);
                return;
            }

            if(min_hold_passed)
            {
                anim.SetBool("Hold", input/*你知道的，只会是false*/);
            }
        }

        protected override void Awake()
        {
            base.Awake();
            anim = GetComponentInChildren<Animator>();
            weapon.On_Current_Input_Change += Handle_Current_Input_Change;
            anim_handler.On_Min_Hold_Passed += Handle_Min_Hold_Passed;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            weapon.On_Current_Input_Change -= Handle_Current_Input_Change;
            anim_handler.On_Min_Hold_Passed -= Handle_Min_Hold_Passed;
        }
    }
}
