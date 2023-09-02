using Kysect.Configuin.Core.RoslynRuleModels;

namespace Kysect.Configuin.Core.CodeStyleGeneration.Models;

public record CodeStyleRoslynOptionConfiguration(RoslynStyleRuleOption Option, string SelectedValue);