using System;
using Godot;
using Story.Dialogue.Core.Data.Enums;

namespace Story.Dialogue.Core.Data
{
    public class DialogueConnection
    {
        public DialogueConnection(string fromNode, int fromPort, string toNode, int toPort)
        {
            FromNode = fromNode;
            FromPort = fromPort;
            ToNode = toNode;
            ToPort = toPort;
        }

        public string FromNode { get; }
        public int FromPort { get; }
        public string ToNode { get; }
        public int ToPort { get; }
        
        // 连接属性
        public Color LineColor { get; set; } = Colors.White;
        public float LineWidth { get; set; } = 2.0f;
        public ConnectionType ConnectionType { get; set; } = ConnectionType.Flow;
        
        // 条件连接
        public string Condition { get; set; } = "";
        public int Priority { get; set; } = 0;
        
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        
        public bool IsValid()
        {
            return !string.IsNullOrEmpty(FromNode) && !string.IsNullOrEmpty(ToNode) && FromNode != ToNode;
        }
        
        public override bool Equals(object obj)
        {
            if (obj is DialogueConnection other)
            {
                return FromNode == other.FromNode && ToNode == other.ToNode && FromPort == other.FromPort && ToPort == other.ToPort;
            }
            return false;
        }
        
        public override int GetHashCode()
        {
            return HashCode.Combine(FromNode, ToNode, FromPort, ToPort);
        }
    }
}
