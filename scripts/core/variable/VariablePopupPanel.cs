namespace Story.Dialogue.Variable;

using Godot;
using Godot.Collections;
using System;

[Tool]
public partial class VariablePopupPanel : PopupPanel
{
	private LineEdit _variableEdit;
	private LineEdit _valueEdit;
	private TextEdit _tipsEdit;
	private Button _saveButton;
	private Button _closeButton;
	
	/// <summary>.保存新建变量 </summary>
	public event Action<Dictionary> EventVariableSaved = delegate { };
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_variableEdit = GetNode<LineEdit>("VBoxContainer/VarBoxContainer/Variable");
		_valueEdit = GetNode<LineEdit>("VBoxContainer/VarBoxContainer/Value");
		_tipsEdit = GetNode<TextEdit>("VBoxContainer/TextEdit");

		_saveButton = GetNode<Button>("VBoxContainer/ButtonBoxContainer/Save");
		_closeButton = GetNode<Button>("VBoxContainer/ButtonBoxContainer/Close");
		
		_saveButton.Pressed += SaveButtonOnPressed;
		_closeButton.Pressed += CloseButtonOnPressed;
	}

	public void SetVariableData(Dictionary data)
	{
		_variableEdit.Text = data["Variable"].ToString();
		_valueEdit.Text = data["Value"].ToString();
		_tipsEdit.Text = data["Tooltips"].ToString();
	}

	public void Clear()
	{
		_variableEdit.Text = "";
		_valueEdit.Text = "";
		_tipsEdit.Text = "";
	}

	private void CloseButtonOnPressed()
	{
		Clear();
		Hide();
	}

	private void SaveButtonOnPressed()
	{
		Dictionary res = new()
		{
			{ "Variable", _variableEdit.Text },
			{ "Value", _valueEdit.Text },
			{ "Tooltips", _tipsEdit.Text },
		};
		
		EventVariableSaved.Invoke(res);
		
		Clear();
		Hide();
	}
}
