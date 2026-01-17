using Badtime;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.Callbacks;
using System;

public class BehaviorTree_Editor : EditorWindow
{
    BehaviorTree_View tree_view;
    Inspector_View inspector_view;
    IMGUIContainer blackboard_view;

    /*
     *SerializedObject和SerializedProperty的关系：
     *SerializedObject表示一个Unity对象的序列化表示，
     *SerializedProperty表示该对象中一个具体属性,必须依附于SerializedObject才存在，
     *修改SerializedProperty后，必须通过SerializedObject生效，
     *SerializedObject 需要刷新缓存来同步SerializedProperty
     */
    SerializedObject tree_object;
    SerializedProperty blackboard_property;

    [SerializeField]
    private VisualTreeAsset m_VisualTreeAsset = default;

    [MenuItem("BehaviorTree_Editor/Editor ...")]
    public static void OpenWindow()
    {
        BehaviorTree_Editor wnd = GetWindow<BehaviorTree_Editor>();
        wnd.titleContent = new GUIContent("BehaviorTree_Editor");
    }

    [OnOpenAsset]//标记此方法为「打开资产时的回调方法」，即打开资产时会自动调用此方法
    public static bool OnOpenAsset(int instace_id , int line)
    {
        if(Selection.activeObject is Behavior_Tree)//确保选中的对象是行为树资产
        {
            OpenWindow();//如果是就执行自定义逻辑，打开自定义窗口
            return true;//表示已处理该打开请求，避免Unity继续执行默认的打开逻辑
        }
        return false;
    }

    public void CreateGUI()//仅在窗口/UI首次创建、或被重置（刷新）时执行一次
    {
        // 1. 获取窗口的根UI节点（所有UI都要挂在这个根节点下，类似HTML的<body>）
        // Each editor window contains a root VisualElement object
        VisualElement root = rootVisualElement;

        // 2. 加载UXML布局文件（UI的结构，比如按钮、面板、输入框都定义在这）
        // Instantiate UXML
        var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Scripts/System/Behavior_Tree_System/Editor/UI_Builder/BehaviorTree_Editor.uxml");
        visualTree.CloneTree(root);// 把UXML里的UI结构克隆到根节点下（否则UI不会显示）

        // 3. 加载USS样式文件（UI的样式，比如颜色、大小、位置，类似CSS）
        var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Scripts/System/Behavior_Tree_System/Editor/UI_Builder/BehaviorTree_Editor.uss");
        root.styleSheets.Add(styleSheet);// 把样式表添加到根节点，让UI应用样式

        // 4. 获取UXML里定义的各个UI子节点的引用（方便后续操作这些UI）
        //Q<T>方法用于在 UI 树中查找特定类型或名称的子节点
        tree_view = root.Q<BehaviorTree_View>();
        inspector_view = root.Q<Inspector_View>();
        blackboard_view = root.Q<IMGUIContainer>();
        blackboard_view.onGUIHandler = 
            () => 
            {
                /*
                    关于blackboard为什么依赖SerializedObject和SerializedProperty来绘制：
                    Blackboard是Behavior_Tree类中的一个字段，
                    直接修改Behavior_Tree实例的blackboard字段不会被Unity的序列化
                 */
                tree_object.Update();//同步序列化对象的状态
                EditorGUILayout.PropertyField(blackboard_property);//绘制黑板属性的编辑器界面
                tree_object.ApplyModifiedProperties();//应用对序列化对象所做的修改
            };

        tree_view.OnNode_Selected = On_NodeSelection_Changed;
        OnSelectionChange();//初始化时调用一次，确保选中对象时能正确显示
    }

    private void OnEnable()
    {
        //playModeStateChanged,一个委托，顾名思义，是在播放模式状态改变时触发的事件
        EditorApplication.playModeStateChanged -= On_PlayModeState_Changed;
        EditorApplication.playModeStateChanged += On_PlayModeState_Changed;
    }
    private void OnDisable()
    {
        EditorApplication.playModeStateChanged -= On_PlayModeState_Changed;
    }


    /************************************************
     * 在编辑器「播放模式↔编辑模式」切换完成时，
     * 主动触发 OnSelectionChange() 刷新行为树视图，
     * 确保模式切换后，视图能同步当前选中的行为树状态，
     * 避免「模式切换后视图空白 / 数据不同步」的问题。
     ************************************************/
    private void On_PlayModeState_Changed(PlayModeStateChange obj)
    {
        switch(obj)
        {
            case PlayModeStateChange.EnteredEditMode:
                OnSelectionChange();
                break;
            case PlayModeStateChange.ExitingEditMode:
                break;
            case PlayModeStateChange.EnteredPlayMode:
                OnSelectionChange();
                break;
            case PlayModeStateChange.ExitingPlayMode:
                break;
        }
    }


    private void OnSelectionChange()//当 Unity 编辑器的「选择对象」发生变化时自动触发
    {
        Behavior_Tree tree = Selection.activeObject as Behavior_Tree;

        //尝试寻找选中对象是否有TreeRunner.
        if (!tree)
        {
            if(Selection.activeObject)
            {
                //尝试从选中的游戏对象中获取 Behavior_TreeRunner 组件
                Behavior_TreeRunner runner = Selection.activeGameObject?.GetComponent<Behavior_TreeRunner>();
                if(runner)//如果有该组件就赋给tree
                {
                    tree = runner.tree;
                }
            }
        }

        if(Application.isPlaying)//运行时更新视图
        {
            if(tree)
            {
                tree_view.PopulateView(tree);
            }
        }

        if (tree && AssetDatabase.CanOpenAssetInEditor(tree.GetInstanceID()))//确保选中的对象是可编辑的行为树资产
        {
            tree_view.PopulateView(tree);
        }

        if(tree!=null)
        {
            tree_object = new SerializedObject(tree);
            blackboard_property = tree_object.FindProperty("blackboard");
        }
    } 

    void On_NodeSelection_Changed(Node_View node)
    {
        inspector_view.Update_Selection(node);

    }

    private void OnInspectorUpdate()//仅在编辑模式下运行时调用
    {
        tree_view?.Update_NodeStates();
    }
}
