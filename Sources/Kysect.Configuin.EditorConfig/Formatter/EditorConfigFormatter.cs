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
        IReadOnlyCollection<EditorConfigPropertyNode> styleRuleNodesForMoving = SelectIdeNodes(value, RoslynRuleTypes.StyleRule).OrderBy(r => r.Key.Value).ToList();
        IReadOnlyCollection<EditorConfigPropertyNode> qualityRuleNodesForMoving = SelectIdeNodes(value, RoslynRuleTypes.QualityRule).OrderBy(r => r.Key.Value).ToList();
        nodesForRemoving.AddRange(styleRuleNodesForMoving);
        nodesForRemoving.AddRange(qualityRuleNodesForMoving);

        if (nodesForRemoving.IsEmpty())
            return value;

        EditorConfigCategoryNode autoGeneratedSection = CreateAutoGeneratedCategory(styleRuleNodesForMoving, [new RoslynQualityRuleFormattedSection("CA", qualityRuleNodesForMoving)]);

        return value
            .RemoveNodes(nodesForRemoving)
            .AddChild(autoGeneratedSection);
    }

    public EditorConfigDocument FormatAccordingToRuleDefinitions(EditorConfigDocument document, RoslynRules rules, bool groupQualityRulesByCategory = false)
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

            if (roslynStyleRuleGroup.Rules.Any(r => r.RuleId.Equals(RoslynNameRuleInfo.RuleId)))
            {
                foreach (EditorConfigPropertyNode editorConfigPropertyNode in propertyNodes)
                {
                    IEditorConfigSetting editorConfigSetting = _settingsParser.ParseSetting(editorConfigPropertyNode);
                    if (editorConfigSetting is not CompositeRoslynOptionEditorConfigSetting option)
                        continue;

                    if (RoslynNameRuleInfo.IsNameRuleOption(option.ToDisplayString()))
                        selectedStyleRuleNodes.Add(editorConfigPropertyNode);
                }
            }
        }

        List<RoslynQualityRuleFormattedSection> formattedSections = new List<RoslynQualityRuleFormattedSection>();
        if (groupQualityRulesByCategory)
        {
            foreach (IGrouping<string, RoslynQualityRule> categoryRules in rules.QualityRules.GroupBy(r => r.Category).OrderBy(c => c.Key))
            {
                List<EditorConfigPropertyNode> selectedQualityRuleNodes = new List<EditorConfigPropertyNode>();
                foreach (RoslynQualityRule qualityRule in categoryRules.OrderBy(r => r.RuleId))
                {
                    EditorConfigPropertyNode? editorConfigPropertyNode = TryFindSeverityNode(propertyNodes, qualityRule.RuleId);
                    if (editorConfigPropertyNode is null)
                        continue;

                    selectedQualityRuleNodes.Add(editorConfigPropertyNode);
                }

                if (selectedQualityRuleNodes.Any())
                    formattedSections.Add(new RoslynQualityRuleFormattedSection($"CA {categoryRules.Key}", selectedQualityRuleNodes));
            }
        }
        else
        {
            List<EditorConfigPropertyNode> selectedQualityRuleNodes = new List<EditorConfigPropertyNode>();
            foreach (RoslynQualityRule qualityRule in rules.QualityRules)
            {
                EditorConfigPropertyNode? editorConfigPropertyNode = TryFindSeverityNode(propertyNodes, qualityRule.RuleId);
                if (editorConfigPropertyNode is null)
                    continue;

                selectedQualityRuleNodes.Add(editorConfigPropertyNode);
            }
            if (selectedQualityRuleNodes.Any())
                formattedSections.Add(new RoslynQualityRuleFormattedSection("CA", selectedQualityRuleNodes));
        }

        nodesForRemoving.AddRange(selectedStyleRuleNodes);
        nodesForRemoving.AddRange(formattedSections.SelectMany(s => s.SelectedQualityRuleNodes));
        EditorConfigCategoryNode autoGeneratedSection = CreateAutoGeneratedCategory(selectedStyleRuleNodes, formattedSections);

        return document
            .RemoveNodes(nodesForRemoving)
            .AddChild(autoGeneratedSection);
    }

    public record RoslynQualityRuleFormattedSection(string Title, IReadOnlyCollection<EditorConfigPropertyNode> SelectedQualityRuleNodes);

    private EditorConfigCategoryNode CreateAutoGeneratedCategory(
        IReadOnlyCollection<EditorConfigPropertyNode> styleRuleNodesForMoving,
        IReadOnlyCollection<RoslynQualityRuleFormattedSection> qualityRuleNodesForMoving)
    {
        var autoGeneratedSection = new EditorConfigCategoryNode("*.cs", [], ["# Autogenerated values"], null);

        if (styleRuleNodesForMoving.Any())
        {
            var styleRuleSection = new EditorConfigDocumentSectionNode("### IDE ###");

            foreach (EditorConfigPropertyNode styleRule in styleRuleNodesForMoving)
                styleRuleSection = styleRuleSection.AddChild(styleRule);

            autoGeneratedSection = autoGeneratedSection.AddChild(styleRuleSection);
        }

        foreach (RoslynQualityRuleFormattedSection roslynQualityRuleFormattedSection in qualityRuleNodesForMoving)
        {
            if (roslynQualityRuleFormattedSection.SelectedQualityRuleNodes.IsEmpty())
                continue;

            var qualitySection = new EditorConfigDocumentSectionNode($"### {roslynQualityRuleFormattedSection.Title} ###");
            foreach (EditorConfigPropertyNode qualityRule in roslynQualityRuleFormattedSection.SelectedQualityRuleNodes)
                qualitySection = qualitySection.AddChild(qualityRule);
            autoGeneratedSection = autoGeneratedSection.AddChild(qualitySection);
        }

        return autoGeneratedSection;
    }

    private IReadOnlyCollection<EditorConfigPropertyNode> SelectIdeNodes(EditorConfigDocument document, string ruleType)
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

            if (severityConfigSetting.RuleId.RuleType == ruleType)
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