using Godot;
using Godot.Collections;
using Story.Dialogue.Core;
using Story.Dialogue.Graph;
using Story.Dialogue.Utils;
using Story.Dialogue.Core.Data;

namespace Story.Dialogue.Runtime.Nodes
{
    [Tool]
    public partial class ConditionNode : DialogueNode
    {
        [NodeMeta] public override string NodeType { get; set; } = "Condition";
        [NodeMeta] public override string NodeCategory { get; set; } = "条件节点";

        [NodeMeta] public Dictionary<int, Dictionary> Variables { get; set; } = new() { { 0, VariableUtil.Default } };

        public ConditionNode()
        {

        }

        public ConditionNode(DialogueGraph dialogueGraph, Dictionary data) : base(dialogueGraph, data)
        {

        }

        public ConditionNode(DialogueGraph dialogueGraph, DialogueGraphNode mGraphNode, Dictionary data) : base(
            dialogueGraph, mGraphNode, data)
        {

        }

    }
}