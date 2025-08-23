namespace Story.Dialogue.Utils;

using System;
using Godot;
using Godot.Collections;

[Tool]
public partial class VariableUtil : Node
{
	public enum VariableWindowState
	{
		None,
		Searching
	}
	
	public static readonly Dictionary Default = new()
	{
		{ "Value", "" }, { "Variable", "" }, 
		{ "LogicalOperators", "AND" }, {"LogicalOperatorsIdx", 0},
		{ "ComparisonOperators", "==" }, {"ComparisonOperatorsIdx", 0},
	};
	
	public static readonly Dictionary VariableDefault = new()
	{
		{ "Value", "" }, { "Variable", "" }, 
		{ "ArithmeticOperators", "==" }, {"ArithmeticOperatorsIdx", 0},
	};
}
