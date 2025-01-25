using Story.Dialogue.Editor;

namespace Story.Dialogue.Graph;
using Godot;
using Godot.Collections;
using Story.Dialogue.Core;


/// <summary>
/// Inherited from GraphNode which is edit-time scene's script.
/// </summary>
[Tool]
public partial class DialogueGraphNode : GraphNode
{
	public MainWindow MainWindow { get; set; }
	/// <summary>
	/// 节点的数据结构和业务逻辑
	/// </summary>
	public DialogueNodeBase Base;
	
    private DialogueGraphEdit _graphEdit;
    
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
	    MainWindow = DialoguePlugin.GetMainWindow();
    }

    public override void _GuiInput(InputEvent @event)
    {
    	base._GuiInput(@event);
    	
    	// 4.2版本没有右键节点事件,所以自己写一个
    	if (@event is InputEventMouseButton { ButtonIndex: MouseButton.Right })
    	{
    		if (!@event.IsPressed()) return;
    		Selected = true;
    	}
    }

    /// <summary>
    /// 添加进GraphEdit前需要先初始化部分参数
    /// </summary>
    /// <param name="graphEdit"></param>
    /// <param name="data"></param>
    public void Initialize(DialogueGraphEdit graphEdit, Dictionary data)
    {
	    _graphEdit = graphEdit;
	    _graphEdit.NodeRemoved += OnNodeRemoved;
	    
	    var nodeType = (string)data["NodeType"];
	    Base = NodeMetaFactory.GetNodeMeta(nodeType, this, _graphEdit.DialogGraphResource, data);
	    Base.Initialize(this);
    }
    
    /// <summary>
    /// 添加进GraphEdit前需要先初始化部分参数
    /// </summary>
    /// <param name="graphEdit"></param>
    /// <param name="base"></param>
    public void Initialize(DialogueGraphEdit graphEdit, DialogueNodeBase @base)
    {
	    _graphEdit = graphEdit;
	    _graphEdit.NodeRemoved += OnNodeRemoved;
	    
	    Base = @base;
	    Base.Initialize(this);
    }

    protected virtual void OnNodeRemoved(DialogueGraphNode node)
    {
	    
    }
}
