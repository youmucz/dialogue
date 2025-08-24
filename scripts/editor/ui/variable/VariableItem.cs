using Godot;
using System;
using Godot.Collections;

namespace Story.Dialogue.Variable;

[Tool]
public partial class VariableItem : BoxContainer
{
	public int VariableIndex;
	
	public event Action<int> EventDelButtonPressed = delegate { };
	public event Action<int, string> EventValueChanged = delegate { };
	public event Action<int, string> EventVariableChanged = delegate { };
	public event Action<int, int, string> EventArithmeticOperatorsChanged = delegate { };
	
	private LineEdit _value;
	private LineEdit _variable;
	private TextureButton _delButton;
	private OptionButton _arithmeticOperators;
	
// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_value = GetNode<LineEdit>("Value");
		_variable = GetNode<LineEdit>("Variable");
		_delButton = GetNode<TextureButton>("DelButton");
		_arithmeticOperators = GetNode<OptionButton>("ArithmeticOperators");
		
		_delButton.Pressed += DelButtonOnPressed;
		_value.TextChanged += ValueOnTextChanged;
		_variable.TextChanged += VariableOnTextChanged;
		_arithmeticOperators.ItemSelected += ArithmeticOperatorsOnItemSelected;
	}

	public void Initialize(int index, Dictionary data)
	{
		VariableIndex = index;
		_value.Text = (string)data["Value"];
		_variable.Text = (string)data["Variable"];
		_arithmeticOperators.Select((int)data["ArithmeticOperatorsIdx"]);
	}

	public void SetVariableData(string variable, string value)
	{
		_value.SetText(value);
		_variable.SetText(variable);
		EventVariableChanged?.Invoke(VariableIndex, variable);
		EventValueChanged?.Invoke(VariableIndex, value);
	}

	public Dictionary GetVariableData()
	{
		return new Dictionary()
		{
			{ "Value", _value.Text }, 
			{ "Variable", _variable.Text }, 
			{ "ArithmeticOperators", _arithmeticOperators.GetText() },
			{ "ArithmeticOperatorsIdx", _arithmeticOperators.GetSelectedId() },
		};
	}
	
	/// <summary>
	/// 定义多个变量之间的逻辑运算
	/// </summary>
	/// <param name="index"></param>
	private void ArithmeticOperatorsOnItemSelected(long index)
	{
		EventArithmeticOperatorsChanged?.Invoke(VariableIndex, (int)index, _arithmeticOperators.GetText());
	}
	
	/// <summary>
	/// 变量
	/// </summary>
	/// <param name="newText"></param>
	private void VariableOnTextChanged(string newText)
	{
		EventVariableChanged?.Invoke(VariableIndex, newText);
	}
	
	/// <summary>
	/// 变量值
	/// </summary>
	/// <param name="newText"></param>
	private void ValueOnTextChanged(string newText)
	{
		EventValueChanged?.Invoke(VariableIndex, newText);
	}
	
	private void DelButtonOnPressed()
	{
		EventDelButtonPressed?.Invoke(VariableIndex);
	}
}
