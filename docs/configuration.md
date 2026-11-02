# Configuration

NuciText.Grammar has no built-in configuration system. It is a stateless library with no external dependencies, configuration files, or runtime configuration.

## Configuration points

### Compile-time configuration

| Setting | Location | Description |
|---------|----------|-------------|
| Target framework | `NuciText.Grammar.csproj` | `<TargetFramework>net10.0</TargetFramework>` |
| Nullable context | `NuciText.Grammar.csproj` | `<Nullable>enable</Nullable>` |
| Implicit usings | `NuciText.Grammar.csproj` | `<ImplicitUsings>disable</ImplicitUsings>` |
| Language version | `NuciText.Grammar.csproj` | `<LangVersion>latest</LangVersion>` |
| Packable | `NuciText.Grammar.csproj` | `<IsPackable>true</IsPackable>` |
| Documentation file | `NuciText.Grammar.csproj` | `<GenerateDocumentationFile>true</GenerateDocumentationFile>` |

### Runtime configuration

None. The library has:
- No configuration files (no `appsettings.json`, no `.config`)
- No environment variable dependencies
- No command-line arguments
- No DI configuration requirements
- No feature flags

### Consumer-controlled configuration

Consumers configure behavior by:
1. **Choosing rule set** — Implement `IGrammarRuleSet` with desired rules
2. **Ordering rules** — Define `Rules` property order
3. **Implementing custom rules** — Create `IGrammarRule` implementations
4. **Instantiating corrector** — `new GrammarCorrector(ruleSet)`

## No configuration precedence

Since there is no configuration system, there is no precedence to document.

## Extending with configuration

If consumers need configurable rules, they should:
1. Add configuration to their application
2. Pass configuration to custom rule/rule set constructors
3. Use configuration in `DoApply` or rule selection logic

Example:
```csharp
public sealed class ConfigurableRuleSet : GrammarRuleSet
{
    readonly MyOptions options;

    public ConfigurableRuleSet(IOptions<MyOptions> options)
    {
        this.options = options.Value;
    }

    public override string LanguageCode => "en";

    public override IReadOnlyList<IGrammarRule> Rules { get; } = new IGrammarRule[]
    {
        new TrimWhitespaceRule(),
        new CustomRule(options.SomeSetting)
    };
}
```