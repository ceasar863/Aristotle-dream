using System;
using UnityEngine;

namespace Badtime
{
    public class Timer
    {
        public event Action on_timer_done;

        private float start_time;
        private float duration_time;
        private float target_time;
        private bool is_active;//是否处于可激活状态(可激活就是可以调用action，反之不行)

        public Timer(float duration_time)
        {
            this.duration_time = duration_time;
        }

        public void Start_Timer()
        {
            start_time = Time.time;
            target_time = start_time + duration_time;
            is_active = true;
        }

        public void Set_Timer_Active(bool flag)
        {
            is_active = flag;
        }

        public void Tick()
        {
            if (!is_active) return;
            if(Time.time > target_time)
            {
                on_timer_done?.Invoke();
                Set_Timer_Active(false);
            }
        }
    }
}
