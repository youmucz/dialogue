namespace Story.Dialogue.Variable;
using Godot;
using System;
using Godot.Collections;

[Tool]
public partial class ConditionItem : BoxContainer
{
	public int VariableIndex;
	public event Action<int> EventDelButtonPressed = delegate { };
	public event Action<int, string> EventVariableChanged = delegate { };
	public event Action<int, string> EventValueChanged = delegate { };
	public event Action<int, int, string> EventComparisonOperatorsChanged = delegate { };
	public event Action<int, int, string> EventLogicalOperatorsChanged = delegate { };
	
	private LineEdit _value;
	private LineEdit _variable;
	private TextureButton _delButton;
	private OptionButton _logicalOperators;
	private OptionButton _comparisonOperators;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_value = GetNode<LineEdit>("Value");
		_variable = GetNode<LineEdit>("Variable");
		_delButton = GetNode<TextureButton>("DelButton");
		_logicalOperators = GetNode<OptionButton>("LogicalOperators");
		_comparisonOperators = GetNode<OptionButton>("ComparisonOperators");
		
		_delButton.Pressed += DelButtonOnPressed;
		_value.TextChanged += ValueOnTextChanged;
		_variable.TextChanged += VariableOnTextChanged;
		_logicalOperators.ItemSelected += LogicalOperatorsOnItemSelected;
		_comparisonOperators.ItemSelected += ComparisonOperatorsOnItemSelected;
	}

	public void Initialize(int index, Dictionary data)
	{
		VariableIndex = index;
		_value.Text = (string)data["Value"];
		_variable.Text = (string)data["Variable"];
		_logicalOperators.Select((int)data["LogicalOperatorsIdx"]);
		_comparisonOperators.Select((int)data["ComparisonOperatorsIdx"]);
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
			{ "LogicalOperators", _logicalOperators.GetText() },
			{ "LogicalOperatorsIdx", _logicalOperators.GetSelectedId() },
			{ "ComparisonOperators", _comparisonOperators.GetText() },
			{ "ComparisonOperatorsIdx", _comparisonOperators.GetSelectedId() },
		};
	}
	
	/// <summary>
	/// 操作符
	/// </summary>
	/// <param name="index"></param>
	private void ComparisonOperatorsOnItemSelected(long index)
	{
		EventComparisonOperatorsChanged?.Invoke(VariableIndex, (int)index, _comparisonOperators.GetText());
	}
	
	/// <summary>
	/// 定义多个变量之间的逻辑运算
	/// </summary>
	/// <param name="index"></param>
	private void LogicalOperatorsOnItemSelected(long index)
	{
		EventLogicalOperatorsChanged?.Invoke(VariableIndex, (int)index, _logicalOperators.GetText());
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
