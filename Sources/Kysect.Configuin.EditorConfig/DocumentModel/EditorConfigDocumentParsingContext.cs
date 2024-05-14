using Kysect.CommonLib.BaseTypes.Extensions;
using Kysect.Configuin.EditorConfig.DocumentModel.Nodes;
using System.Collections.Immutable;

namespace Kysect.Configuin.EditorConfig.DocumentModel;

public class EditorConfigDocumentParsingContext
{
    private EditorConfigDocument _currentDocument;
    private EditorConfigCategoryNode? _currentCategory;
    private EditorConfigDocumentSectionNode? _currentSection;
    private List<string> _currentTrivia;
    public EditorConfigDocumentParsingContext()
    {
        _currentDocument = new EditorConfigDocument(ImmutableList<IEditorConfigNode>.Empty);
        _currentCategory = null;
        _currentSection = null;

        _currentTrivia = new List<string>();
    }

    public void AddCategory(string categoryName)
    {
        DumpSection();
        DumpCategory();
        _currentCategory = new EditorConfigCategoryNode(categoryName) { LeadingTrivia = _currentTrivia.ToImmutableList() };
        _currentTrivia = new List<string>();
    }

    public void AddSection(string sectionName)
    {
        DumpSection();
        _currentSection = new EditorConfigDocumentSectionNode(sectionName) { LeadingTrivia = _currentTrivia.ToImmutableList() };
        _currentTrivia = new List<string>();
    }

    public void AddProperty(EditorConfigPropertyNode propertyNode)
    {
        propertyNode.ThrowIfNull();

        propertyNode = propertyNode with { LeadingTrivia = _currentTrivia.ToImmutableList() };
        _currentTrivia = new List<string>();

        if (_currentSection is not null)
        {
            _currentSection = _currentSection.AddChild(propertyNode);
            return;
        }

        if (_currentCategory is not null)
        {
            _currentCategory = _currentCategory.AddChild(propertyNode);
            return;
        }

        _currentDocument = _currentDocument.AddChild(propertyNode);
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

    public EditorConfigDocument Build()
    {
        DumpSection();
        DumpCategory();
        return _currentDocument;
    }

    public void AddTrivia(string line)
    {
        _currentTrivia.Add(line);
    }
}