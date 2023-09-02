# Configuin

Configuin - это утилита для работы с .editorconfig'ом.

## Возможности

- Генерация документации к editorconfig (по документации из MS learn)
- [in progress] Сравнение двух editorconfig
- [in progress] Анализатор .editorconfig'а
- [in progress] Форматирование editorconfig'а
- [in progress] Предпросмотр изменений от editorconfig'а

### Генерация документации к editorconfig

Configuin парсит описание Roslyn, которые есть на сайте MS Learn, парсит .editorconfig файл предоставленный пользователем и генерирует документ с подробным описанием строк. Например, .editorconfig файл может содержать:
```ini
dotnet_diagnostic.IDE0040.severity = warning
dotnet_style_require_accessibility_modifiers = always:warning
```

При генерации кодстайла, для этих строк будет находиться описание и генерироваться такой output:

```md
## Add accessibility modifiers (IDE0040)

Severity: Warning

This style rule concerns requiring accessibility modifiers in declarations.

### dotnet_style_require_accessibility_modifiers = always

\```csharp
// dotnet_style_require_accessibility_modifiers = always
// dotnet_style_require_accessibility_modifiers = for_non_interface_members
class MyClass
{
    private const string thisFieldIsConst = "constant";
}

// dotnet_style_require_accessibility_modifiers = never
class MyClass
{
    const string thisFieldIsConst = "constant";
}
\```
```

### [in progress] Сравнение двух editorconfig

Configuin сравнивает два .editorconfig файла и генерирует diff между ними.

### [in progress] Анализатор .editorconfig'а

Configuin анализирует editorconfig файл и:
- Находит несуществующие правила
- Находит не корректные значения
- Генерирует список существующих правил, которые не указаны в .editorconfig

### [in progress] Форматирование editorconfig'а

### [in progress] Предпросмотр изменений от editorconfig'а

Configuin генерирует список изменений, который будут получены, если к проекту применить .editorconfig.

Алгоритм:
- Запускается `dotnet format` для проекта и сохраняется список сообщений
- Подменяется .editorconfig на то, который указал пользователь
- Запускается `dotnet format` и сохраняется второй результат
- Генерируется diff между результатами
- Откатывается изменение .editorconfig'а 
