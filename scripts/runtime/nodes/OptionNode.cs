using Godot;
using Godot.Collections;
using Story.Dialogue.Core;
using Story.Dialogue.Core.Data;
using Story.Dialogue.Graph;

namespace Story.Dialogue.Runtime.Nodes
{

    [Tool]
    public partial class OptionNode : DialogueNode
    {
        [NodeMeta] public Dictionary<int, string> Options { get; set; } = new() { { 0, "" } };

        [NodeMeta] public override string NodeType { get; set; } = "Option";
        [NodeMeta] public override string NodeCategory { get; set; } = "选项节点";

        public OptionNode()
        {
            // 可以在这里进行初始化操作
        }

        public OptionNode(DialogueGraph dialogueGraph, Dictionary data) : base(dialogueGraph, data)
        {

        }

        public OptionNode(DialogueGraph dialogueGraph, DialogueGraphNode mGraphNode, Dictionary data) : base(
            dialogueGraph, mGraphNode, data)
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
}