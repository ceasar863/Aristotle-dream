using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UIElements;
using UnityEditor;

namespace Badtime
{
    [UxmlElement("Inspector_View")]
    public partial class Inspector_View : VisualElement
    {
        Editor editor;

        public Inspector_View()
        {

        }

        internal void Update_Selection(Node_View node_view)
        {
            Clear();
            UnityEngine.Object.DestroyImmediate(editor);//销毁之前的编辑器实例，避免内存泄漏

            editor = Editor.CreateEditor(node_view.node);//创建一个针对所选节点数据的自定义编辑器
            IMGUIContainer container = new IMGUIContainer(() => 
            { 
                if (editor.target) //确保编辑器的目标对象存在
                { 
                    editor.OnInspectorGUI();
                }
            });//创建一个IMGUI容器，用于在UIElements中嵌入IMGUI绘制的内容
            Add(container);//把IMGUI容器添加到Inspector_View中，以显示节点的属性编辑界面
        }
    }
}
