using System.Collections.Generic;
using Godot;
using Godot.Collections;
using Story.Dialogue.Utils;
using Story.Dialogue.Variable;

namespace Story.Dialogue.Graph;

/// <summary>
/// 变量节点,用来设置、修改和存储当前对话拥有的变量属性
/// </summary>
[Tool]
public partial class VariableGraphNode : DialogueGraphNode
{
	[Export] public Texture2D AddButtonIcon;
	[Export] public PackedScene TypeButton;
	[Export] public PackedScene VariableItemScene;
	
	public VariableNode Node;
	
	private Button _addButton = new ();
	private NinePatchRect _ninePatchRect;  // 变量类型存放面板
	private HBoxContainer _titleContainer;
	private VBoxContainer _variableContainer;
	private Array<VariableItem> _variables = new ();
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		base._Ready();
	    
		_variableContainer = GetNode<VBoxContainer>("HBoxContainer/VBoxContainer");
		_ninePatchRect = GetNode<NinePatchRect>("HBoxContainer/NinePatchRect");
        
		_addButton.Icon = AddButtonIcon;
		_addButton.Pressed += AddButtonOnPressed;
		
		_titleContainer = GetTitlebarHBox();
		_titleContainer.AddChild(_addButton);
		
		Node = Base as VariableNode;
		
		if (Node != null)
		{
			foreach (var kvp in Node.Variables)
			{
				CreateVariableItem(kvp.Key, kvp.Value);
			}
		}
	}
	
	/// <summary>
	/// 添加条件判断模块
	/// </summary>
	private void AddButtonOnPressed()
	{
		CreateVariableItem(_variables.Count, VariableUtil.VariableDefault.Duplicate());
	}
	
	/// <summary>
    /// 添加新变量
    /// </summary>
    /// <param name="variableIndex"></param>
    /// <param name="data"></param>
    private void CreateVariableItem(int variableIndex, Dictionary data)
    {
        VariableItem variable;
        if (variableIndex == 0)
        {
	        variable = GetNode<VariableItem>("HBoxContainer/VBoxContainer/VariableItem");
	        _ninePatchRect.AddChild(TypeButton.Instantiate<TextureButton>());
        }
        else
        {
	        variable = VariableItemScene.Instantiate<VariableItem>();
	        _variableContainer.AddChild(variable);
	        CreateTypeButton();
        }

        Node.Variables.TryAdd(variableIndex, data);
		
        _variables.Add(variable);
        variable.Initialize(variableIndex, data);
        variable.EventValueChanged += VariableOnEventValueChanged;
        variable.EventVariableChanged += VariableOnEventVariableChanged;
        variable.EventArithmeticOperatorsChanged += VariableOnEventArithmeticOperatorsChanged;
        variable.EventDelButtonPressed += VariableOnEventDelButtonPressed;
    }
	
    /// <summary>
    /// 创建变量类型按钮
    /// </summary>
    public void CreateTypeButton()
    {
	    _ninePatchRect.AddChild(TypeButton.Instantiate<TextureButton>());
        for (var i = 0; i < _ninePatchRect.GetChildCount(); ++i)
        {
	        var linkButton = _ninePatchRect.GetChild<TextureButton>(i);
	        var position = new Vector2(
		        linkButton.Position.X, 
		        (float)i / _ninePatchRect.GetChildCount() * _ninePatchRect.GetSize().Y
	        );
	        linkButton.Position = position;
        }
    }
	
    /// <summary>
    /// 删除变量类型按钮
    /// </summary>
    public void RemoveTypeButton()
    {
        // 2.变量类型
        _ninePatchRect.RemoveChild(_ninePatchRect.GetChild(_ninePatchRect.GetChildCount() - 1));
        
        for (var i = 0; i < _ninePatchRect.GetChildCount(); ++i)
        {
	        var linkButton = _ninePatchRect.GetChild<TextureButton>(i);
	        var position = new Vector2(
		        linkButton.Position.X, 
		        i* 33
	        );
	        linkButton.Position = position;
        }
    }
	
    /// <summary>
    /// 逻辑运算
    /// </summary>
    /// <param name="index"></param>
    /// <param name="idx"></param>
    /// <param name="obj"></param>
    private void VariableOnEventArithmeticOperatorsChanged(int index, int idx, string obj)
    {
        if (Node.Variables.TryGetValue(index, out var value))
        {
	        value["ArithmeticOperators"] = obj;
	        value["ArithmeticOperatorsIdx"] = idx;
        }
    }

    /// <summary>
    /// 比较运算
    /// </summary>
    /// <param name="index">变量序号</param>
    /// <param name="idx">操作符号序号</param>
    /// <param name="obj">操作符号</param>
    private void VariableOnEventComparisonOperatorsChanged(int index, int idx, string obj)
    {
        if (Node.Variables.TryGetValue(index, out var value))
        {
	        value["ComparisonOperators"] = obj;
	        value["ComparisonOperatorsIdx"] = idx;
        }
    }

    /// <summary>
    /// 变量值
    /// </summary>
    /// <param name="index"></param>
    /// <param name="obj"></param>
    private void VariableOnEventValueChanged(int index, string obj)
    {
        if (Node.Variables.TryGetValue(index, out var value))
        {
	        value["Value"] = obj;
        }
    }

    /// <summary>
    /// 变量
    /// </summary>
    /// <param name="index"></param>
    /// <param name="obj"></param>
    private void VariableOnEventVariableChanged(int index, string obj)
    {
        if (Node.Variables.TryGetValue(index, out var value))
        {
	        value["Variable"] = obj;
        }
    }
	
    /// <summary>
    /// 删除变量
    /// </summary>
    /// <param name="index"></param>
    private void VariableOnEventDelButtonPressed(int index)
    {
        // 默认初始选项不可删除
        if (index <= 0 || index >= _variables.Count) return;
		
        var oldVariable = _variables[index];
		
        Node.Variables.Clear();
        _variables.RemoveAt(index);
		
        for (var i = 0; i < _variables.Count; i++)
        {
	        var variable = _variables[i];
	        variable.VariableIndex = i;
	        Node.Variables.Add(i, variable.GetVariableData());
        }
        
		_variableContainer.RemoveChild(oldVariable);
		RemoveTypeButton();
        ResetSize();
    }
}
