using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Badtime
{
    /// <summary>
    /// 行为树的节点基本类
    /// </summary>
    public abstract class Node : ScriptableObject
    {
        public enum State
        {
            Running,
            Failure,
            Success
        }

        [HideInInspector] public State state = State.Running;
        [HideInInspector] public bool started = false;
        [HideInInspector] public string guid;//每个节点的唯一标识符
        [HideInInspector] public Vector2 position;
        [HideInInspector] public BlackBoard blackboard;
        [HideInInspector] public Enemy agent;
        [TextArea] public string description;

        public State Update()
        {
            if(!started)//如果没启动就进入启动状态
            {
                On_Start();
                started = true;
            }

        
            state = On_Upate();//执行节点自己的更新函数

            //不论执行的结果成功与否，都应该停止了
            if (state == State.Success || state == State.Failure)
            {
                On_Stop();
                started = false;
            }

            return state;//给父节点转递执行结果
        }

        public virtual Node Clone()
        {
            return Instantiate(this);
        }

        //树中每个节点都必须要写的三个函数
        protected abstract void On_Start();
        protected abstract void On_Stop();
        protected abstract State On_Upate();
    }
}
