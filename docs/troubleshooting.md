# Troubleshooting

## Common issues

### Rule not applied

**Symptoms**: Expected correction doesn't happen

**Causes & fixes**:

| Cause | Fix |
|-------|-----|
| Rule not in rule set | Add rule to `IGrammarRuleSet.Rules` |
| `CanApply` returns `false` | Check rule logic; ensure input matches condition |
| Rule order | Earlier rule transforms text so later rule doesn't match; reorder rules |
| Rule is internal | `SpacingRule` is internal; use public API or copy logic |

### ArgumentNullException

**Symptoms**: `ArgumentNullException` thrown

**Locations**:

| Method | Parameter | Fix |
|--------|-----------|-----|
| `GrammarCorrector..ctor` | `ruleSet` | Pass non-null rule set |
| `GrammarCorrector.Correct` | `text` | Pass non-null string |
| `GrammarRule.CanApply` | `text` | Pass non-null string |
| `GrammarRule.Apply` | `text` | Pass non-null string |

**Fix**: Validate input before calling:

```csharp
if (string.IsNullOrEmpty(input)) return input;
return corrector.Correct(input);
```

### GrammarRuleException

**Symptoms**: `GrammarRuleException` thrown during correction

**Properties**:
- `RuleId` — identifies which rule failed
- `Message` — error description
- `InnerException` — original exception

**Fix**:
1. Check `RuleId` to identify failing rule
2. Review rule's `DoApply` implementation
3. Handle specific exceptions in custom rules

```csharp
try
{
    return corrector.Correct(text);
}
catch (GrammarRuleException ex)
{
    logger.LogError(ex, "Rule {RuleId} failed", ex.RuleId);
    return text; // or rethrow
}
```

### SpacingRule not accessible

**Symptoms**: Can't reference `SpacingRule` from consumer code

**Cause**: `SpacingRule` is `internal sealed`

**Fixes**:
1. Use `TrimWhitespaceRule` (public) for basic whitespace
2. Copy `SpacingRule` logic to a public custom rule
3. Request public API in future version

### Performance issues

**Symptoms**: Slow correction for large texts

**Investigation**:
- Profile with `dotnet trace` or Visual Studio profiler
- Check for ReDoS in custom regex rules
- Measure per-rule timing

**Optimizations**:
- Reduce rule set size
- Simplify regex patterns
- Avoid unnecessary allocations in custom rules
- Consider chunking large texts (consumer responsibility)

### Build failures

**Symptoms**: `dotnet build` fails

**Common causes**:

| Error | Fix |
|-------|-----|
| `NETSDK1045` (target framework) | Install .NET 10 SDK |
| `NU1101` (package not found) | Run `dotnet restore` |
| `CS0246` (type not found) | Check namespace/using directives |

### Test failures

**Symptoms**: `dotnet test` fails

**Debugging**:
```bash
# Run specific failing test
dotnet test --filter "FullyQualifiedName~TestName" --logger "console;verbosity=detailed"

# Check test output
dotnet test --logger "trx;LogFileName=test_results.trx"
```

**Common test issues**:
- Null reference in test setup
- Incorrect expected values
- Rule ordering assumptions
- Culture-dependent behavior (use `CultureInvariant`)

### Package issues

**NuGet package not found**:
- Check package name: `NuciText.Grammar`
- Check version exists on NuGet.org
- Verify feed configuration

**Version conflicts**:
- Check `PackageReference` versions
- Use `dotnet list package --vulnerable --deprecated`

## Debugging tips

### Enable detailed logging (consumer-side)

```csharp
public class DebugCorrector : IGrammarCorrector
{
    readonly IGrammarCorrector inner;

    public DebugCorrector(IGrammarCorrector inner) => this.inner = inner;

    public string Correct(string text)
    {
        Console.WriteLine($"[DEBUG] Input: '{text}'");
        string result = inner.Correct(text);
        Console.WriteLine($"[DEBUG] Output: '{result}'");
        return result;
    }
}
```

### Inspect rule set

```csharp
foreach (var rule in ruleSet.Rules)
{
    Console.WriteLine($"Rule: {rule.Id} - {rule.Description}");
    Console.WriteLine($"  CanApply: {rule.CanApply(text)}");
}
```

### Test individual rules

```csharp
foreach (var rule in ruleSet.Rules)
{
    if (rule.CanApply(text))
    {
        string before = text;
        text = rule.Apply(text);
        Console.WriteLine($"  {rule.Id}: '{before}' → '{text}'");
    }
}
```

## Getting help

1. Check [FAQ](faq.md)
2. Search [GitHub Issues](https://github.com/nucitext/nucitext.grammar/issues)
3. Open new issue with:
   - Minimal reproduction
   - Expected vs actual behavior
   - .NET version
   - Package version
   - Stack trace (if exception)

## Known limitations

| Limitation | Workaround |
|------------|------------|
| No streaming | Process in chunks (consumer) |
| No language detection | Consumer selects rule set |
| SpacingRule internal | Copy to public custom rule |
| No configuration | Pass via rule constructors |
| Single-threaded per call | Parallelize at consumer level |