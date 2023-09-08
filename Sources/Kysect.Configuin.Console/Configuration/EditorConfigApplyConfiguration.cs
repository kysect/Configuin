using System.ComponentModel.DataAnnotations;

namespace Kysect.Configuin.Console.Configuration;

public class EditorConfigApplyConfiguration
{
    [Required]
    public string SolutionPath { get; init; } = null!;
    [Required]
    public string SourceEditorConfig { get; init; } = null!;
    [Required]
    public string NewEditorConfig { get; init; } = null!;
}