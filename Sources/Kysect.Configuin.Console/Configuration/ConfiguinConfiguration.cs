using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Kysect.Configuin.Console.Configuration;

[SuppressMessage("Naming", "CS8618:Non-nullable variable must contain a non-null value when exiting constructor", Justification = "AutoValidation")]
public class ConfiguinConfiguration
{
    [Required]
    public string MsLearnRepositoryPath { get; init; }
}