namespace Story.Dialogue.Graph;

using Godot;
using System;
using Godot.Collections;
using Story.Dialogue.Core;


[Tool]
public partial class FinishedNode : DialogueNode
{
    [NodeMeta] public override string NodeType { get; set; } = "Finished";
    [NodeMeta] public override string NodeCategory { get; set; } = "选项节点";

    [Export, NodeMeta] public int Option { set; get; } = 0;

    public FinishedNode()
    {
        // 可以在这里进行初始化操作
    }

    public FinishedNode(DialogueGraph dialogueGraph, Dictionary data) : base(dialogueGraph, data)
    {
    
    }

    public FinishedNode(DialogueGraph dialogueGraph, DialogueGraphNode mGraphNode, Dictionary data) : base(dialogueGraph, mGraphNode, data)
    {

    }

} 
