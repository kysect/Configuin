# Configuin

Configuin - это утилита для работы с .editorconfig'ом.

## Возможности

- Генерация шаблона .editorconfig'а
- Генерация документации к editorconfig'у
- Предпросмотр изменений от editorconfig'а
- [in progress] Анализатор .editorconfig'а
- [in progress] Сравнение двух editorconfig'ов
- [in progress] Форматирование editorconfig'а

### Генерация шаблона .editorconfig'а

Configuin парсит все описанные в документации правила и генерирует .editorconfig, где они все описаны, чтобы упростить процесс заполенения .editorconfig'а. Например, команда

```
Kysect.Configuin.Console.exe template ".editorconfig" -d "C:\Coding\dotnet-docs"
```

сгенерирует файл с таким содержанием:

```ini
## Simplify name (IDE0001)
## This rule concerns the use of simplified type names in declarations and executable code, when possible. You can remove unnecessary name qualification to simplify code and improve readability.
## using System.IO;
## class C
## {
##     // IDE0001: 'System.IO.FileInfo' can be simplified to 'FileInfo'
##     System.IO.FileInfo file;
## 
##     // Fixed code
##     FileInfo file;
## }
# dotnet_diagnostic.IDE0001.severity = 

# ...
```

Полный файл можно посмотреть [тут](Docs/.editorconfig).

### Генерация документации к .editorconfig

Configuin парсит описание Roslyn, которые есть на сайте MS Learn, парсит .editorconfig файл предоставленный пользователем и генерирует документ с подробным описанием строк. Пример вызова:

```
Kysect.Configuin.Console.exe generate-codestyle-doc "C:\Project\.editorconfig" -o "output.md" -d "C:\Coding\dotnet-docs"
```

Например, .editorconfig файл может содержать:
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

```
Kysect.Configuin.Console.exe preview -s "C:\Project\" -t "C:\Project\.editorconfig" -e "C:\.editorconfig"
```

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

### [in progress] Анализатор .editorconfig'а

```
Kysect.Configuin.Console.exe analyze "C:\Project\.editorconfig" -d "C:\Coding\dotnet-docs"
```

Configuin анализирует editorconfig файл и:
- Находит несуществующие правила
- Находит не корректные значения
- Генерирует список существующих правил, которые не указаны в .editorconfig

### [in progress] Сравнение двух editorconfig

Configuin сравнивает два .editorconfig файла и генерирует diff между ними.

### [in progress] Форматирование editorconfig'а
