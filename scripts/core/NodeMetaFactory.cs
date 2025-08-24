using Godot;
using System;
using System.Collections.Generic;
using System.Reflection;
using Story.Dialogue.Core.Data;
using Story.Dialogue.Graph;

namespace Story.Dialogue.Core
{
	[Tool]
	public class NodeMetaAttribute : Attribute
	{
		// 要通过GetCustomAttributes去获取属性的特性标记,需要给属性加上{get;set;}
	}

	public class NodeMetaFactory
	{
		public static readonly Dictionary<string, List<Dictionary<string, string>>> NodeMenu = new();
		public static readonly Dictionary<string, Type> NodeMetaTypes = new();

		private static bool _isInit;

		public static void Initialize()
		{
			if (_isInit) return;

			// 获取程序集
			var assembly = Assembly.GetExecutingAssembly();
			// 获取程序集中的所有类型
			var types = assembly.GetTypes();

			// 遍历所有类
			foreach (var type in types)
			{
				if (!type.IsSubclassOf(typeof(DialogueNode))) continue;

				string nodeType = null;
				string nodeCategory = null;

				// 获取类中已经初始化的参数属性
				var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
				foreach (var property in properties)
				{
					if (property.Name == "NodeType")
						nodeType = (string)property.GetValue(Activator.CreateInstance(type));
					if (property.Name == "NodeCategory")
						nodeCategory = (string)property.GetValue(Activator.CreateInstance(type));
					// if (property.Character == "NodeName") nodeName = (string)property.GetValue(Activator.CreateInstance(type));
				}

				if (nodeType == null || nodeCategory == null) continue;

				// 存储meta类型
				NodeMetaTypes.TryAdd(nodeType, type);

				if (nodeType == "Root") continue;
				if (NodeMenu.TryGetValue(nodeCategory, out var categories))
				{
					var map = new Dictionary<string, string> { { "NodeType", nodeType } };
					categories.Add(map);
				}
				else
				{
					var map = new Dictionary<string, string> { { "NodeType", nodeType } };
					var newTypes = new List<Dictionary<string, string>> { map };
					NodeMenu.Add(nodeCategory, newTypes);
				}
			}

			_isInit = true;
		}

		public static DialogueNode GetNodeMeta(string nodeType, DialogueGraph resource, Godot.Collections.Dictionary data)
		{
			if (NodeMetaTypes.TryGetValue(nodeType, out var type))
			{
				var instance = (DialogueNode)Activator.CreateInstance(type, new object[] { resource, data });
				return instance;
			}

			return null;
		}

		public static DialogueNode GetNodeMeta(string nodeType, DialogueGraphNode node, DialogueGraph resource, Godot.Collections.Dictionary data)
		{
			if (NodeMetaTypes.TryGetValue(nodeType, out var type))
			{
				var instance = (DialogueNode)Activator.CreateInstance(type, new object[] { resource, node, data });
				return instance;
			}

			return null;
		}
	}
}
