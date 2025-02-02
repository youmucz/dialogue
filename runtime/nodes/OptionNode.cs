namespace Story.Dialogue.Graph;

using Godot;
using Godot.Collections;
using Story.Dialogue.Core;


[Tool]
public partial class OptionNode : DialogueNodeBase
{
    [NodeMeta] public Dictionary<int, string> Options { get; set; } = new () {{0, ""}};
    
    [NodeMeta] public override string NodeType { get; set; } = "Option";
    [NodeMeta] public override string NodeCategory { get; set; } = "选项节点";
    
    public OptionNode()
    {
        // 可以在这里进行初始化操作
    }

    public OptionNode(DialogueGraphResource dialogueGraphResource, Dictionary data) : base(dialogueGraphResource, data)
    {
    
    }

    public OptionNode(DialogueGraphResource dialogueGraphResource, DialogueGraphNode mGraphNode, Dictionary data) : base(dialogueGraphResource, mGraphNode, data)
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
