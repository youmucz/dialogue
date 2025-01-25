namespace Story.Dialogue.Graph;

using Godot;
using Story.Dialogue.Runtime;


[Tool]
public partial class DialogGraphNode : DialogueGraphNode
{
    public DialogNode Node;
    
    private LineEdit _char;
    private TextEdit _content;
    private TextureButton _portrait;
    
    public override void _Ready()
    {
        base._Ready();
        
        _char = GetNode<LineEdit>("HSplitContainer/VBoxContainer/Character");
        _content = GetNode<TextEdit>("HSplitContainer/VBoxContainer/Content");
        _portrait = GetNode<TextureButton>("HSplitContainer/Portrait");
        
        _char.TextChanged += CharOnTextChanged;
        _content.TextChanged += ContentOnTextChanged;

        Node = Base as DialogNode;
        
        if (Node != null)
        {
            _content.Text = Node.Content;
            _char.Text = Node.Character;
        }
    }

    private void ContentOnTextChanged()
    {
        Node.Content = _content.Text;
    }

    private void CharOnTextChanged(string text)
    {
        Node.Character = text;
    }
}
