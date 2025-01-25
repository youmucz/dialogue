using Godot;
using Story.Dialogue.Core;

namespace Story.Dialogue.Runtime
{
	[Tool, GlobalClass]
	public partial class DialoguePlayer : Node
	{
		[Export] public DialogueGraphResource DialogueResource;
		
		private string _prevNode;
	
		// Called when the node enters the scene tree for the first time.
		public override void _Ready()
		{
			NodeMetaFactory.Initialize();
		}

		// Called every frame. 'delta' is the elapsed time since the previous frame.
		public override void _Process(double delta)
		{
			
		}

		public void LoadResource(DialogueGraphResource resource)
		{
			DialogueResource = resource;
			DialogueResource.Initialize();
		}
		
		/// <summary>
		/// 使用当前播放器上挂载的对话资源进行对话
		/// </summary>
		/// <returns></returns>
		public bool Play()
		{
			if (DialogueResource is null) return false;
			
			LoadResource(DialogueResource);
			DoPlay();
			return true;
		}

		/// <summary>
		/// 获取指定编号的本地对话资源文件(或已绑定当前播放器的对话资源)进行播放
		/// </summary>
		/// <param name="id">通常是本地配表编号，通过路径来索引对话资源文件</param>
		/// <returns></returns>
		public bool Play(int id)
		{
			return false;
		}
		
		/// <summary>
		/// 停止当前对话
		/// </summary>
		/// <returns></returns>
		public bool Stop()
		{
			if (DialogueResource is null) return false;

			var preNode = DialogueResource.GetNodeByName(_prevNode);
			preNode?.Stop();
			
			return true;
		}
		
		/// <summary>
		/// 进入指定节点的下一个节点
		/// </summary>
		/// <param name="name">节点名</param>
		public void Next(string name)
		{
			var preNode = DialogueResource.GetNodeByName(_prevNode);
			preNode?.NodeExited();

			var curNode = DialogueResource.GetNodeByName(name);
			
			// 选择节点等待玩家选择吼再触发
			if (curNode.NodeType == "OptionNode")
			{
				
			}
			else
			{
				var nextNodes = DialogueResource.GetNextNodeByName(_prevNode);
				// 默认除了选择节点外都只有一个子节点
				if (nextNodes.Count is 0) return;
				
				var node = nextNodes[0];
				_prevNode = node.NodeName;
				node.NodeEntered();
			}

		}

		private void DoPlay()
		{
			var root = DialogueResource.GetNodeByName("Root");
			_prevNode = root.NodeName;
			root.NodeEntered();
			Next(root.NodeName);
		}
	}
}
