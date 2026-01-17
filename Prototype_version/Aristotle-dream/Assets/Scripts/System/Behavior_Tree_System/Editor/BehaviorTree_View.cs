using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using System.Linq;
using System.Collections.Generic;
using System;

namespace Badtime
{
    /******************************************
     * 把自定义C#控件(比如 BehaviorTree_View) *
     * 注册到 Unity 的 UXML 系统里” 的工厂类 *
     ******************************************/
    [UxmlElement("BehaviorTree_View")]

    /*partial是标记,Unity需要通过这个关键字自动生成配套的UXML解析代码
     *partial（部分类）是 C# 的基础关键字，唯一目的是：
     *将同一个类的定义拆分到多个.cs 文件中，
     *编译时编译器会把这些拆分的部分合并成一个完整的类。
     */
    public partial class BehaviorTree_View : GraphView
    {
        //public new class UxmlFactory : UxmlFactory<BehaviorTree_View, GraphView.UxmlTraits> { } 已弃用的写法

        public Action<Node_View> OnNode_Selected;

        Behavior_Tree tree;
        public BehaviorTree_View()
        {
            Insert(0, new GridBackground());//添加网格背景

            this.AddManipulator(new ContentZoomer());//添加缩放功能
            this.AddManipulator(new ContentDragger());//添加拖拽功能
            this.AddManipulator(new SelectionDragger());//添加选择拖拽功能
            this.AddManipulator(new RectangleSelector());//添加矩形选择功能

            var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Scripts/System/Behavior_Tree_System/Editor/UI_Builder/BehaviorTree_Editor.uss");
            styleSheets.Add(styleSheet);// 把样式表添加到根节点，让UI应用样式

            Undo.undoRedoPerformed += On_Undo_Redo;//当用户在编辑器中执行「撤销（Ctrl+Z）」或「重做（Ctrl+Y）」操作时，自动触发绑定的 On_Undo_Redo 方法。
        }

        private void On_Undo_Redo()
        {
            PopulateView(tree);
            AssetDatabase.SaveAssets();
        }

        Node_View Find_NodeView(Node node)//根据数据节点查找对应的节点视图
        {
            return GetNodeByGuid(node.guid) as Node_View;
        }

        internal void PopulateView(Behavior_Tree tree)
        {
            this.tree = tree;
            
            /*
             解绑再绑定的原因：在刷新画布（PopulateView）时，先解绑旧的回调引用，
             避免重复绑定导致「一次变更触发多次回调逻辑」（比如重复删除数据节点），
             再重新绑定，保证回调逻辑的唯一性和正确性。
             */
            graphViewChanged -= OnGraphView_Changed;
            DeleteElements(graphElements);//清空现有的图元素
            graphViewChanged += OnGraphView_Changed;

            if (tree.root_node == null)
            {
                tree.root_node = tree.Create_Node(typeof(Root_Node)) as Root_Node;
                EditorUtility.SetDirty(tree);//标记行为树资产为已修改，确保更改会被保存
                AssetDatabase.SaveAssets();//保存资产文件
            }

            tree.nodes.ForEach(n => Create_NodeView(n));//为行为树中的每个节点创建对应的节点视图
            tree.nodes.ForEach(n =>
            {
                var children = tree.Get_Children(n);
                children.ForEach(c =>
                {
                    Node_View parent_view = Find_NodeView(n);
                    Node_View child_view = Find_NodeView(c);

                    Edge edge = parent_view.output.ConnectTo(child_view.input); //创建 UI 边（Edge）并建立端口连接
                    AddElement(edge);//将 UI 边添加到 GraphView 画布，实现可视化
                });
            });
        }

        /*
         * Unity GraphView 中用于「筛选可连线的兼容端口」的重写方法，
         * 核心作用是在你尝试拖拽节点连线时，
         * 自动过滤出哪些端口可以和当前拖拽的端口建立有效连接，
         * 避免无效连线.
         * 返回的列表是给 Unity GraphView 编辑器自身（底层内置逻辑）使用的
         */
        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {
            return ports.ToList().Where(endport =>
            endport.direction != startPort.direction &&//端口的方向需要不同
            endport.node != startPort.node).ToList();//端口不能连接到同一个节点上
        }

        private GraphViewChange OnGraphView_Changed(GraphViewChange graph_view_change)
        {
            if(graph_view_change.elementsToRemove != null)
            {
                foreach(var element in graph_view_change.elementsToRemove)
                {
                    Node_View node_view = element as Node_View;
                    if(node_view != null)
                    {
                        tree.Delete_Node(node_view.node);
                    }

                    Edge edge = element as Edge;
                    if(edge != null)
                    {
                        Node_View parent_view = edge.output.node as Node_View;
                        Node_View child_view = edge.input.node as Node_View;
                        tree.Remove_Child(parent_view.node, child_view.node);
                    }

                }
            }

            if(graph_view_change.edgesToCreate != null)
            {
               foreach(var edge in graph_view_change.edgesToCreate)
               {
                    Node_View parent_node_view = edge.output.node as Node_View;
                    Node_View child_node_view = edge.input.node as Node_View;
                    tree.Add_Child(parent_node_view.node, child_node_view.node);
               }
            }

            if(graph_view_change.movedElements != null)//节点被移动时
            {
                nodes.ForEach((n) =>
                {
                    Node_View view = n as Node_View;
                    view.Sort_Children();
                });
            }

            return graph_view_change;
        }

        public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
        {
            //base.BuildContextualMenu(evt);
            {
                var types = TypeCache.GetTypesDerivedFrom<Action_Node>();
                foreach(var type in types)
                {
                    //evt.menu是菜单实例，就是鼠标右键点击时弹出的菜单
                    //AppendAction是向菜单添加一个新选项的方法,有两个参数，前者是选项名称，后者是选项被点击时调用的回调函数
                    evt.menu.AppendAction($"[{type.BaseType.Name}] {type.Name}"/*前者是基类名，后者是子类名*/, 
                                            (a) => Create_Node(type));
                }
            }

            {
                var types = TypeCache.GetTypesDerivedFrom<Composite_Node>();
                foreach (var type in types)
                {
                    
                    evt.menu.AppendAction($"[{type.BaseType.Name}] {type.Name}",
                                            (a) => Create_Node(type));
                }
            }

            {
                var types = TypeCache.GetTypesDerivedFrom<Decoration_Node>();
                foreach (var type in types)
                {
                   
                    evt.menu.AppendAction($"[{type.BaseType.Name}] {type.Name}",
                                            (a) => Create_Node(type));
                }
            }
        }

        void Create_Node(System.Type type)
        {
            Node node = tree.Create_Node(type);
            Create_NodeView(node);
        }

        void Create_NodeView(Node node)
        {
            Node_View node_view = new Node_View(node);
            node_view.OnNode_Selected = OnNode_Selected;
            AddElement(node_view);//把节点视图添加到图视图中
        }

        public void Update_NodeStates()
        {
            nodes.ForEach(n =>
            {
                Node_View view = n as Node_View;
                view.Update_State();
            });
        }
    }
}

/*
 *  热知识:
 *  UXML 是 Unity 为 UI Toolkit 设计的可视化布局标记语言，
 *  全称是 Unity Extensible Markup Language，
 *  本质是类 HTML 的 XML 格式文件，用来描述 UI 界面的结构和基础属性。
 */