using System;

using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Godot;
using Godot.Collections;
using Story.Dialogue.Core.Interfaces;
using Story.Dialogue.Graph;

namespace Story.Dialogue.Core.Data
{

    /// <summary>
    /// 节点的元数据结构,用来存储数据和实现业务逻辑,因为使用了反射来获取程序集里的所有类的<see cref="NodeMetaAttribute"/>,所以需要初始化构造函数.
    /// </summary>
    [Tool]
    public partial class DialogueNode : Resource, IDialogueNode
    {
        #region Basics

        /// <summary> Edit graph node </summary>
        protected DialogueGraphNode GraphNode;

        /// <summary> <see cref="DialogueGraphNode"/> </summary>
        protected DialogueGraph DialogueGraph;

        private string _nodeName;
        private Vector2 _positionOffset;

        #endregion

        #region Serialie/Deserialize @base

        /// <summary>需要存储到Resource本地文件里的参数和参数值。</summary>
        private readonly List<PropertyInfo> _metaPropertyInfo;
        
        /// <summary> 节点类型,将会展示到<see cref="GraphNode.Title"/>作为节点面板名称. </summary>
        [NodeMeta]
        public virtual string NodeType { get; set; }

        /// <summary> 节点名称,等价于<see cref="GraphNode.Name"/>由于<see cref="GraphEdit"/>是以Name作为节点索引参数. </summary>
        [NodeMeta]
        public virtual string NodeName
        {
            private set
            {
                if (GraphNode != null) GraphNode.Name = value;
                else _nodeName = value;
            }
            get
            {
                if (GraphNode != null) return GraphNode.Name;

                return _nodeName;
            }
        }

        /// <summary> 当前节点类型,注册到<see cref="GraphEdit"/>的<see cref="PopupMenu"/>的子菜单栏中 </summary>
        [NodeMeta]
        public virtual string NodeCategory { get; set; }

        /// <summary> <see cref="GraphNode.PositionOffset"/> </summary>
        [NodeMeta]
        public virtual Vector2 NodePositionOffset
        {
            private set
            {
                if (GraphNode != null) GraphNode.PositionOffset = value;
                else
                {
                    _positionOffset = value;
                }
            }
            get => GraphNode?.PositionOffset ?? _positionOffset;
        }

        /// <summary> 当前节点的孩子节点 </summary>
        [NodeMeta]
        public virtual Array<string> Children { get; set; } = new();

        /// <summary>
        /// 序列化meta数据,用来存储到resource本地文件上
        /// </summary>
        /// <returns></returns>
        public Dictionary Serialize()
        {
            var data = new Dictionary();

            foreach (var property in _metaPropertyInfo)
            {
                data.Add(property.Name, Get(property.Name));
            }

            return data;
        }

        /// <summary>
        /// 反序列化数据
        /// </summary>
        /// <param name="data"></param>
        public void Deserialize(Dictionary data)
        {
            if (data == null) return;

            foreach (var kvp in data)
            {
                Set((StringName)kvp.Key, kvp.Value);
            }
        }

        #endregion

        #region 构造函数

        public DialogueNode()
        {

        }

        /// <summary>
        /// 编辑器模式创建节点 @base
        /// </summary>
        /// <param name="dialogueGraph"></param>
        /// <param name="mGraphNode"></param>
        /// <param name="data"></param>
        public DialogueNode(DialogueGraph dialogueGraph, DialogueGraphNode mGraphNode, Dictionary data)
        {
            DialogueGraph = dialogueGraph;

            Deserialize(data);

#if TOOLS
            GraphNode = mGraphNode;
            GraphNode.Title = (string)data["NodeType"];
            GraphNode.Name = (string)data["NodeName"];
            GraphNode.PositionOffset = (Vector2)data["NodePositionOffset"];
#endif

            // 加载用作序列化的meta数据
            _metaPropertyInfo = new List<PropertyInfo>();
            _metaPropertyInfo = GetType().GetProperties()
                .Where(t => t.GetCustomAttributes(typeof(NodeMetaAttribute), true).Any())
                .ToList();
        }

        /// <summary>
        /// behavior tree加载的节点meta
        /// </summary>
        /// <param name="dialogueGraph"></param>
        /// <param name="data"></param>
        public DialogueNode(DialogueGraph dialogueGraph, Dictionary data)
        {
            DialogueGraph = dialogueGraph;

            Deserialize(data);

            // 加载用作序列化的meta数据
            _metaPropertyInfo = new List<PropertyInfo>();
            _metaPropertyInfo = GetType().GetProperties()
                .Where(t => t.GetCustomAttributes(typeof(NodeMetaAttribute), true).Any())
                .ToList();
        }

        /// <summary>
        /// 实例化当前节点的所有孩子节点
        /// </summary>
        public void Initialize(DialogueGraphNode graphNode = null)
        {
            
#if TOOLS
            if (graphNode is not null)
            {
                graphNode.Title = NodeType;
                graphNode.Name = NodeName;
                graphNode.PositionOffset = NodePositionOffset;
                GraphNode = graphNode;
            }
#endif
            
        }

        /// <summary>
        /// 设置参数
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public void UpdateProperty(StringName name, Variant value)
        {
            Set(name, value);
        }
        
        #endregion

        #region runtime functions

        public virtual void NodeEntered()
        {
            GD.Print($"{NodeName} NodeEntered");
        }

        public virtual void NodeExecute()
        {
            GD.Print($"{NodeName} NodeExecute");
        }

        public virtual void NodeExited()
        {
            GD.Print($"{NodeName} NodeExited");
        }

        public virtual void Stop()
        {
            NodeExited();
        }

        #endregion
 
    }
}
