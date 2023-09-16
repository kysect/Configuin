using Microsoft.Extensions.Logging;

namespace Kysect.Configuin.DotnetFormatIntegration.FileSystem;

public class MoveBackFileOperation : IFileMoveUndoOperation
{
    private readonly string _source;
    private readonly string _target;
    private readonly ILogger _logger;

    public MoveBackFileOperation(string source, string target, ILogger logger)
    {
        _source = source;
        _target = target;
        _logger = logger;
    }

    public void Undo()
    {
        _logger.LogInformation("Undo file move. Move backup file from {source} to {target}", _source, _target);
        File.Move(_source, _target, overwrite: true);
    }
}