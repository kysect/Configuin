namespace Kysect.Configuin.EditorConfig;

public interface IEditorConfigContentProvider
{
    string Provide(string filePath);
}