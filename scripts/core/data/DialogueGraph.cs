
using System;
using System.Collections.Generic;
using System.Linq;

using Godot;
using Godot.Collections;
using Story.Dialogue.Runtime.Nodes;


namespace Story.Dialogue.Core.Data
{
    [Tool]
    public partial class DialogueGraph : Resource
    {
        #region resource parms base
        
        [Export] public string Gid = Guid.NewGuid().ToString();
        [Export] public string FileDir;
        [Export] public string Filename;
        [Export] public string Filepath;
        [Export] public string EngineVersion = Engine.GetVersionInfo()["string"].ToString();
        
        /// <summary> 变量系统,当前对话存储的变量 </summary>
        [Export] public Godot.Collections.Dictionary<string, Dictionary> Variables { get; set; } = new();
        /// <summary> 全局变量系统,当前对话存储的全局变量 </summary>
        [Export] public Godot.Collections.Dictionary<string, Dictionary> GlobalVariables { get; set; } = new();
        /// <summary> 节点数据 </summary>
        [Export] public Array<Dictionary> NodeData = new (){
            new Dictionary
            {
                {"NodeType", "Root"}, 
                {"NodeName", "Root"}, 
                {"NodeCategory", "Root"},
                {"NodePositionOffset", Vector2.Zero},
            }
        };
        
        /// <summary> 节点连接信息 </summary>
        [Export] public Array<Dictionary> Connections = new ();
        
        public List<DialogueNode> Nodes { get; set; } = new();
        
        private RootNode _rootNode;
        private List<DialogueConnection> _connections = new ();
        
        #endregion

        public void Initialize()
        {
            Nodes.Clear();

            foreach (var data in NodeData)
            {
                var nodeName = (string)data["NodeName"]; 
                var nodeType = (string)data["NodeType"];
                var meta = NodeMetaFactory.GetNodeMeta(nodeType, this, data);
                
                if (meta is null) return;
                if (nodeType == "Root") _rootNode = (RootNode)meta;
                
                meta.Initialize();
                Nodes.Add(meta);
            }
            
            foreach (var c in Connections)
            {
                var connection = new DialogueConnection(
                    (string)c["from_node"], (int)c["from_port"], 
                    (string)c["to_node"], (int)c["to_port"]
                    );
                
                _connections.Add(connection);
            }
        }
        
        public RootNode GetRootNode()
        {
            return _rootNode;
        }

        public DialogueNode GetNodeByName(string nodeName)
        {
            return Nodes.FirstOrDefault(n => n.NodeName == nodeName);
        }
        
        public Array<DialogueNode> GetNextNodeByName(StringName name)
        {
            var toNode = new Array<DialogueNode>();
            
            foreach (var connect in Connections)
            {
                if ((StringName)connect["from_node"] == name)
                {
                    toNode.Add(GetNodeByName((StringName)connect["to_node"]));
                }
            }

            return toNode;
        }
        
        /// <summary>
        /// 获取节点的所有连接信息
        /// </summary>
        /// <param name="nodeName"></param>
        /// <returns></returns>
        public List<DialogueConnection> GetNodeConnections(string nodeName)
        {
            return _connections.Where(c => c.FromNode == nodeName || c.ToNode == nodeName).ToList();
        }
        
        /// <summary>
        /// 获取指定节点所有后续直接链接节点
        /// </summary>
        /// <param name="nodeName"></param>
        /// <returns></returns>
        public List<DialogueNode> GetConnectedNodes(string nodeName)
        {
            var connections = GetNodeConnections(nodeName);
            var connectedNames = connections
                .Where(c => c.FromNode == nodeName)
                .Select(c => c.ToNode)
                .ToList();
            
            return Nodes.Where(n => connectedNames.Contains(n.NodeName)).ToList();
        }
    }
}
