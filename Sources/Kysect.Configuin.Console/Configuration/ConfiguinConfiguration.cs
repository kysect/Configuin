using System.ComponentModel.DataAnnotations;

namespace Kysect.Configuin.Console.Configuration;

public class ConfiguinConfiguration
{
    [Required]
    public string MsLearnRepositoryPath { get; init; } = null!;
}