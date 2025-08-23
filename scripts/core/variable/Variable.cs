using Story.Dialogue.Utils;

namespace Story.Dialogue.Variable;
using Story.Dialogue.Graph;
using Godot;

[Tool]
public partial class Variable : LineEdit
{
	private TextureButton _addButton;
	private ConditionItem _conditionItem;
	private ConditionGraphNode _conditionNode;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_addButton = GetNode<TextureButton>("AddButton");
		_addButton.Toggled += AddButtonOnToggled;
		
		_conditionItem = GetParentOrNull<ConditionItem>();
		_conditionNode = (ConditionGraphNode)_conditionItem?.FindParent("Condition");
		if (_conditionNode != null) _conditionNode.NodeDeselected += ConditionNodeOnNodeDeselected;
	}

	private void ConditionNodeOnNodeDeselected()
	{
		_addButton.SetPressed(false);
	}

	private void AddButtonOnToggled(bool toggledOn)
	{
		var variableWindow = _conditionNode.MainWindow.GetVariableWindow();
		
		if (toggledOn)
		{
			variableWindow.SetState(VariableUtil.VariableWindowState.Searching);
			variableWindow.EventItemAtPosition += EventItemAtPosition;
			variableWindow.EventItemDoubleClick += EventItemDoubleClick;
			_conditionNode.SetSelected(true);
			_conditionNode.MainWindow.SetSidePanelVisible(true);
		}
		else
		{
			variableWindow.SetState(VariableUtil.VariableWindowState.None);
			variableWindow.EventItemAtPosition -= EventItemAtPosition;
			variableWindow.EventItemDoubleClick -= EventItemDoubleClick;
		}
		
	}

	private void EventItemDoubleClick(TreeItem item)
	{
		_conditionItem.SetVariableData(item.GetText(0), item.GetText(1));
	}

	private void EventItemAtPosition(TreeItem item)
	{
		
	}
}
