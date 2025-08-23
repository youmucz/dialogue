namespace Story.Dialogue.Graph;

using Godot;
using System;
using Godot.Collections;
using Story.Dialogue.Core;
using Story.Dialogue.Editor;

/// <summary>
/// 根节点,起始节点,不建议修改该节点基础功能
/// </summary>
[Tool]
public partial class RootNode : DialogueNode
{
    [NodeMeta] public override string NodeType { get; set; } = "Root";
    [NodeMeta] public override string NodeCategory { get; set; } = "Root";

    public RootNode()
    {
        // 可以在这里进行初始化操作
    }

    public RootNode(DialogueGraph dialogueGraph, Dictionary data) : base(dialogueGraph, data)
    {
    
    }

    public RootNode(DialogueGraph dialogueGraph, DialogueGraphNode mGraphNode, Dictionary data) : base(dialogueGraph, mGraphNode, data)
    {

    }

}
