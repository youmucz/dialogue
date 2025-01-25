namespace Story.Dialogue.Graph;

using Godot;
using Godot.Collections;
using Story.Dialogue.Core;
using Story.Dialogue.Utils;
using Story.Dialogue.Editor;


[Tool]
public partial class ConditionNode : DialogueNodeBase
{
    [NodeMeta] public override string NodeType { get; set; } = "Condition";
    [NodeMeta] public override string NodeCategory { get; set; } = "条件节点";

    [NodeMeta] public Dictionary<int, Dictionary> Variables { get; set; } = new() { { 0, VariableUtil.Default } };
    
    public ConditionNode()
    {

    }

    public ConditionNode(DialogueGraphResource dialogueGraphResource, Dictionary data) : base(dialogueGraphResource, data)
    {
    
    }

    public ConditionNode(DialogueGraphResource dialogueGraphResource, DialogueGraphNode mGraphNode, Dictionary data) : base(dialogueGraphResource, mGraphNode, data)
    {

    }

} 
