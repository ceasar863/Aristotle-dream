using UnityEngine;

namespace Badtime
{
    public class Wait_Node : Action_Node
    {
        public float duration = 1;
        float start_time;

        protected override void On_Start()
        {
            start_time = Time.time;
        }

        protected override void On_Stop()
        {
           
        }

        protected override State On_Upate()
        {
            if (Time.time >= start_time + duration) return State.Success;
            return State.Running;
        }
    }
}
