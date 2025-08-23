namespace Story.Dialogue.Graph;

using Godot;
using System;
using System.Collections.Generic;
using Godot.Collections;
using Story.Dialogue.Utils;
using Story.Dialogue.Variable;


[Tool]
public partial class ConditionGraphNode : DialogueGraphNode
{
    [Export] public Texture2D AddButtonIcon;
    [Export] public PackedScene LinkButton;
    [Export] public PackedScene TypeButton;
    [Export] public PackedScene ConditionItemScene;
    
    public ConditionNode Node;
    
    private Button _addButton = new ();
    private NinePatchRect _vTypeNinePatchRect;  // 变量类型存放面板
    private NinePatchRect _ninePatchRect;  // 链接按钮面板
    private HBoxContainer _titleContainer;
    private VBoxContainer _variableContainer;
    private Array<ConditionItem> _variables = new ();
    private Array<TextureButton> _linkButtons = new();
    
    public override void _Ready()
    {
	    base._Ready();
	    
        _variableContainer = GetNode<VBoxContainer>("HBoxContainer/VBoxContainer");
        _ninePatchRect = GetNode<NinePatchRect>("HBoxContainer/NinePatchRect");
        _vTypeNinePatchRect = GetNode<NinePatchRect>("HBoxContainer/VTypeNinePatchRect");
        
        _addButton.Icon = AddButtonIcon;
        _addButton.Pressed += AddButtonOnPressed;
		
        _titleContainer = GetTitlebarHBox();
        _titleContainer.AddChild(_addButton);
		
        Node = Base as ConditionNode;
        
        if (Node != null)
        {
            foreach (var kvp in Node.Variables)
            {
	            CreateVariableEdit(kvp.Key, kvp.Value);
            }
        }
    }
	
    /// <summary>
    /// 添加条件判断模块
    /// </summary>
    private void AddButtonOnPressed()
    {
        CreateVariableEdit(_variables.Count, VariableUtil.Default.Duplicate());
    }
	
    /// <summary>
    /// 添加新变量
    /// </summary>
    /// <param name="variableIndex"></param>
    /// <param name="data"></param>
    private void CreateVariableEdit(int variableIndex, Dictionary data)
    {
        ConditionItem variable;
        if (variableIndex == 0)
        {
	        variable = GetNode<ConditionItem>("HBoxContainer/VBoxContainer/ConditionItem");
	        _vTypeNinePatchRect.AddChild(TypeButton.Instantiate<TextureButton>());
        }
        else
        {
	        variable = ConditionItemScene.Instantiate<ConditionItem>();
	        _variableContainer.AddChild(variable);
	        CreateLinkButton();
        }

        Node.Variables.TryAdd(variableIndex, data);
		
        _variables.Add(variable);
        variable.Initialize(variableIndex, data);
        variable.EventValueChanged += VariableOnEventValueChanged;
        variable.EventVariableChanged += VariableOnEventVariableChanged;
        variable.EventLogicalOperatorsChanged += VariableOnEventLogicalOperatorsChanged;
        variable.EventComparisonOperatorsChanged += VariableOnEventComparisonOperatorsChanged;
        variable.EventDelButtonPressed += VariableOnEventDelButtonPressed;
    }
	
    /// <summary>
    /// 创建链接按钮
    /// </summary>
    public void CreateLinkButton()
    {
	    // 1.逻辑链接按钮
        _ninePatchRect.AddChild(LinkButton.Instantiate<TextureButton>());
        for (var i = 0; i < _ninePatchRect.GetChildCount(); ++i)
        {
	        var linkButton = _ninePatchRect.GetChild<TextureButton>(i);
	        var position = new Vector2(
		        linkButton.Position.X, 
		        (float)i / _ninePatchRect.GetChildCount() * _ninePatchRect.GetSize().Y + linkButton.GetSize().Y / 2
	        );
	        linkButton.Position = position;
        }
        
        // 2.节点类型按钮
        _vTypeNinePatchRect.AddChild(TypeButton.Instantiate<TextureButton>());
        for (var i = 0; i < _vTypeNinePatchRect.GetChildCount(); ++i)
        {
	        var linkButton = _vTypeNinePatchRect.GetChild<TextureButton>(i);
	        var position = new Vector2(
		        linkButton.Position.X, 
		        (float)i / _vTypeNinePatchRect.GetChildCount() * _vTypeNinePatchRect.GetSize().Y
	        );
	        linkButton.Position = position;
        }
    }
	
    /// <summary>
    /// 删除链接按钮
    /// </summary>
    public void RemoveLinkButton()
    {
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
        
        // 2.变量类型
        _vTypeNinePatchRect.RemoveChild(_vTypeNinePatchRect.GetChild(_vTypeNinePatchRect.GetChildCount() - 1));
        
        for (var i = 0; i < _vTypeNinePatchRect.GetChildCount(); ++i)
        {
	        var linkButton = _vTypeNinePatchRect.GetChild<TextureButton>(i);
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
    private void VariableOnEventLogicalOperatorsChanged(int index, int idx, string obj)
    {
        if (Node.Variables.TryGetValue(index, out var value))
        {
	        value["LogicalOperators"] = obj;
	        value["LogicalOperatorsIdx"] = idx;
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
		RemoveLinkButton();
        ResetSize();
    }
}
