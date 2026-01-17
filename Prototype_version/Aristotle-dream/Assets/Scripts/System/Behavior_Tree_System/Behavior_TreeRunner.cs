using UnityEngine;

namespace Badtime
{
    public class Behavior_TreeRunner : MonoBehaviour
    {
        public Behavior_Tree tree;
        private void Start()
        {
            tree = tree.Clone();
            tree.Bind(GetComponent<Enemy>());
        }

        private void Update()
        {
            tree.Update();
        }
    }
}
