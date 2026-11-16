# Testing

## Test strategy

NuciText.Grammar uses **NUnit 4.x** with **Moq** for unit testing. Tests focus on:

- Public API contracts
- Rule behavior (matching/non-matching)
- Rule ordering
- Exception handling
- Edge cases (null, empty, whitespace)

## Test project structure

```
NuciText.Grammar.UnitTests/
├── GrammarCorrectorTests.cs            # Corrector behaviour
├── GrammarRuleExceptionTests.cs        # Exception constructors and properties
├── GrammarRuleSetTests.cs              # Rule set contract
├── GrammarRuleTests.cs                 # Base rule behaviour
├── Dummies/                            # Shared custom test implementations
│   ├── DoubleSpaceRule.cs
│   ├── EmptyRuleSet.cs
│   ├── FixedApplicabilityRule.cs
│   ├── MultiRuleSet.cs
│   ├── RegexReplacementRule.cs
│   ├── SingleRuleSet.cs
│   ├── SingleWordReplacementRule.cs
│   ├── TrimRule.cs
│   ├── UppercaseFirstCharRule.cs
│   └── WordReplacementRule.cs
└── Rules/
    ├── PatternReplacementRuleBaseTests.cs   # Word and regex replacement helpers
    ├── SpacingRuleTests.cs                 # SpacingRule tests
    └── TrimWhitespaceRuleTests.cs          # TrimWhitespaceRule tests
```

Dummy implementations are top-level `internal` classes in separate files so they can be reused across fixtures without duplicating rule logic.

## Naming convention

Tests follow **Given-When-Then** pattern:

```csharp
[Test]
public void GivenNullRuleSet_WhenConstructing_ThenThrowsArgumentNullException()

[Test]
public void GivenMatchingRule_WhenCorrecting_ThenAppliesRule()

[Test]
public void GivenMultipleRules_WhenCorrecting_ThenAppliesInOrder()
```

## Running tests

```bash
# All tests
dotnet test

# With coverage
dotnet test --collect:"XPlat Code Coverage"

# Specific test class
dotnet test --filter "FullyQualifiedName~GrammarCorrectorTests"

# Verbose output
dotnet test --logger "console;verbosity=detailed"
```

## Test categories

### GrammarCorrectorTests

| Area | Purpose |
|------|---------|
| Constructor validation | Null `ruleSet` throws `ArgumentNullException` with `ruleSet` parameter name |
| Input validation | Null `text` throws `ArgumentNullException` with `text` parameter name |
| Empty rule set | No rules = identity; result is never null |
| Single rule | One matching rule is applied; non-matching rule is skipped |
| Rule ordering | Rules receive the previous rule's output; declared order is enforced with `MockSequence` |
| `CanApply` gating | `Apply` is never invoked when `CanApply` returns `false` |
| Applicability overrides | `CheckApplicability` overrides are honoured in both directions |
| Exception propagation | Exceptions from `CanApply` and `Apply` propagate unchanged; later rules are not invoked |
| Determinism | Repeated and re-applied corrections produce stable results |

### GrammarRuleTests

| Area | Purpose |
|------|---------|
| Metadata | `Id` and `Description` are exposed |
| Default applicability | `CanApply` returns `true` only when `DoApply` changes the text |
| Null validation | `CanApply` and `Apply` throw `ArgumentNullException` naming `text` |
| Delegation | `Apply` returns the `DoApply` result for empty, single-word, and whitespace input |
| Overridden applicability | `CheckApplicability` overrides bypass the default comparison |

### GrammarRuleSetTests

| Area | Purpose |
|------|---------|
| Empty rule set | `LanguageCode` is exposed; `Rules` is empty |
| Single rule | `Rules` exposes exactly the declared rule |
| Ordering | Multi-rule sets preserve declared order and count |

### GrammarRuleExceptionTests

| Area | Purpose |
|------|---------|
| Default constructor | `RuleId` is preserved and the message names the rule |
| Custom message | Message is preserved alongside `RuleId` |
| Inner exception | Inner exception is preserved with default or custom message |
| Edge cases | Null `RuleId` renders an empty identifier; type derives from `Exception` |

### Rule-specific tests

- `TrimWhitespaceRule` — Trimming, collapsing, tab/newline/form-feed handling, non-breaking space, empty input, idempotency
- `SpacingRule` — Spaces before punctuation, spaces after punctuation, all supported emoticons, unchanged text, idempotency
- `PatternReplacementRuleBase` — Whole-word replacement at start, middle, and end of text, all word terminators, partial-word non-matches, chained replacements, regex capture groups, `RegexReplacement` equality

## Test patterns

### Testing a custom rule

```csharp
[TestFixture]
public class MyRuleTests
{
    MyRule rule;

    [SetUp]
    public void SetUp() => rule = new MyRule();

    [Test]
    public void GivenMatchingInput_WhenCanApply_ThenReturnsTrue()
    {
        Assert.That(rule.CanApply("input"), Is.True);
    }

    [Test]
    public void GivenNonMatchingInput_WhenCanApply_ThenReturnsFalse()
    {
        Assert.That(rule.CanApply("other"), Is.False);
    }

    [Test]
    public void GivenNull_WhenCanApply_ThenThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => rule.CanApply(null!));
    }

    [Test]
    public void GivenMatchingInput_WhenApply_ThenReturnsExpected()
    {
        string result = rule.Apply("input");
        Assert.That(result, Is.EqualTo("expected"));
    }
}
```

### Testing rule ordering

```csharp
[Test]
public void GivenTwoRules_WhenCorrecting_ThenAppliesInOrder()
{
    var ruleSet = new TestRuleSet(
        new RuleA(), // Runs first
        new RuleB()  // Runs second
    );
    var corrector = new GrammarCorrector(ruleSet);

    string result = corrector.Correct("input");

    // Verify RuleA ran before RuleB
    Assert.That(result, Is.EqualTo("expected"));
}
```

### Testing exceptions

```csharp
[Test]
public void GivenRuleThrows_WhenCorrecting_ThenWrapsInGrammarRuleException()
{
    var ruleSet = new ThrowingRuleSet();
    var corrector = new GrammarCorrector(ruleSet);

    var ex = Assert.Throws<GrammarRuleException>(() => corrector.Correct("input"));
    Assert.That(ex.RuleId, Is.EqualTo("throwing-rule"));
}
```

## Mocking

Moq used for testing interactions:

```csharp
var mockRule = new Mock<IGrammarRule>();
mockRule.Setup(r => r.CanApply("text")).Returns(true);
mockRule.Setup(r => r.Apply("text")).Returns("result");
mockRule.Setup(r => r.Id).Returns("mock-rule");
```

## Coverage targets

| Component | Target |
|-----------|--------|
| Public API | 100% |
| Exception paths | 100% |
| Rule logic | 100% |
| Edge cases | 100% |

## CI integration

Tests run in GitHub Actions (`.github/workflows/dotnet.yml`):

```yaml
- name: Test
  run: dotnet test --no-restore --verbosity normal
```

## Adding new tests

1. Create test file in appropriate location
2. Follow naming convention
3. Cover: happy path, null input, edge cases
4. Run `dotnet test` to verify
5. Ensure CI passes

## Test data

Fixture values follow the shared `test-values` skill guidance; use short Romanian-language phrases (`salut`, `salut lume`, `bună`) for correction samples.