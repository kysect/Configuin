﻿using Kysect.CommonLib.BaseTypes.Extensions;
using Kysect.CommonLib.Collections.Extensions;
using Kysect.Configuin.EditorConfig.DocumentModel;
using Kysect.Configuin.EditorConfig.DocumentModel.Nodes;
using Kysect.Configuin.EditorConfig.Settings;
using Kysect.Configuin.RoslynModels;

namespace Kysect.Configuin.EditorConfig.Formatter;

public class EditorConfigFormatter
{
    private readonly IDotnetConfigSettingsParser _settingsParser;

    public EditorConfigFormatter(IDotnetConfigSettingsParser settingsParser)
    {
        _settingsParser = settingsParser;
    }

    public EditorConfigDocument Format(EditorConfigDocument value)
    {
        List<EditorConfigPropertyNode> nodesForRemoving = new List<EditorConfigPropertyNode>();
        IReadOnlyCollection<EditorConfigPropertyNode> styleRuleNodesForMoving = SelectIdeNodes(value, RoslynRuleType.StyleRule).OrderBy(r => r.Key.Value).ToList();
        IReadOnlyCollection<EditorConfigPropertyNode> qualityRuleNodesForMoving = SelectIdeNodes(value, RoslynRuleType.QualityRule).OrderBy(r => r.Key.Value).ToList();
        nodesForRemoving.AddRange(styleRuleNodesForMoving);
        nodesForRemoving.AddRange(qualityRuleNodesForMoving);

        if (nodesForRemoving.IsEmpty())
            return value;

        EditorConfigCategoryNode autoGeneratedSection = CreateAutoGeneratedCategory(styleRuleNodesForMoving, qualityRuleNodesForMoving);

        return value
            .RemoveNodes(nodesForRemoving)
            .AddChild(autoGeneratedSection);
    }

    public EditorConfigDocument FormatAccordingToRuleDefinitions(EditorConfigDocument document, RoslynRules rules)
    {
        rules.ThrowIfNull();

        List<EditorConfigPropertyNode> nodesForRemoving = new List<EditorConfigPropertyNode>();

        List<EditorConfigPropertyNode> propertyNodes = document
            .DescendantNodes()
            .OfType<EditorConfigPropertyNode>()
            .ToList();

        List<EditorConfigPropertyNode> selectedStyleRuleNodes = new List<EditorConfigPropertyNode>();
        foreach (RoslynStyleRuleGroup roslynStyleRuleGroup in rules.StyleRuleGroups)
        {
            foreach (RoslynStyleRule roslynStyleRule in roslynStyleRuleGroup.Rules)
            {
                EditorConfigPropertyNode? editorConfigPropertyNode = TryFindSeverityNode(propertyNodes, roslynStyleRule.RuleId);
                if (editorConfigPropertyNode is null)
                    continue;

                selectedStyleRuleNodes.Add(editorConfigPropertyNode);
            }

            foreach (RoslynStyleRuleOption roslynStyleRuleOption in roslynStyleRuleGroup.Options)
            {
                EditorConfigPropertyNode? editorConfigPropertyNode = TryFindOptionNode(propertyNodes, roslynStyleRuleOption);
                if (editorConfigPropertyNode is null)
                    continue;

                selectedStyleRuleNodes.Add(editorConfigPropertyNode);
            }
        }

        List<EditorConfigPropertyNode> selectedQualityRuleNodes = new List<EditorConfigPropertyNode>();
        foreach (RoslynQualityRule qualityRule in rules.QualityRules)
        {
            EditorConfigPropertyNode? editorConfigPropertyNode = TryFindSeverityNode(propertyNodes, qualityRule.RuleId);
            if (editorConfigPropertyNode is null)
                continue;

            selectedQualityRuleNodes.Add(editorConfigPropertyNode);
        }

        nodesForRemoving.AddRange(selectedStyleRuleNodes);
        nodesForRemoving.AddRange(selectedQualityRuleNodes);
        EditorConfigCategoryNode autoGeneratedSection = CreateAutoGeneratedCategory(selectedStyleRuleNodes, selectedQualityRuleNodes);

        return document
            .RemoveNodes(nodesForRemoving)
            .AddChild(autoGeneratedSection);
    }

    private EditorConfigCategoryNode CreateAutoGeneratedCategory(IReadOnlyCollection<EditorConfigPropertyNode> styleRuleNodesForMoving, IReadOnlyCollection<EditorConfigPropertyNode> qualityRuleNodesForMoving)
    {
        var autoGeneratedSection = new EditorConfigCategoryNode("*.cs", [], ["# Autogenerated values"], null);

        if (styleRuleNodesForMoving.Any())
        {
            var styleRuleSection = new EditorConfigDocumentSectionNode("### IDE ###");

            foreach (EditorConfigPropertyNode styleRule in styleRuleNodesForMoving)
                styleRuleSection = styleRuleSection.AddChild(styleRule);

            autoGeneratedSection = autoGeneratedSection.AddChild(styleRuleSection);
        }

        if (qualityRuleNodesForMoving.Any())
        {
            var qualitySection = new EditorConfigDocumentSectionNode("### CA ###");
            foreach (EditorConfigPropertyNode qualityRule in qualityRuleNodesForMoving)
                qualitySection = qualitySection.AddChild(qualityRule);

            autoGeneratedSection = autoGeneratedSection.AddChild(qualitySection);
        }

        return autoGeneratedSection;
    }

    private IReadOnlyCollection<EditorConfigPropertyNode> SelectIdeNodes(EditorConfigDocument document, RoslynRuleType roslynRuleType)
    {
        List<EditorConfigPropertyNode> propertyNodes = document
            .DescendantNodes()
            .OfType<EditorConfigPropertyNode>()
            .ToList();

        List<EditorConfigPropertyNode> styleRuleNodes = new List<EditorConfigPropertyNode>();
        foreach (EditorConfigPropertyNode editorConfigPropertyNode in propertyNodes)
        {
            IEditorConfigSetting editorConfigSetting = _settingsParser.ParseSetting(editorConfigPropertyNode);
            if (editorConfigSetting is not RoslynSeverityEditorConfigSetting severityConfigSetting)
                continue;

            if (severityConfigSetting.RuleId.Type == roslynRuleType)
                styleRuleNodes.Add(editorConfigPropertyNode);
        }

        return styleRuleNodes;
    }

    private EditorConfigPropertyNode? TryFindSeverityNode(IReadOnlyCollection<EditorConfigPropertyNode> propertyNodes, RoslynRuleId id)
    {
        foreach (EditorConfigPropertyNode editorConfigPropertyNode in propertyNodes)
        {
            IEditorConfigSetting editorConfigSetting = _settingsParser.ParseSetting(editorConfigPropertyNode);
            if (editorConfigSetting is not RoslynSeverityEditorConfigSetting severitySettings)
                continue;

            if (severitySettings.RuleId == id)
                return editorConfigPropertyNode;
        }

        return null;
    }

    private EditorConfigPropertyNode? TryFindOptionNode(IReadOnlyCollection<EditorConfigPropertyNode> propertyNodes, RoslynStyleRuleOption roslynStyleRuleOption)
    {
        foreach (EditorConfigPropertyNode editorConfigPropertyNode in propertyNodes)
        {
            IEditorConfigSetting editorConfigSetting = _settingsParser.ParseSetting(editorConfigPropertyNode);
            if (editorConfigSetting is not RoslynOptionEditorConfigSetting option)
                continue;

            if (option.Key == roslynStyleRuleOption.Name)
                return editorConfigPropertyNode;
        }

        return null;
    }
}