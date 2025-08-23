namespace Story.Dialogue.Variable;

using System;
using Godot;
using Godot.Collections;
using Story.Dialogue.Core;
using Story.Dialogue.Utils;

[Tool]
public partial class VariableWindow : Control
{
	public VariableUtil.VariableWindowState State { get; set; } = VariableUtil.VariableWindowState.None;
	
	public Action<TreeItem> EventItemDoubleClick = delegate {};
	/// <summary> 若当前鼠标位置存在TreeItem，则返回其变量的数据,否则触发action </summary>
	public Action<TreeItem> EventItemAtPosition = delegate {};
	
    [Export] private Texture2D _variableAddButtonTexture2D;
    [Export] private Texture2D _variableRemoveButtonTexture2D;
    [Export] private Texture2D _variableEditButtonTexture2D;
    [Export] private Texture2D _localVariableType;
    [Export] private Texture2D _globalVariableType;
    [Export] private PackedScene _variablePopupPanel;
    [Export] private PackedScene _variablePopupMenuScene;

    private DialogueGraph _dialogueGraph;
    
    private TreeItem _localRootItem;
    private Tree _localVariableTree;
    private VariablePopupPanel _localVariablePopupPanel;
    private VariablePopupMenu _localVariablePopupMenu;
    private NinePatchRect _localVariableNinePatchRect;
    
    private TreeItem _globalRootItem;
    private Tree _globalVariableTree;
    private VariablePopupPanel _globalVariablePopupPanel;
    private VariablePopupMenu _globalVariablePopupMenu;
    private NinePatchRect _globalVariableNinePatchRect;
    
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
	    // 局部变量
	    _localVariableNinePatchRect = GetNode<NinePatchRect>("MainContainer/HSplitContainer/VBoxContainer/LocalVariableTree/NinePatchRect");
	    _localVariableTree = GetNode<Tree>("MainContainer/HSplitContainer/VBoxContainer/LocalVariableTree");
	    _localVariableTree.ButtonClicked += LocalVariableTreeOnButtonClicked;
	    _localVariableTree.EmptyClicked += LocalVariableTreeOnEmptyClicked;
	    _localVariableTree.ItemMouseSelected += LocalVariableTreeOnItemMouseSelected;
	    _localVariableTree.ItemActivated += LocalVariableTreeOnItemActivated;
	    _localVariableTree.ItemEdited += LocalVariableTreeOnItemEdited;
	    _localVariableTree.Draw += LocalVariableTreeOnDraw;
	    _localRootItem = _localVariableTree.CreateItem();
	    _localRootItem.SetText(0, "变量");
	    _localRootItem.SetText(1, "值");
	    _localRootItem.SetIcon(0, _localVariableType);
	    _localRootItem.SetTooltipText(0, "当前对话文件内存放的变量数据,只在一次对话生命周期内生效");
	    _localRootItem.AddButton(2, _variableAddButtonTexture2D, tooltipText:"添加局部变量");
	    _localRootItem.SetSelectable(0, false);
	    _localRootItem.SetSelectable(1, false);
	    _localRootItem.SetSelectable(2, false);
	    
	    _localVariablePopupPanel = _variablePopupPanel.Instantiate<VariablePopupPanel>();
	    _localVariablePopupPanel.Visible = false;
	    _localVariablePopupPanel.EventVariableSaved += SaveLocalVariable;
	    AddChild(_localVariablePopupPanel);
	    
	    _localVariablePopupMenu = _variablePopupMenuScene.Instantiate<VariablePopupMenu>();
	    _localVariablePopupMenu.IdPressed += LocalVariablePopupMenuOnIdPressed;
	    AddChild(_localVariablePopupMenu);
	    
	    // 全局变量
	    _globalVariableNinePatchRect = GetNode<NinePatchRect>("MainContainer/HSplitContainer/VBoxContainer2/GlobalVariableTree/NinePatchRect");
	    _globalVariableTree = GetNode<Tree>("MainContainer/HSplitContainer/VBoxContainer2/GlobalVariableTree");
	    _globalVariableTree.ButtonClicked += GlobalVariableTreeOnButtonClicked;
	    _globalVariableTree.EmptyClicked += GlobalVariableTreeOnEmptyClicked;
	    _globalVariableTree.ItemMouseSelected += GlobalVariableTreeOnItemMouseSelected;
	    _globalVariableTree.ItemActivated += GlobalVariableTreeOnItemActivated;
	    _globalVariableTree.ItemEdited += GlobalVariableTreeOnItemEdited;
	    _globalVariableTree.Draw += GlobalVariableTreeOnDraw;
	    _globalRootItem = _globalVariableTree.CreateItem();
	    _globalRootItem.SetText(0, "变量");
	    _globalRootItem.SetText(1, "值");
	    _globalRootItem.SetIcon(0, _globalVariableType);
	    _globalRootItem.SetTooltipText(0, "对话系统共用的变量数据,在整个游戏生命周期内生效");
	    _globalRootItem.AddButton(2, _variableAddButtonTexture2D, tooltipText:"添加全局变量");
	    _globalRootItem.SetSelectable(0, false);
	    _globalRootItem.SetSelectable(1, false);
	    _globalRootItem.SetSelectable(2, false);
	    
	    _globalVariablePopupPanel = _variablePopupPanel.Instantiate<VariablePopupPanel>();
	    _globalVariablePopupPanel.Visible = false;
	    _globalVariablePopupPanel.EventVariableSaved += SaveGlobalVariable;
	    AddChild(_globalVariablePopupPanel);
	    
	    _globalVariablePopupMenu = _variablePopupMenuScene.Instantiate<VariablePopupMenu>();
	    _globalVariablePopupMenu.IdPressed += GlobalVariablePopupMenuOnIdPressed;
	    AddChild(_globalVariablePopupMenu);
    }

    #region 局部变量

    /// <summary>
    /// 局部变量右键菜单
    /// </summary>
    /// <param name="mousePosition"></param>
    /// <param name="mouseButtonIndex"></param>
    private void LocalVariableTreeOnItemMouseSelected(Vector2 mousePosition, long mouseButtonIndex)
    {
	    var pos = mousePosition + _localVariableTree.GlobalPosition + GetWindow().Position;
	    
	    if (mouseButtonIndex == (long)MouseButton.Right)
	    {
		    var size = _localVariablePopupMenu.Size;
		    _localVariablePopupMenu.Popup(new Rect2I((int)pos.X, (int)pos.Y, size.X, size.Y));
	    }
    }
    
    /// <summary>
    /// 双击选中item修改数据
    /// </summary>
    private void LocalVariableTreeOnItemActivated()
    {
	    EventItemDoubleClick?.Invoke(_localVariableTree.GetSelected());
    }
    
    /// <summary>
    /// 同步修改后的数据
    /// </summary>
    private void LocalVariableTreeOnItemEdited()
    {
	    var item = _localVariableTree.GetSelected();
	    _dialogueGraph.Variables[item.GetText(0)]["Variable"] = item.GetText(0);
	    _dialogueGraph.Variables[item.GetText(0)]["Value"] = item.GetText(1);
    }
	
    /// <summary>
    /// 变量右键菜单点击事件函数
    /// </summary>
    /// <param name="id"></param>
    private void LocalVariablePopupMenuOnIdPressed(long id)
    {
	    switch (id)
	    {
		    case 0:
			    _localVariablePopupPanel.Show();
			    break;
		    case 1:
			    RemoveLocalVariable();
			    break;
		    case 2:
			    ResettingLocalVariable();
			    break;
	    }
    }

    /// <summary>
    /// 空白处点击
    /// </summary>
    /// <param name="clickPosition"></param>
    /// <param name="mouseButtonIndex"></param>
    /// <exception cref="NotImplementedException"></exception>
    private void LocalVariableTreeOnEmptyClicked(Vector2 clickPosition, long mouseButtonIndex)
    {
	    var pos = clickPosition + _localVariableTree.GlobalPosition + GetWindow().Position;

	    if (mouseButtonIndex == (long)MouseButton.Right)
	    {
		    var size = _localVariablePopupMenu.Size;
		    _localVariablePopupMenu.Popup(new Rect2I((int)pos.X, (int)pos.Y, size.X, size.Y));
	    }
	    else
	    {
		    _localVariableTree.DeselectAll();
	    }
    }

    /// <summary>
    /// 创建新局部变量
    /// </summary>
    /// <param name="item"></param>
    /// <param name="column"></param>
    /// <param name="id"></param>
    /// <param name="mouseButtonIndex"></param>
    private void LocalVariableTreeOnButtonClicked(TreeItem item, long column, long id, long mouseButtonIndex)
    {
	    if (_dialogueGraph == null) return;
	    
	    item.Select(0);
	    
	    switch (id)
	    {
		    case 0:
			    _localVariablePopupPanel.Show();
			    break;
		    case 2:	
			    ResettingLocalVariable(item);
			    break;
	    }
	    
    }
    
    /// <summary>
    /// 保存变量,如果变量不存在则新建，否则修改已有变量的数据
    /// </summary>
    /// <param name="data"></param>
    private void SaveLocalVariable(Dictionary data)
    {
	    var variable = (string)data["Variable"];

	    TreeItem item;
	    if (_dialogueGraph.Variables.ContainsKey(variable))
	    {
		    item = _localVariableTree.GetSelected();
	    }
	    else
	    {
		    item = _localRootItem.CreateChild();
		    item.AddButton(2, _variableEditButtonTexture2D, id: 2, tooltipText:"编辑当前变量");
	    }
	    
	    item.SetText(0, variable);
	    item.SetEditable(0, true);
	    item.SetTooltipText(0, (string)data["Tooltips"]);
	    item.SetText(1, (string)data["Value"]);
	    item.SetEditable(1, true);
	    
		_dialogueGraph.Variables[variable] = data;
    }
	
    /// <summary>
    /// 删除局部变量
    /// </summary>
    private void RemoveLocalVariable()
    {
	    var item = _localVariableTree.GetSelected();
	    
	    if (item == null) return;
	    
	    if (_dialogueGraph.Variables.ContainsKey(item.GetText(0)))
	    {
		    _dialogueGraph.Variables.Remove(item.GetText(0));
	    }
	    _localRootItem.RemoveChild(item);
    }
    
    /// <summary>
    /// 编辑当前选中的变量
    /// </summary>
    private void ResettingLocalVariable(TreeItem item=null)
    {
	    item ??= _localVariableTree.GetSelected();
	    
	    if (item == null) return;

	    if (_dialogueGraph.Variables.TryGetValue(item.GetText(0), out var data))
	    {
		    _localVariablePopupPanel.SetVariableData(data);
		    _localVariablePopupPanel.PopupCentered();
	    }
    }
    
    /// <summary>
    /// tree图形绘制回掉,因为找不到鼠标移动到item上的action，因此用该action进行替代
    /// </summary>
    private void LocalVariableTreeOnDraw()
    {
	    var item = _localVariableTree.GetItemAtPosition(_localVariableTree.GetLocalMousePosition());
	    
	    if (item != null && item != _localRootItem)
	    {
		    EventItemAtPosition(item);
	    }
    }

    #endregion
    
    #region 全局变量
    
    /// <summary>
    /// tree图形绘制回掉,因为找不到鼠标移动到item上的action，因此用该action进行替代
    /// </summary>
    private void GlobalVariableTreeOnDraw()
    {
	    var item = _globalVariableTree.GetItemAtPosition(_globalVariableTree.GetLocalMousePosition());
	    
	    if (item != null && item != _globalRootItem)
	    {
		    EventItemAtPosition(item);
	    }
    }
    
    /// <summary>
    /// 空白处点击
    /// </summary>
    /// <param name="clickPosition"></param>
    /// <param name="mouseButtonIndex"></param>
    /// <exception cref="NotImplementedException"></exception>
    private void GlobalVariableTreeOnEmptyClicked(Vector2 clickPosition, long mouseButtonIndex)
    {
	    var pos = clickPosition + _globalVariableTree.GlobalPosition + GetWindow().Position;

	    if (mouseButtonIndex == (long)MouseButton.Right)
	    {
		    var size = _localVariablePopupMenu.Size;
		    _globalVariablePopupMenu.Popup(new Rect2I((int)pos.X, (int)pos.Y, size.X, size.Y));
	    }
	    else
	    {
		    _globalVariableTree.DeselectAll();
	    }
    }
	
    /// <summary>
    /// 同步修改后的数据
    /// </summary>
    private void GlobalVariableTreeOnItemEdited()
    {
	    var item = _globalVariableTree.GetSelected();
	    // _dialogueGraphResource.GlobalVariables[item.GetText(0)]["Variable"] = item.GetText(0);
	    // _dialogueGraphResource.GlobalVariables[item.GetText(0)]["Value"] = item.GetText(1);
    }

    private void GlobalVariableTreeOnItemActivated()
    {
	    EventItemDoubleClick?.Invoke(_localVariableTree.GetSelected());
    }

    private void GlobalVariableTreeOnButtonClicked(TreeItem item, long column, long id, long mouseButtonIndex)
    {
	    if (_dialogueGraph == null) return;
	    
	    item.Select(0);
	    
	    switch (id)
	    {
		    case 0:
			    _globalVariablePopupPanel.Show();
			    break;
		    case 2:	
			    ResettingGlobalVariable(item);
			    break;
	    }
    }
    
    private void GlobalVariableTreeOnItemMouseSelected(Vector2 mousePosition, long mouseButtonIndex)
    {
	    var pos = mousePosition + _globalVariableTree.GlobalPosition + GetWindow().Position;
	    
	    if (mouseButtonIndex == (long)MouseButton.Right)
	    {
		    var size = _globalVariablePopupMenu.Size;
		    _globalVariablePopupMenu.Popup(new Rect2I((int)pos.X, (int)pos.Y, size.X, size.Y));
	    }
    }
    
    /// <summary>
    /// 全局变量右键菜单点击事件函数
    /// </summary>
    /// <param name="id"></param>
    private void GlobalVariablePopupMenuOnIdPressed(long id)
    {
	    switch (id)
	    {
		    case 0:
			    _globalVariablePopupPanel.Show();
			    break;
		    case 1:
			    OnGlobalVariableRemoved();
			    break;
		    case 2:
			    ResettingGlobalVariable();
			    break;
	    }
    }
    
    /// <summary>
    /// 保存新建变量
    /// </summary>
    /// <param name="data"></param>
    private void SaveGlobalVariable(Dictionary data)
    {
	    var variable = (string)data["Variable"];
		
	    // TODO: 后续要改成全局变量池
	    TreeItem item;
	    if (_dialogueGraph.GlobalVariables.ContainsKey(variable))
	    {
		    item = _globalVariableTree.GetSelected();
	    }
	    else
	    {
		    item = _globalRootItem.CreateChild();
		    item.AddButton(2, _variableEditButtonTexture2D, id: 2, tooltipText:"编辑当前变量");
	    }
	    
	    item.SetText(0, variable);
	    item.SetEditable(0, true);
	    item.SetTooltipText(0, (string)data["Tooltips"]);
	    item.SetText(1, (string)data["Value"]);
	    item.SetEditable(1, true);
	    
	    // _dialogueGraphResource.GlobalVariables[variable] = data;
    }
    
    /// <summary>
    /// 删除全局变量
    /// </summary>
    private void OnGlobalVariableRemoved()
    {
	    var item = _globalVariableTree.GetSelected();
	    _globalRootItem.RemoveChild(item);
    }
    
    /// <summary>
    /// 编辑当前选中的变量
    /// </summary>
    private void ResettingGlobalVariable(TreeItem item=null)
    {
	    item ??= _globalVariableTree.GetSelected();
		
	    // TODO: 后续应该编辑全局变量池
	    if (_dialogueGraph.GlobalVariables.TryGetValue(item.GetText(0), out var data))
	    {
		    _globalVariablePopupPanel.SetVariableData(data);
		    _globalVariablePopupPanel.PopupCentered();
	    }
    }

    #endregion
    
    /// <summary>
    /// 加载当前对话的变量数据
    /// </summary>
    /// <param name="resource"></param>
    public void LoadVariables(DialogueGraph resource)
    {
	    ClearVariables(resource);
	    
	    _dialogueGraph = resource;

	    foreach (var pair in _dialogueGraph.Variables)
	    {
		    var item = _localRootItem.CreateChild();
		    item.SetText(0, (string)pair.Value["Variable"]);
		    item.SetEditable(0, true);
		    item.SetTooltipText(0, (string)pair.Value["Tooltips"]);
		    item.SetText(1, (string)pair.Value["Value"]);
		    item.SetEditable(1, true);
		    item.AddButton(2, _variableEditButtonTexture2D, id: 2, tooltipText: "编辑当前变量");
	    }
    }
	
    /// <summary>
    /// 关掉截点图时清理当前变量窗口数据
    /// </summary>
    /// <param name="resource"></param>
    public void ClearVariables(DialogueGraph resource)
    {
	    _dialogueGraph = null;

	    foreach (var item in _localRootItem.GetChildren())
	    {
		    _localRootItem.RemoveChild(item);
	    }
    }

    public void SetState(VariableUtil.VariableWindowState state)
    {
	    State = state;

	    if (state == VariableUtil.VariableWindowState.Searching)
	    {
		    _localVariableNinePatchRect.SetVisible(true);
		    _globalVariableNinePatchRect.SetVisible(true);
	    }
	    else
	    {
		    _localVariableNinePatchRect.SetVisible(false);
		    _globalVariableNinePatchRect.SetVisible(false);
	    }
    }
}
