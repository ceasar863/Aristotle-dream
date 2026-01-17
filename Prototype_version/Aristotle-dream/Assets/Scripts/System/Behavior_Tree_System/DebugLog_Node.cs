using UnityEngine;

namespace Badtime
{
    public class DebugLog_Node : Action_Node
    {
        public string message;

        protected override void On_Start()
        {
            Debug.Log($"OnStart{message}");
        }

        protected override void On_Stop()
        {
            Debug.Log($"OnStop{message}");
        }

        protected override State On_Upate()
        {
            Debug.Log($"OnUpdate{message}");
            Debug.Log($"Blackboard:{blackboard.move_to_position}");
            blackboard.move_to_position.x++;

            return State.Success;
        }
    }
}
