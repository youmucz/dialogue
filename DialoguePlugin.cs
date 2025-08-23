using Godot;
using Story.Dialogue.Core;

namespace Story.Dialogue.Editor
{
	[Tool]
	public partial class DialoguePlugin : EditorPlugin
	{
		private static MainWindow _mainWindowInstance;
		private PackedScene _mainWindow = ResourceLoader.Load<PackedScene>("res://addons/dialogue/scenes/editor/main_window.tscn");
	
		public override void _EnterTree()
		{
			// Initialization of the plugin goes here.
			_mainWindowInstance = _mainWindow.Instantiate<MainWindow>();
		
			// Add the main panel to the editor's main viewport.
			EditorInterface.Singleton.GetEditorMainScreen().AddChild(_mainWindowInstance);
			// Hide the main panel. Very much required.
			_MakeVisible(false);
		
			NodeMetaFactory.Initialize();
		}

		public override void _ExitTree()
		{
			_mainWindowInstance?.QueueFree();
		}

		public override bool _HasMainScreen()
		{
			return true;
		}

		public override void _MakeVisible(bool visible)
		{
			if (_mainWindowInstance != null)
			{
				_mainWindowInstance.Visible = visible;
			}
		}

		public override string _GetPluginName()
		{
			return "Dialogue";
		}

		public override Texture2D _GetPluginIcon()
		{
			return EditorInterface.Singleton.GetEditorTheme().GetIcon("Node", "EditorIcons");
		}

		public static MainWindow GetMainWindow()
		{
			return _mainWindowInstance;
		}
	}
}
