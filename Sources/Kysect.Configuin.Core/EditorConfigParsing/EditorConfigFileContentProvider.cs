namespace Kysect.Configuin.Core.EditorConfigParsing;

public class EditorConfigFileContentProvider : IEditorConfigContentProvider
{
    private readonly string _filePath;

    public EditorConfigFileContentProvider(string filePath)
    {
        _filePath = filePath;
    }

    public string Provide()
    {
        return File.ReadAllText(_filePath);
    }
}