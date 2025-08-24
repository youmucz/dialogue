@ -1,50 +1,283 @@
# godot-dialogue
This is the dialogue editor which is one part of the game story toolsets.

# version 1.0.0
- 剧情工具管线-对话编辑器，用来编辑游戏内的普通交互式对话、触发式对话或弹幕等类型的文本
- 初版支持创建、编辑和存储对话节点图文件，采用MVVM架构设计

# version 1.1.0
- 新增对话节点，对话结束节点
- 支持节点菜单

# version 1.2.0
- 代码架构重写,规范命名空间
- 使用工厂模式初始化meta,以便支持runtime和etitime

# version 1.2.1
- 对话节点面板显示参数，并且支持修改参数后同步到检查器中

# version 1.2.2
- add: 节点package scene换成export 读取形式
- add: 新增option选择节点,支持添加删除选项
- fix: dialog node meta初始化null报错
- mod: 节点菜单分类改名
- add: 新增标准图标资源库
- add: 新增toolbar,加入预览播放功能模块(占位,不起作用)

# version 1.2.3
- add: 新增运行时对话播放器，meta里加入节点进入/退出接口,
- mod: 去掉旧代码
- mod: 迁移代码,nodeMeta放到runtime,resouce放到core
- mod: 去掉node meta改成node和node base

# version 1.2.4
- add: 新增条件节点和变量节点
- mod: 修改scene节点命名格式,统一加上graph node
- add: 新增变量窗口,支持添加、删除和修改变量并将其存储到对应的graph文件中
- add: 新增变量链接图标
- mod: 变量窗口迭代，支持全局和局部变量,并用tree控件存储变量;删除旧变量窗口和功能模块
- add: 支持tree右键添加/删除局部变量,并存储到dialog中
- mod: 修改部分文件的目录结构,更合理的管理资源文件
- mod: 整理代码文件命名空间
- add: 局部变量窗口支持修改已有变量，新增图标
- add: 局部变量窗口新增setting按钮,支持双击item直接修改参数
- add: 条件节点支持选择并应用当前对话节点存储的局部变量（通过在窗口中双击）
- add: 条件节点新增变量类型按钮和功能
- add: 支持空白处弹出变量右键功能菜单
- add: 全局变量窗口支持编辑添加删除变量
- add: 搜下状态下获取到的变量数据支持存储到dialog本地资源文件
- add: 变量节点支持复数变量,节点面板支持添加、删除、修改和保持变量参数
- mod: 修改VariableEdit为ConditionItem

# version 1.2.5
- mod: 重构目录结构
- dialogue/
  ├── DialoguePlugin.cs
  ├── plugin.cfg
  ├── Scripts/
  │   ├── Core/                           # 核心系统
  │   │   ├── Data/                       # 数据模型
  │   │   │   ├── DialogueGraph.cs
  │   │   │   ├── DialogueNode.cs
  │   │   │   ├── DialogueConnection.cs
  │   │   │   ├── DialogueContext.cs
  │   │   │   └── Enums/
  │   │   │       ├── DialogueNodeType.cs
  │   │   │       └── PropertyType.cs
  │   │   │
  │   │   ├── Interfaces/                 # 接口定义
  │   │   │   ├── IDialogueNode.cs
  │   │   │   ├── ICommand.cs
  │   │   │   ├── ISerializer.cs
  │   │   │   └── IDialoguePlugin.cs
  │   │   │
  │   │   ├── Events/                     # 事件系统
  │   │   │   ├── DialogueEvents.cs
  │   │   │   └── EventArgs/
  │   │   │       ├── NodeEventArgs.cs
  │   │   │       └── ConnectionEventArgs.cs
  │   │   │
  │   │   └── Utils/                      # 工具类
  │   │       ├── GuidGenerator.cs
  │   │       ├── ColorPalette.cs
  │   │       └── MathUtils.cs
  │   │
  │   ├── Editor/                         # 编辑器相关
  │   │   ├── GraphEditor/                # 图形编辑器
  │   │   │   ├── DialogueGraphEditor.cs
  │   │   │   ├── GraphEditExtended.cs
  │   │   │   ├── ConnectionManager.cs
  │   │   │   └── ViewportController.cs
  │   │   │
  │   │   ├── Nodes/                      # 节点实现
  │   │   │   ├── Base/
  │   │   │   │   ├── BaseDialogueNode.cs
  │   │   │   │   ├── NodePort.cs
  │   │   │   │   └── NodeProperty.cs
  │   │   │   │
  │   │   │   ├── Dialogue/               # 对话节点
  │   │   │   │   ├── TextDialogueNode.cs
  │   │   │   │   ├── ChoiceDialogueNode.cs
  │   │   │   │   └── NarratorNode.cs
  │   │   │   │
  │   │   │   ├── Logic/                  # 逻辑节点
  │   │   │   │   ├── ConditionNode.cs
  │   │   │   │   ├── VariableNode.cs
  │   │   │   │   └── RandomNode.cs
  │   │   │   │
  │   │   │   ├── Flow/                   # 流程控制
  │   │   │   │   ├── StartNode.cs
  │   │   │   │   ├── EndNode.cs
  │   │   │   │   └── JumpNode.cs
  │   │   │   │
  │   │   │   └── Event/                  # 事件节点
  │   │   │       ├── EventTriggerNode.cs
  │   │   │       ├── QuestNode.cs
  │   │   │       └── ScriptNode.cs
  │   │   │
  │   │   ├── UI/                         # 编辑器UI
  │   │   │   ├── MainWindow/
  │   │   │   │   ├── MainEditorWindow.cs
  │   │   │   │   ├── MenuBar.cs
  │   │   │   │   ├── ToolBar.cs
  │   │   │   │   └── StatusBar.cs
  │   │   │   │
  │   │   │   ├── Panels/
  │   │   │   │   ├── NodeLibraryPanel.cs
  │   │   │   │   ├── NodeInspectorPanel.cs
  │   │   │   │   ├── GraphOutlinePanel.cs
  │   │   │   │   └── VariablePanel.cs
  │   │   │   │
  │   │   │   ├── Dialogs/
  │   │   │   │   ├── NewProjectDialog.cs
  │   │   │   │   ├── SaveAsDialog.cs
  │   │   │   │   ├── SettingsDialog.cs
  │   │   │   │   └── AboutDialog.cs
  │   │   │   │
  │   │   │   └── Controls/               # 自定义控件
  │   │   │       ├── PropertyEditors/
  │   │   │       │   ├── StringPropertyEditor.cs
  │   │   │       │   ├── IntPropertyEditor.cs
  │   │   │       │   ├── BoolPropertyEditor.cs
  │   │   │       │   └── ColorPropertyEditor.cs
  │   │   │       │
  │   │   │       ├── NodeCreationMenu.cs
  │   │   │       ├── MiniMap.cs
  │   │   │       └── SearchBox.cs
  │   │   │
  │   │   ├── Commands/                   # 命令系统
  │   │   │   ├── CommandManager.cs
  │   │   │   ├── NodeCommands/
  │   │   │   │   ├── CreateNodeCommand.cs
  │   │   │   │   ├── DeleteNodeCommand.cs
  │   │   │   │   ├── MoveNodeCommand.cs
  │   │   │   │   └── EditNodeCommand.cs
  │   │   │   │
  │   │   │   └── ConnectionCommands/
  │   │   │       ├── CreateConnectionCommand.cs
  │   │   │       └── DeleteConnectionCommand.cs
  │   │   │
  │   │   └── Tools/                      # 编辑器工具
  │   │       ├── NodeFactory.cs
  │   │       ├── NodeValidator.cs
  │   │       ├── GraphAnalyzer.cs
  │   │       └── AutoLayout.cs
  │   │
  │   ├── Runtime/                        # 运行时系统
  │   │   ├── DialogueRunner.cs
  │   │   ├── DialogueManager.cs
  │   │   ├── VariableManager.cs
  │   │   ├── SaveSystem/
  │   │   │   ├── DialogueSaveData.cs
  │   │   │   └── SaveManager.cs
  │   │   │
  │   │   └── UI/                         # 运行时UI
  │   │       ├── DialogueUI.cs
  │   │       ├── ChoiceButton.cs
  │   │       └── DialogueBox.cs
  │   │
  │   ├── Serialization/                  # 序列化系统
  │   │   ├── DialogueSerializer.cs
  │   │   ├── JsonSerializer.cs
  │   │   ├── BinarySerializer.cs
  │   │   └── Converters/
  │   │       ├── Vector2Converter.cs
  │   │       └── ColorConverter.cs
  │   │
  │   ├── Plugins/                        # 插件系统
  │   │   ├── PluginManager.cs
  │   │   ├── BasePlugin.cs
  │   │   └── Examples/
  │   │       ├── LocalizationPlugin/
  │   │       │   ├── LocalizationPlugin.cs
  │   │       │   └── LocalizationNode.cs
  │   │       │
  │   │       └── AudioPlugin/
  │   │           ├── AudioPlugin.cs
  │   │           └── AudioNode.cs
  │   │
  │   └── Tests/                          # 单元测试
  │       ├── Core/
  │       │   ├── DataTests.cs
  │       │   └── SerializationTests.cs
  │       │
  │       ├── Editor/
  │       │   ├── NodeTests.cs
  │       │   └── CommandTests.cs
  │       │
  │       └── Runtime/
  │           └── DialogueRunnerTests.cs
  │
  ├── Scenes/                             # 场景文件
  │   ├── Editor/
  │   │   ├── MainEditor.tscn
  │   │   ├── NodeTemplates/
  │   │   │   ├── TextNodeTemplate.tscn
  │   │   │   ├── ChoiceNodeTemplate.tscn
  │   │   │   └── ConditionNodeTemplate.tscn
  │   │   │
  │   │   └── UI/
  │   │       ├── NodeInspector.tscn
  │   │       ├── NodeLibrary.tscn
  │   │       └── ToolbarPanel.tscn
  │   │
  │   └── Runtime/
  │       ├── DialogueSystem.tscn
  │       └── DialogueUI.tscn
  │
  ├── Resources/                          # 资源文件
  │   ├── Themes/
  │   │   ├── EditorTheme.tres
  │   │   └── NodeThemes/
  │   │       ├── TextNodeTheme.tres
  │   │       ├── ChoiceNodeTheme.tres
  │   │       └── ConditionNodeTheme.tres
  │   │
  │   ├── Icons/
  │   │   ├── NodeIcons/
  │   │   │   ├── text_node.svg
  │   │   │   ├── choice_node.svg
  │   │   │   └── condition_node.svg
  │   │   │
  │   │   └── UI/
  │   │       ├── toolbar_icons/
  │   │       └── menu_icons/
  │   │
  │   ├── Fonts/
  │   │   ├── EditorFont.ttf
  │   │   └── MonospaceFont.ttf
  │   │
  │
  ├── Data/                               # 数据文件
  │   ├── Examples/
  │   │   ├── tutorial_dialogue.json
  │   │   └── complex_dialogue.json
  │   │
  │   └── Templates/
  │       └── node_templates.json
  │
  ├── Documentation/                      # 文档
  │   ├── API/
  │   │   ├── Core.md
  │   │   ├── Editor.md
  │   │   └── Runtime.md
  │   │
  │   ├── Tutorials/
  │   │   ├── GettingStarted.md
  │   │   ├── CreatingNodes.md
  │   │   └── PluginDevelopment.md
  └── Export/                             # 导出配置
  ├── Templates/
  └── Presets/