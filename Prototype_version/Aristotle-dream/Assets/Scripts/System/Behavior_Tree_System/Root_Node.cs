using UnityEngine;

namespace Badtime
{
    public class Root_Node : Node
    {
        public Node child;

        protected override void On_Start()
        {
            
        }

        protected override void On_Stop()
        {
            
        }

        protected override State On_Upate()
        {
            return child.Update();
        }

        public override Node Clone()
        {
            Root_Node node = Instantiate(this);
            node.child = child.Clone();
            return node;
        }   
    }
}
