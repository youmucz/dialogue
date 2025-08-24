namespace Story.Dialogue.Core.Data.Enums
{
    public enum DialogueNodeType
    {
        // 流程控制
        Start,
        End,
        Jump,
        
        // 对话节点
        Text,
        Choice,
        Narrator,
        
        // 逻辑节点
        Condition,
        Variable,
        Random,
        Switch,
        
        // 事件节点
        Event,
        Quest,
        Script,
        
        // 特殊节点
        Comment,
        Group,
        SubGraph
    }
    
    public enum PortType
    {
        Flow,      // 流程端口
        Data,      // 数据端口
        Event,     // 事件端口
        Condition  // 条件端口
    }
    
    public enum ConnectionType
    {
        Flow,      // 普通流程连接
        Data,      // 数据连接
        Event,     // 事件连接
        Condition  // 条件连接
    }
    
    public enum PropertyType
    {
        String,
        Int,
        Float,
        Bool,
        Vector2,
        Vector3,
        Color,
        Resource,
        Enum,
        Array,
        Dictionary
    }
}
