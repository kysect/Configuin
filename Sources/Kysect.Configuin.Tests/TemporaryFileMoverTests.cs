using Kysect.CommonLib.FileSystem;
using Kysect.Configuin.DotnetFormatIntegration.FileSystem;
using Kysect.Configuin.Tests.Tools;

namespace Kysect.Configuin.Tests;

public class TemporaryFileMoverTests : IDisposable
{
    private const string TestGenerated = "TemporaryFileMoverTests";

    private readonly TemporaryFileMover _sut;

    public TemporaryFileMoverTests()
    {
        _sut = new TemporaryFileMover(TestLogger.ProviderForTests());
        DirectoryExtensions.EnsureDirectoryExists(new System.IO.Abstractions.FileSystem(), TestGenerated);

    }

    public void Dispose()
    {
        Directory.Delete(TestGenerated, recursive: true);

    }

    [Fact]
    public void MoveFile_WhenTargetNotExists_UndoDeleteFile()
    {
        string fileForMove = CreateFile("file_for_move");
        string targetPath = GeneratePath("target");

        IFileMoveUndoOperation undoAction = _sut.MoveFile(fileForMove, targetPath);
        File.Exists(targetPath).Should().BeTrue();

        undoAction.Undo();
        File.Exists(targetPath).Should().BeFalse();
    }

    [Fact]
    public void MoveFile_WhenTargetExists_UndoReturnOriginalFile()
    {
        string fileForMove = CreateFile("file_for_move");
        string targetFile = CreateFile("target");

        File.Exists(targetFile).Should().BeTrue();
        File.ReadAllText(targetFile).Should().Be(targetFile);

        IFileMoveUndoOperation undoAction = _sut.MoveFile(fileForMove, targetFile);

        File.Exists(targetFile).Should().BeTrue();
        File.ReadAllText(targetFile).Should().Be(fileForMove);

        undoAction.Undo();

        File.Exists(targetFile).Should().BeTrue();
        File.ReadAllText(targetFile).Should().Be(targetFile);
    }

    private static string CreateFile(string prefix)
    {
        string path = GeneratePath(prefix);

        File.WriteAllText(path: path, contents: path);

        return path;
    }

    private static string GeneratePath(string prefix)
    {
        return Path.Combine(TestGenerated, $"{prefix}_{Guid.NewGuid()}");
    }
}