using Kysect.Configuin.Console;

IServiceProvider s = DependencyBuilder.InitializeServiceProvider();
var configuinCommands = new ConfiguinCommands(s);

configuinCommands.GenerateCodeStyle();