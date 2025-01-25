namespace Story.Dialogue.Variable;
using Godot;
using System;


[Tool]
public partial class OptionEdit : Control
{
	
	public event Action<int> EventDelButtonPressed = delegate { };
	public event Action<int, string> EventOptionTextChanged = delegate { };
	
	public int SlotIndex { get; set; } = 0;
	
	private LineEdit _lineEdit;
	private TextureButton _delButton;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_lineEdit = GetNode<LineEdit>("HSplitContainer/LineEdit");
		_delButton = GetNode<TextureButton>("HSplitContainer/DelButton");
		
		_lineEdit.TextChanged += LineEditOnTextChanged;
		_delButton.Pressed += DelButtonOnPressed;
	}

	public void Initialize(int slotIndex, string text)
	{
		SlotIndex = slotIndex;
		_lineEdit.Text = text;
	}

	public string GetOptionText()
	{
		return _lineEdit.Text;
	}

	private void LineEditOnTextChanged(string text)
	{
		EventOptionTextChanged?.Invoke(SlotIndex, text);
	}

	private void DelButtonOnPressed()
	{
		EventDelButtonPressed?.Invoke(SlotIndex);
	}
}
