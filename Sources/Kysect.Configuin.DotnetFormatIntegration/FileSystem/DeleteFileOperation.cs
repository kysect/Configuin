using Kysect.CommonLib.BaseTypes.Extensions;
using Microsoft.Extensions.Logging;

namespace Kysect.Configuin.DotnetFormatIntegration.FileSystem;

public class DeleteFileOperation : IFileMoveUndoOperation
{
    private readonly string _path;
    private readonly ILogger _logger;

    public DeleteFileOperation(string path, ILogger logger)
    {
        _path = path.ThrowIfNull();
        _logger = logger.ThrowIfNull();
    }

    public void Undo()
    {
        _logger.LogInformation("Undo file move. Delete {path}", _path);
        File.Delete(_path);
    }
}