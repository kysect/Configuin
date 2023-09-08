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

### Предпросмотр изменений от editorconfig'а

Configuin генерирует список изменений, который будут получены, если к проекту применить .editorconfig.

Алгоритм:
- Запускается `dotnet format` для проекта и сохраняется список сообщений
- Подменяется .editorconfig на то, который указал пользователь
- Запускается `dotnet format` и сохраняется второй результат
- Генерируется diff между результатами
- Откатывается изменение .editorconfig'а 

Пример использования:

1. Берётся проект с .editorconfig'ом
2. Копируется .editorconfig и модифицируется, например, включается CA1032
3. Запускается Configuin, указывается путь к солюшену и изменённому .editorconfig
4. Генерируется результат:

```log
[18:24:58 INF] Generate dotnet format warnings for C:\Coding\Kysect.CommonLib\Sources\Kysect.CommonLib.sln will save to output-8b107f73-6763-4c1f-b643-4e8eabac9d91.json
[18:24:58 INF] Generate warnings for C:\Coding\Kysect.CommonLib\Sources\Kysect.CommonLib.sln and write result to output-8b107f73-6763-4c1f-b643-4e8eabac9d91.json
[18:24:58 VRB] Execute cmd command cmd.exe /C dotnet format "C:\Coding\Kysect.CommonLib\Sources\Kysect.CommonLib.sln" --verify-no-changes --report "output-8b107f73-6763-4c1f-b643-4e8eabac9d91.json"
[18:25:08 INF] Remove temp file output-8b107f73-6763-4c1f-b643-4e8eabac9d91.json
[18:25:08 INF] Move C:\Users\fredi\Desktop\.editorconfig to C:\Coding\Kysect.CommonLib\Sources\.editorconfig
[18:25:08 INF] Target path already exists. Save target file to temp path C:\Coding\Kysect.CommonLib\Sources\.congifuing\.editorconfig
[18:25:08 INF] Move C:\Coding\Kysect.CommonLib\Sources\.editorconfig to C:\Coding\Kysect.CommonLib\Sources\.congifuing\.editorconfig
[18:25:08 INF] Copy C:\Users\fredi\Desktop\.editorconfig to C:\Coding\Kysect.CommonLib\Sources\.editorconfig
[18:25:08 INF] Generate dotnet format warnings for C:\Coding\Kysect.CommonLib\Sources\Kysect.CommonLib.sln will save to output-4d210962-92ec-44c8-b367-35840f6403d9.json
[18:25:08 INF] Generate warnings for C:\Coding\Kysect.CommonLib\Sources\Kysect.CommonLib.sln and write result to output-4d210962-92ec-44c8-b367-35840f6403d9.json
[18:25:08 VRB] Execute cmd command cmd.exe /C dotnet format "C:\Coding\Kysect.CommonLib\Sources\Kysect.CommonLib.sln" --verify-no-changes --report "output-4d210962-92ec-44c8-b367-35840f6403d9.json"
[18:25:16 ERR] Cmd execution finished with exit code 2.
[18:25:16 INF] Remove temp file output-4d210962-92ec-44c8-b367-35840f6403d9.json
[18:25:16 INF] Undo file move. Move backup file from C:\Coding\Kysect.CommonLib\Sources\.congifuing\.editorconfig to C:\Coding\Kysect.CommonLib\Sources\.editorconfig
[18:25:16 INF] Comparing dotnet format report
[18:25:16 INF] Same: 0, added: 2, removed: 0
[18:25:16 INF] New warnings count: 2
[18:25:16 INF]  C:\Coding\Kysect.CommonLib\Sources\Kysect.CommonLib\Reflection\ReflectionException.cs
[18:25:16 INF]          error CA1032: Add the following constructor to ReflectionException: public ReflectionException()
[18:25:16 INF]  C:\Coding\Kysect.CommonLib\Sources\Kysect.CommonLib\Reflection\ReflectionException.cs
[18:25:16 INF]          error CA1032: Add the following constructor to ReflectionException: public ReflectionException(string message)
```

### [in progress] Сравнение двух editorconfig

Configuin сравнивает два .editorconfig файла и генерирует diff между ними.

### [in progress] Анализатор .editorconfig'а

Configuin анализирует editorconfig файл и:
- Находит несуществующие правила
- Находит не корректные значения
- Генерирует список существующих правил, которые не указаны в .editorconfig

### [in progress] Форматирование editorconfig'а
