using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Badtime
{
    [CreateAssetMenu()]
    public class Behavior_Tree : ScriptableObject
    {
        /*2026.1.4疑问：这么设计是何evil啊？*/
        public Node root_node;
        public Node.State tree_state = Node.State.Running;
        public List<Node> nodes = new List<Node>();
        public BlackBoard blackboard = new BlackBoard();

        public Node.State Update()
        {
            if (root_node.state == Node.State.Running)
            {
                tree_state = root_node.Update();
            }

            return tree_state;
        }

        public Node Create_Node(System.Type type)
        {
            Node node = ScriptableObject.CreateInstance(type) as Node;
            node.name = type.Name;

#if UNITY_EDITOR
            node.guid = GUID.Generate().ToString();
            Undo.RecordObject(this, "Behaviour Tree (CreateNode)");//记录行为树资产的状态，以便撤销
#endif
            nodes.Add(node);

#if UNITY_EDITOR
            if (!Application.isPlaying)
                AssetDatabase.AddObjectToAsset(node, this);//把新创建的节点对象添加到行为树资产文件中
            Undo.RegisterCreatedObjectUndo(node, "Behaviour Tree (CreateNode)");//注册子对象顺序的撤销操作,作用是在撤销时恢复子对象的顺序

            AssetDatabase.SaveAssets();//保存资产文件
#endif
            return node;
        }

        public void Delete_Node(Node node)
        {
#if UNITY_EDITOR
            Undo.RecordObject(this, "Behaviour Tree (DeleteNode)");//记录行为树资产的状态，以便撤销
#endif

            nodes.Remove(node);
#if UNITY_EDITOR
            //AssetDatabase.RemoveObjectFromAsset(node);
            Undo.DestroyObjectImmediate(node);//使用撤销系统销毁节点对象，确保支持撤销操作
            AssetDatabase.SaveAssets();
#endif
        }

        public void Add_Child(Node parent , Node child)
        {
            Decoration_Node decorator = parent as Decoration_Node;
            if (decorator)
            {
#if UNITY_EDITOR
                Undo.RecordObject(decorator, "Behaviour Tree (AddChild)");
#endif

                decorator.child = child;

#if UNITY_EDITOR
                EditorUtility.SetDirty(decorator);
#endif
            }

            Root_Node root_node = parent as Root_Node;
            if (root_node)
            {
#if UNITY_EDITOR
                Undo.RecordObject(root_node, "Behaviour Tree (AddChild)");
#endif

                root_node.child = child;

#if UNITY_EDITOR
                EditorUtility.SetDirty(root_node);
#endif
            }

            Composite_Node composite = parent as Composite_Node;
            if (composite)
            {
#if UNITY_EDITOR
                Undo.RecordObject(composite, "Behaviour Tree (AddChild)");
#endif

                composite.children.Add(child);

#if UNITY_EDITOR
                EditorUtility.SetDirty(composite);
#endif
            }
        }

        public void Remove_Child(Node parent, Node child)
        {
            Decoration_Node decorator = parent as Decoration_Node;
            if (decorator)
            {
#if UNITY_EDITOR
                Undo.RecordObject(decorator, "Behaviour Tree (RemoveChild)");
#endif    

                decorator.child = null;

#if UNITY_EDITOR
                EditorUtility.SetDirty(decorator);
#endif
            }

            Root_Node root_node = parent as Root_Node;
            if (root_node)
            {
#if UNITY_EDITOR
                Undo.RecordObject(root_node, "Behaviour Tree (RemoveChild)");
#endif

                root_node.child = null;

#if UNITY_EDITOR
                EditorUtility.SetDirty(root_node);
#endif
            }
            Composite_Node composite = parent as Composite_Node;
            if (composite)
            {
#if UNITY_EDITOR
                Undo.RecordObject(composite, "Behaviour Tree (RemoveChild)");
#endif 

                composite.children.Remove(child);

#if UNITY_EDITOR
                EditorUtility.SetDirty(composite);
#endif
            }
        }

        public List<Node> Get_Children(Node parent)
        {
            List<Node> children = new List<Node>();

            Decoration_Node decorator = parent as Decoration_Node;
            if (decorator && decorator.child!=null) children.Add(decorator.child);

            Root_Node root_node = parent as Root_Node;
            if (root_node && root_node.child!=null) children.Add(root_node.child);

            Composite_Node composite = parent as Composite_Node;
            if (composite) return composite.children;

            return children;
        }

        public void Traverse(Node node , System.Action<Node> visiter)
        {
            if(node)
            {
                visiter.Invoke(node);
                List<Node> children = Get_Children(node);
                children.ForEach((n) => Traverse(n, visiter));
            }
        }

        public Behavior_Tree Clone()
        {
            Behavior_Tree tree = Instantiate(this);
            tree.root_node = tree.root_node.Clone();
            tree.nodes = new List<Node>();
            Traverse(tree.root_node , (n)=> tree.nodes.Add(n));//DFS收集所有节点

            return tree;
        }   

        public void Bind(Enemy agent)
        {
            Traverse(root_node,
            (node) =>
            {
                node.agent = agent;
                node.blackboard = blackboard;
            });
        }
    }
}
