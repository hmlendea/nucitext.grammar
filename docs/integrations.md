# Integrations

## External system integrations

NuciText.Grammar has **no built-in integrations** with external systems. It is a standalone, in-process library with no network, file system, or external service dependencies.

## Integration patterns

### As a library dependency

Consumers reference the NuGet package and use the API directly:

```xml
<PackageReference Include="NuciText.Grammar" Version="1.0.0" />
```

```csharp
using NuciText.Grammar;
using NuciText.Grammar.Rules;

IGrammarRuleSet ruleSet = new MyRuleSet();
IGrammarCorrector corrector = new GrammarCorrector(ruleSet);
string corrected = corrector.Correct(inputText);
```

### In a processing pipeline

```csharp
public class TextProcessingPipeline
{
    readonly IGrammarCorrector corrector;

    public TextProcessingPipeline(IGrammarCorrector corrector)
    {
        this.corrector = corrector;
    }

    public string Process(string rawText)
    {
        // Other processing steps...
        string corrected = corrector.Correct(rawText);
        // More processing...
        return corrected;
    }
}
```

### With dependency injection

```csharp
// Registration
services.AddSingleton<IGrammarRuleSet, EnglishRuleSet>();
services.AddSingleton<IGrammarCorrector, GrammarCorrector>();

// Usage
public class MyService
{
    readonly IGrammarCorrector corrector;

    public MyService(IGrammarCorrector corrector)
    {
        this.corrector = corrector;
    }

    public string CleanText(string input) => corrector.Correct(input);
}
```

## No built-in integrations

| Integration type | Status | Notes |
|------------------|--------|-------|
| HTTP APIs | ❌ None | Library has no HTTP client |
| Databases | ❌ None | No persistence |
| Message queues | ❌ None | No messaging |
| Cloud services | ❌ None | No cloud SDKs |
| Logging frameworks | ❌ None | No logging |
| Metrics/tracing | ❌ None | No observability |
| Configuration systems | ❌ None | No config providers |

## Consumer integration responsibilities

Consumers integrating NuciText.Grammar are responsible for:

1. **Rule set selection** — Choose or implement appropriate `IGrammarRuleSet`
2. **Error handling** — Catch `ArgumentNullException` and `GrammarRuleException`
3. **Performance** — Profile for their text sizes and rule complexity
4. **Thread safety** — Library is thread-safe; consumers manage their own concurrency
5. **Licensing** — Comply with GPL-3.0-or-later

## Related projects

- **NuciText** — Parent project ecosystem (if applicable)
- **NuciAPI.Middleware** — Used in unit tests only (internal Nuci package)

## Future integration considerations

If integrations are added in the future, they should:
- Be opt-in (separate packages)
- Follow the same zero-dependency principle for core
- Maintain stateless, pure function design
- Document clearly in this file