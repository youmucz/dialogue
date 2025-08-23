

using System.Collections.Generic;
using System.Linq;

namespace Story.Dialogue.Core;

using System;
using Godot;
using Godot.Collections;
using Story.Dialogue.Graph;


[Tool]
public partial class DialogueGraph : Resource
{
    #region resource parms base
    
    [Export] public string Id = Guid.NewGuid().ToString();
    [Export] public string FileDir;
    [Export] public string Filename;
    [Export] public string Filepath;
    [Export] public string EngineVersion = Engine.GetVersionInfo()["string"].ToString();
    
    /// <summary> 变量系统,当前对话存储的变量 </summary>
    [Export] public Dictionary<string, Dictionary> Variables { get; set; } = new();
    /// <summary> 全局变量系统,当前对话存储的全局变量 </summary>
    [Export] public Dictionary<string, Dictionary> GlobalVariables { get; set; } = new();
    
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
    [Export] public Array<Dictionary> Connection = new ();
    
    public Dictionary NodeMetaClasses = new ();
    public List<DialogueNode> Nodes { get; set; } = new();
    
    private RootNode _rootNode;
    
    #endregion

    public void Initialize()
    {
        NodeMetaClasses.Clear();

        foreach (var data in NodeData)
        {
            var nodeName = (string)data["NodeName"]; 
            var nodeType = (string)data["NodeType"];
            var meta = NodeMetaFactory.GetNodeMeta(nodeType, this, data);
            
            if (meta is null) return;
            if (nodeType == "Root") _rootNode = (RootNode)meta;
            
            meta.Initialize();
            NodeMetaClasses.Add(nodeName, meta);
            Nodes.Add(meta);
        }
    }

    public DialogueNode GetNode(string nodeId)
    {
        return Nodes.FirstOrDefault(n => n.NodeId == nodeId);
    }

    public DialogueNode GetNodeByName(string nodeName)
    {
        if (NodeMetaClasses.TryGetValue(nodeName, out var value))
            return (DialogueNode)value;
        
        return null;
    }

    public Array<DialogueNode> GetNextNodeByName(StringName name)
    {
        var toNode = new Array<DialogueNode>();
        
        foreach (var connect in Connection)
        {
            if ((StringName)connect["from_node"] == name)
            {
                toNode.Add(GetNodeByName((StringName)connect["to_node"]));
            }
        }

        return toNode;
    }
}
