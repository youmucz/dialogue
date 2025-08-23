namespace Story.Dialogue.Editor;
using Godot;
using Godot.Collections;
using Story.Dialogue.Core;
using Story.Dialogue.Graph;


[Tool, GlobalClass, Icon("res://addons/dialogue/resources/icons/file_manager.svg")]
public partial class FileManager : Node
{
	public MainWindow MainWindow;
	public Workspace Workspace;
	
	[Export] private PackedScene _graphEdit;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		
	}
	
	public void NewFile(string dir, string filename, string filepath)
	{
		var tabIndex = Workspace.GetTabEditor(filepath);
		
		if (tabIndex >= 0)
		{
			Workspace.SetCurrentTab(tabIndex);
		}
		else
		{
			CreateFile(dir, filename, filepath);
		}
	}

	public void OpenFile(string dir, string filename, string filepath)
	{
		/*
		var filepath = Path.Combine(dir, filename).Replace("\\", "/");
		*/
		var tabIndex = Workspace.GetTabEditor(filepath);
		
		if (tabIndex >= 0)
		{
			Workspace.SetCurrentTab(tabIndex);
		}
		else
		{
			var data = ResourceLoader.Load<DialogueGraph>(filepath, "", ResourceLoader.CacheMode.Replace);
			data.FileDir = dir;
			data.Filename = filename;
			data.Filepath = filepath;
			
			var editor = _graphEdit.Instantiate<DialogueGraphEdit>();
			editor.LoadData(data);
			Workspace.AddEditor(editor);
		}
		
		GD.Print("打开文件:", filename);
	}

	/// <summary>
	/// Create local res file.
	/// </summary>
	/// <param name="dir"></param>
	/// <param name="filename"></param>
	/// <param name="filepath"></param>
	public void CreateFile(string dir, string filename, string filepath)
	{
		var editor = _graphEdit.Instantiate<DialogueGraphEdit>();
		
		var nodeData = new Array<Dictionary>(){
			new Dictionary
			{
				{"NodeType", "Root"}, 
				{"NodeName", "Root"}, 
				{"NodeCategory", "Root"},
				{"NodePositionOffset", Vector2.Zero},
			}
		};
	
		var data = new DialogueGraph()
		{
			FileDir = dir,
			Filename = filename,
			Filepath = filepath,
			NodeData = nodeData,
		};
		
		editor.LoadData(data);
		Workspace.AddEditor(editor);
		var error = ResourceSaver.Save(data, filepath);
		
		if (error == Error.Ok)
		{
			GD.Print("New Dialogue create successfully!");
		}
		else
		{
			GD.Print("New Dialogue create failed. Error: " + error + " path:" + filepath);
		}
	}

	public void SaveFile()
	{
		var editor = Workspace.GetCurrentEditor();
		
		if (editor == null) return;
		
		var data = editor.DumpsData();
		var error = ResourceSaver.Save(data, data.Filepath);
		if (error == Error.Ok)
		{
			GD.Print("Dialogue Resource saved successfully!");
		}
		else
		{
			GD.Print("Failed to save dialogue resource. Error: " + error);
		}
	}

	public void SaveFile(string filepath)
	{
		var tabIndex = Workspace.GetTabEditor(filepath);
		if (tabIndex >= 0)
		{
			var editor = Workspace.GetTabEditor(tabIndex);
			var error = ResourceSaver.Save(editor.DumpsData(), filepath);
			
			if (error == Error.Ok)
			{
				GD.Print("Dialogue Resource saved successfully!");
			}
			else
			{
				GD.Print("Failed to save dialogue resource. Error: "+ error);
			}
		}
	}
}
