using Kysect.CommonLib.BaseTypes.Extensions;
using Kysect.Configuin.DotnetConfig.Syntax.Nodes;
using System.Collections.Immutable;

namespace Kysect.Configuin.DotnetConfig.Syntax;

public class DotnetConfigDocumentParsingContext
{
    private DotnetConfigDocument _currentDocument;
    private DotnetConfigCategoryNode? _currentCategory;
    private DotnetConfigSectionNode? _currentSection;
    private List<string> _currentTrivia;

    public DotnetConfigDocumentParsingContext()
    {
        _currentDocument = new DotnetConfigDocument(ImmutableList<IDotnetConfigSyntaxNode>.Empty);
        _currentCategory = null;
        _currentSection = null;

        _currentTrivia = new List<string>();
    }

    public void AddCategory(DotnetConfigCategoryNode categoryNode)
    {
        categoryNode.ThrowIfNull();

        DumpSection();
        DumpCategory();
        _currentCategory = categoryNode with { LeadingTrivia = _currentTrivia.ToImmutableList() };
        _currentTrivia = new List<string>();
    }

    public void AddSection(string sectionName)
    {
        DumpSection();
        _currentSection = new DotnetConfigSectionNode(sectionName) { LeadingTrivia = _currentTrivia.ToImmutableList() };
        _currentTrivia = new List<string>();
    }

    public void AddProperty(IDotnetConfigPropertySyntaxNode syntaxPropertyNode)
    {
        syntaxPropertyNode.ThrowIfNull();

        syntaxPropertyNode = syntaxPropertyNode.WithLeadingTrivia(_currentTrivia.ToImmutableList());
        _currentTrivia = new List<string>();

        if (_currentSection is not null)
        {
            _currentSection = _currentSection.AddChild(syntaxPropertyNode);
            return;
        }

        if (_currentCategory is not null)
        {
            _currentCategory = _currentCategory.AddChild(syntaxPropertyNode);
            return;
        }

        _currentDocument = _currentDocument.AddChild(syntaxPropertyNode);
    }

    private void DumpSection()
    {
        if (_currentSection is null)
            return;

        if (_currentCategory is not null)
        {
            _currentCategory = _currentCategory.AddChild(_currentSection);
        }
        else
        {
            _currentDocument = _currentDocument.AddChild(_currentSection);
        }

        _currentSection = null;
    }

    private void DumpCategory()
    {
        if (_currentCategory is null)
            return;

        _currentDocument = _currentDocument.AddChild(_currentCategory);
        _currentCategory = null;
    }

    public DotnetConfigDocument Build()
    {
        DumpSection();
        DumpCategory();
        return _currentDocument with { TrailingTrivia = _currentTrivia.ToImmutableList() };
    }

    public void AddTrivia(string line)
    {
        _currentTrivia.Add(line);
    }
}