namespace Story.Dialogue.Graph;

using Godot;
using System;
using Godot.Collections;
using Story.Dialogue.Core;


[Tool]
public partial class DialogNode : DialogueNode
{
    [NodeMeta] public override string NodeType { get; set; } = "Dialog";
    [NodeMeta] public override string NodeCategory { get; set; } = "对话节点";
    
    /// <summary> 说话角色 </summary>
    [Export, NodeMeta] public string Character { 
        get => _character;
        set { _character = value;  CharChanged?.Invoke(value);}
    }
    /// <summary> 说话文本,台词等等 </summary>
    [Export, NodeMeta] public string Content { 
        get => _content;
        set { _content = value; ContentChanged?.Invoke(value); }
    }

    private string _character;
    private string _content;

    public event Action<string> CharChanged;
    public event Action<string> ContentChanged;

    public DialogNode()
    {
        // 可以在这里进行初始化操作
    }

    public DialogNode(DialogueGraph resource, Dictionary data) : base(resource, data)
    {
    
    }

    public DialogNode(DialogueGraph resource, DialogueGraphNode mGraphNode, Dictionary data) : base(resource, mGraphNode, data)
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
