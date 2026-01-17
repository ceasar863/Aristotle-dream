using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace Badtime
{
    public abstract class Composite_Node : Node
    {
        [HideInInspector] public List<Node> children = new List<Node>();

        public override Node Clone()
        {
            Composite_Node node = Instantiate(this);
            node.children = children.ConvertAll(child => child.Clone());
            return node;
        }
    }
}
