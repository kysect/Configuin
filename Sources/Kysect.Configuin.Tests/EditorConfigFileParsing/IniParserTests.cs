using FluentAssertions;
using Kysect.Configuin.Core.IniParsing;
using NUnit.Framework;

namespace Kysect.Configuin.Tests.EditorConfigFileParsing;

public class IniParserTests
{
    private readonly IniParser _parser = new IniParser();

    [Test]
    public void Parse_SimpleLine_ReturnParsedKeyValue()
    {
        string content = "key = value";

        IReadOnlyCollection<IniFileLine> result = _parser.Parse(content);

        result
            .Should().HaveCount(1)
            .And.Contain(new IniFileLine("key", "value"));
    }

    [Test]
    public void Parse_LineWithComment_ReturnParsedKeyValue()
    {
        string content = "# key = value";

        IReadOnlyCollection<IniFileLine> result = _parser.Parse(content);

        result.Should().HaveCount(0);
    }

    [Test]
    public void Parse_EditorConfigFile_ParsedWithoutErrors()
    {
        string fileText = File.ReadAllText(Path.Combine("EditorConfigFileParsing", "Resources", "Editor-config-sample.ini"));

        IReadOnlyCollection<IniFileLine> result = _parser.Parse(fileText);

        result.Should().HaveCount(400);
    }
}