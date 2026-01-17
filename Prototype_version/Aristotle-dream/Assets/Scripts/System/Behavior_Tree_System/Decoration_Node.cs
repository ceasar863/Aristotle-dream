using UnityEngine;

namespace Badtime
{
    public abstract class Decoration_Node : Node
    {
        [HideInInspector] public Node child;

        public override Node Clone()
        {
            Decoration_Node node = Instantiate(this);
            node.child = child.Clone();
            return node;
        }
    }
}
