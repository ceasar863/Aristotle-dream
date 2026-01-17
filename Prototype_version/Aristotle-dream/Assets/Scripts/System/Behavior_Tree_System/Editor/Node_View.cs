using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;
using UnityEditor;
using UnityEngine.Rendering;
using UnityEditor.UIElements;

namespace Badtime
{
    public class Node_View : UnityEditor.Experimental.GraphView.Node
    {
        public Action<Node_View> OnNode_Selected;

        public Node node;
        public Port input , output;//Port是GraphView里的端口类，用于连接节点之间的连线

        public Node_View(Node node) : base("Assets/Scripts/System/Behavior_Tree_System/Editor/UI_Builder/Node_View.uxml")
        {
            this.node = node;
            this.title = node.name;//title是Node的标题
            this.viewDataKey = node.guid;//viewDataKey是Node的唯一标识符

            style.left = node.position.x;//style是Node_View的样式属性
            style.top = node.position.y;

            Create_InputPorts();
            Create_OutputPorts();
            Setup_Classes();

            Label description_label = this.Q<Label>("description");//从当前VisualElement(UI 节点)中,找到UI上ID为"description"的Label控件

            /**********************************************************
             *虽然倒着写逻辑更顺，但是这样做不行，因为
             *调用 Bind() 的瞬间，UI Toolkit 就会读取当前的 bindingPath，
             *去 SerializedObject 中查找对应字段并同步数据。
             **********************************************************/
            description_label.bindingPath = "description";//设置Label的绑定路径为节点的description属性
            description_label.Bind(new SerializedObject(node));//将节点对象包装为SerializedObject，并绑定到Label上，实现数据同步
        }

        private void Setup_Classes()
        {
            if (node is Action_Node)
            {
                AddToClassList("action");//给 UI 元素追加 USS 类名，用于绑定对应的样式（USS 中用于定义样式规则的类选择器）
            }
            else if (node is Composite_Node)
            {
                AddToClassList("composite");
            }
            else if (node is Decoration_Node)
            {
                AddToClassList("decoration");
            }
            else if (node is Root_Node)
            {
                AddToClassList("root");
            }
        }

        private void Create_OutputPorts()
        {
            if(node is Action_Node)
            {

            }
            else if(node is Composite_Node)
            {
                output = InstantiatePort(Orientation.Vertical, Direction.Output, Port.Capacity.Multi, typeof(bool));
            }
            else if(node is Decoration_Node)
            {
                output = InstantiatePort(Orientation.Vertical, Direction.Output, Port.Capacity.Single, typeof(bool));
            }
            else if(node is Root_Node)
            {
                output = InstantiatePort(Orientation.Vertical, Direction.Output, Port.Capacity.Single, typeof(bool));
            }

            if (output != null)
            {
                output.portName = "";

                output.style.flexDirection = FlexDirection.Column;//设置端口的布局方向为纵向排列

                outputContainer.Add(output);
            }
        }

        private void Create_InputPorts()
        {
            //根据节点类型创建输入端口
            //InstantiatePort的四个参数分别是：
            //端口方向（水平/垂直）、端口类型（输入/输出）、端口容量（单一/多重）、端口数据类型
            if (node is Action_Node)
            {
                input = InstantiatePort(Orientation.Vertical, Direction.Input, Port.Capacity.Single, typeof(bool));
            }
            else if (node is Composite_Node)
            {
                input = InstantiatePort(Orientation.Vertical, Direction.Input, Port.Capacity.Single, typeof(bool));
            }
            else if (node is Decoration_Node)
            {
                input = InstantiatePort(Orientation.Vertical, Direction.Input, Port.Capacity.Single, typeof(bool));
            }
            else if(node is Root_Node)
            {
                //根节点没有输入端口喵 :p
            }

            if (input != null)
            {
                input.portName = "";

                input.style.flexDirection = FlexDirection.ColumnReverse;//设置端口的布局方向为纵向反向排列

                inputContainer.Add(input);//存放输入端口的容器
            }
        }


        /*
         * 当在 GraphView 画布上手动拖动节点改变其位置时，
         * Unity 会自动调用该方法，完成位置更新与数据同步
         */
        public override void SetPosition(Rect newPos)//newPos是当前正在被拖动 / 操作的这个节点视图
        {
            base.SetPosition(newPos);

            Undo.RecordObject(node, "Rebaviour Tree (Set Position)");//记录节点对象的状态，以便支持撤销操作

            node.position.x = newPos.xMin;
            node.position.y = newPos.yMin;

            EditorUtility.SetDirty(node);//标记节点对象为已修改，确保更改会被保存
        }

        public override void OnSelected()
        {
            base.OnSelected();
            if (OnNode_Selected != null) 
                OnNode_Selected.Invoke(this);
        }

        public void Sort_Children()
        {
            Composite_Node composite = node as Composite_Node;
            if(composite)
            {
                composite.children.Sort(SortBy_Horizontal_Position);
            }
        }

        private int SortBy_Horizontal_Position(Node left, Node right)
        {
            //-1表示排在前面，1表示排在后面
            return left.position.x < right.position.x ? -1:1;
        }

        public void Update_State()
        {
            RemoveFromClassList("running");
            RemoveFromClassList("failure");
            RemoveFromClassList("success");

            if (Application.isPlaying)
            {
               
                switch (node.state)
                {
                    case Node.State.Running:
                        if (node.started)
                        {
                            AddToClassList("running");
                        }
                        break;
                    case Node.State.Failure:
                        AddToClassList("failure");
                        break;
                    case Node.State.Success:
                        AddToClassList("success");
                        break;
                }
            }
        }
    }
}
