using Kysect.CommonLib.FileSystem.Extensions;
using Microsoft.Extensions.Logging;

namespace Kysect.Configuin.Core.FileSystem;

public class TemporaryFileMover
{
    private readonly string _tempDirectory;
    private readonly ILogger _logger;

    public TemporaryFileMover(string tempDirectory, ILogger logger)
    {
        _tempDirectory = tempDirectory;
        _logger = logger;
    }

    public IFileMoveUndoOperation MoveFile(string sourcePath, string targetPath)
    {
        _logger.LogInformation("Move {sourcePath} to {targetPath}", sourcePath, targetPath);
        DirectoryExtensions.EnsureFileExists(_tempDirectory);

        if (!File.Exists(sourcePath))
            throw new FileNotFoundException(sourcePath);

        if (File.Exists(targetPath))
        {
            var targetFileInfo = new FileInfo(targetPath);
            string tempFilePath = Path.Combine(_tempDirectory, targetFileInfo.Name);
            _logger.LogInformation("Target path already exists. Save target file to temp path {tempPath}", tempFilePath);
            _logger.LogInformation("Move {targetPath} to {tempFilePath}", targetPath, tempFilePath);
            File.Move(targetPath, tempFilePath);
            _logger.LogInformation("Copy {sourcePath} to {targetPath}", sourcePath, targetPath);
            File.Copy(sourcePath, targetPath, overwrite: true);
            return new MoveBackFileOperation(tempFilePath, targetPath, _logger);
        }

        _logger.LogInformation("Target path is not exists. Copy {source} to {target}", sourcePath, targetPath);
        File.Copy(sourcePath, targetPath);
        return new DeleteFileOperation(targetPath, _logger);
    }
}