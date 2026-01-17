using UnityEngine;

namespace Badtime
{
    public class Repeat_Node : Decoration_Node
    {
        protected override void On_Start()
        {
            
        }

        protected override void On_Stop()
        {
            
        }

        protected override State On_Upate()
        {
            child.Update();
            return State.Running;
        }
    }
}
