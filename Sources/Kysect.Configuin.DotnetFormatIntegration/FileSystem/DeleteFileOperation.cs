using Microsoft.Extensions.Logging;

namespace Kysect.Configuin.DotnetFormatIntegration.FileSystem;

public class DeleteFileOperation : IFileMoveUndoOperation
{
    private readonly string _path;
    private readonly ILogger _logger;

    public DeleteFileOperation(string path, ILogger logger)
    {
        _path = path;
        _logger = logger;
    }

    public void Dispose()
    {
        _logger.LogInformation("Undo file move. Delete {path}", _path);
        File.Delete(_path);
    }
}