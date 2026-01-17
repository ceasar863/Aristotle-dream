using UnityEngine;

namespace Badtime
{
    public class Sequencer_Node : Composite_Node
    {
        int current;

        protected override void On_Start()
        {
            current = 0;
        }

        protected override void On_Stop()
        {
            
        }

        protected override State On_Upate()
        {
            Node current_node = children[current];
            switch(current_node.Update())
            {
                case State.Running:
                    return State.Running;
                case State.Failure:
                    return State.Failure;
                case State.Success:
                    current++;
                    break;
            }
            return current == children.Count ? State.Success : State.Running;
        }
    }
}
