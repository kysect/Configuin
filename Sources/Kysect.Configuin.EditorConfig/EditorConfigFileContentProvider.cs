namespace Kysect.Configuin.EditorConfig;

public class EditorConfigFileContentProvider : IEditorConfigContentProvider
{
    public string Provide(string filePath)
    {
        return File.ReadAllText(filePath);
    }
}