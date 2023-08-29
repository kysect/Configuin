using Kysect.Configuin.Core.RoslynRuleModels;

namespace Kysect.Configuin.Core.CodeStyleGeneration.Models;

public class RoslynOptionConfiguration
{
    public RoslynStyleRuleOption Option { get; }
    public string SelectedValue { get; }

    public RoslynOptionConfiguration(RoslynStyleRuleOption option, string selectedValue)
    {
        Option = option;
        SelectedValue = selectedValue;
    }
}