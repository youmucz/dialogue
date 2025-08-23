namespace Story.Dialogue.Graph;

using Godot;
using System;
using Godot.Collections;
using Story.Dialogue.Core;
using Story.Dialogue.Utils;

[Tool]
public partial class VariableNode : DialogueNode
{
    [NodeMeta] public override string NodeType { get; set; } = "Variable";
    [NodeMeta] public override string NodeCategory { get; set; } = "条件节点";
    
    [NodeMeta] public Dictionary<int, Dictionary> Variables { get; set; } = new() { { 0, VariableUtil.VariableDefault } };
    
    public VariableNode()
    {
        // 可以在这里进行初始化操作
    }

    public VariableNode(DialogueGraph dialogueGraph, Dictionary data) : base(dialogueGraph, data)
    {
    
    }

    public VariableNode(DialogueGraph dialogueGraph, DialogueGraphNode mGraphNode, Dictionary data) : base(dialogueGraph, mGraphNode, data)
    {

    }
    
    public override void NodeEntered()
    {
        base.NodeEntered();
    }

    public override void NodeExited()
    {
        base.NodeExited();
    }
    
} 
