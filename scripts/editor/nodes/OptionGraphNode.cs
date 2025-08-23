namespace Story.Dialogue.Graph;

using System.Collections.Generic;
using Godot;
using Godot.Collections;
using Story.Dialogue.Runtime;
using Story.Dialogue.Variable;


[Tool]
public partial class OptionGraphNode : DialogueGraphNode
{
	public OptionNode Node;

	[Export] public Texture2D AddButtonIcon;
	[Export] public PackedScene OptionEditScene; 

	private Button _addButton = new ();
	private HBoxContainer _titleContainer;
	private Array<OptionEdit> _optionSlots = new ();

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_addButton.Icon = AddButtonIcon;
		_addButton.Pressed += AddButtonOnPressed;
		
		_titleContainer = GetTitlebarHBox();
		_titleContainer.AddChild(_addButton);
		
		Node = Base as OptionNode;

		if (Node != null)
			foreach (var kvp in Node.Options)
			{
				CreateOptionEdit(kvp.Key, kvp.Value);
			}
	}

	private void CreateOptionEdit(int slotIndex, string text)
	{
		OptionEdit option;
		if (slotIndex == 0)
		{
			option = GetNode<OptionEdit>("OptionEdit");
		}
		else
		{
			option = OptionEditScene.Instantiate<OptionEdit>();
			AddChild(option);
			SetSlotEnabledRight(slotIndex, true);
		}

		Node.Options.TryAdd(slotIndex, text);
		
		_optionSlots.Add(option);
		option.Initialize(slotIndex, text);
		option.EventDelButtonPressed += OptionOnEventDelButtonPressed;
		option.EventOptionTextChanged += OptionOnEventOptionTextChanged;
	}

	private void OptionOnEventOptionTextChanged(int slotIndex, string text)
	{
		Node.Options[slotIndex] = text;
	}

	private void AddButtonOnPressed()
	{
		CreateOptionEdit(_optionSlots.Count,"");
	}

	private void OptionOnEventDelButtonPressed(int slotIndex)
	{
		// 默认初始选项不可删除
		if (slotIndex <= 0 || slotIndex >= _optionSlots.Count) return;
		
		var option = _optionSlots[slotIndex];
		
		Node.Options.Clear();
		_optionSlots.RemoveAt(slotIndex);
		
		for (var i = 0; i < _optionSlots.Count; i++)
		{
			var optionEdit = _optionSlots[i];
			optionEdit.SlotIndex = i;
			Node.Options.Add(i, optionEdit.GetOptionText());
		}
		
		RemoveChild(option);
		ResetSize();
	}
}
