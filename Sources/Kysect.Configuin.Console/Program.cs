using Kysect.Configuin.ConfigurationRoot;

IServiceProvider s = DependencyBuilder.InitializeServiceProvider();
var configuinCommands = new ConfiguinCommands(s);

//configuinCommands.GenerateCodeStyle();
//configuinCommands.GetEditorConfigWarningUpdates();
configuinCommands.GetMissedConfiguration();