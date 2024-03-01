using Kysect.CommonLib.BaseTypes.Extensions;
using Kysect.CommonLib.FileSystem;
using Microsoft.Extensions.Logging;

namespace Kysect.Configuin.DotnetFormatIntegration.FileSystem;

public class TemporaryFileMover
{
    private readonly ILogger _logger;

    public TemporaryFileMover(ILogger logger)
    {
        _logger = logger;
    }

    public IFileMoveUndoOperation MoveFile(string sourcePath, string targetPath)
    {
        _logger.LogInformation("Move {sourcePath} to {targetPath}", sourcePath, targetPath);

        if (!File.Exists(sourcePath))
            throw new FileNotFoundException(sourcePath);

        if (File.Exists(targetPath))
        {
            var targetFileInfo = new FileInfo(targetPath);
            DirectoryInfo targetFileDirectory = targetFileInfo.Directory.ThrowIfNull();
            string tempFileDirectory = Path.Combine(targetFileDirectory.FullName, ".congifuing");
            DirectoryExtensions.EnsureDirectoryExists(new System.IO.Abstractions.FileSystem(), tempFileDirectory);

            string tempFilePath = Path.Combine(tempFileDirectory, targetFileInfo.Name);
            _logger.LogInformation("Target path already exists. Save target file to temp path {tempPath}", tempFilePath);
            _logger.LogInformation("Move {targetPath} to {tempFilePath}", targetPath, tempFilePath);
            File.Move(targetPath, tempFilePath, overwrite: true);
            _logger.LogInformation("Copy {sourcePath} to {targetPath}", sourcePath, targetPath);
            File.Copy(sourcePath, targetPath, overwrite: true);
            return new MoveBackFileOperation(tempFilePath, targetPath, _logger);
        }

        _logger.LogInformation("Target path is not exists. Copy {source} to {target}", sourcePath, targetPath);
        File.Copy(sourcePath, targetPath);
        return new DeleteFileOperation(targetPath, _logger);
    }
}