using Kysect.Configuin.RoslynModels;

namespace Kysect.Configuin.Learn.Abstraction;

public interface IRoslynRuleDocumentationParser
{
    RoslynRules Parse(string learnRepositoryPath);
}