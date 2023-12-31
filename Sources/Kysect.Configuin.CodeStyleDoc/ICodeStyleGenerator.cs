﻿using Kysect.Configuin.CodeStyleDoc.Models;
using Kysect.Configuin.EditorConfig;
using Kysect.Configuin.RoslynModels;

namespace Kysect.Configuin.CodeStyleDoc;

public interface ICodeStyleGenerator
{
    CodeStyle Generate(EditorConfigSettings editorConfigSettings, RoslynRules roslynRules);
}