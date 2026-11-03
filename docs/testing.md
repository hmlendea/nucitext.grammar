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
├── GrammarCorrectorTests.cs    # Corrector behavior
├── GrammarRuleTests.cs         # Base rule behavior
├── Rules/
│   ├── SpacingRule.cs          # SpacingRule tests
│   └── TrimWhitespaceRule.cs   # TrimWhitespaceRule tests
```

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

| Test class | Purpose |
|------------|---------|
| `NullRuleSet` | Constructor validates ruleSet |
| `NullText` | Correct validates input |
| `EmptyRuleSet` | No rules = identity |
| `SingleRuleSet` | One matching rule |
| `MultiRuleSet` | Multiple rules in order |
| `DoubleSpaceRule` | Custom rule integration |

### GrammarRuleTests

| Test | Purpose |
|------|---------|
| `CanApply_ReturnsTrue_WhenDoApplyChangesText` | Default CanApply logic |
| `CanApply_ReturnsFalse_WhenDoApplyReturnsSameText` | Default CanApply logic |
| `CanApply_Throws_WhenTextIsNull` | Null validation |
| `Apply_ReturnsDoApplyResult_WhenCanApplyTrue` | Apply delegation |
| `Apply_Throws_WhenTextIsNull` | Null validation |

### Rule-specific tests

- `TrimWhitespaceRule` — Trimming, collapsing, edge cases
- `SpacingRule` — Punctuation, emoticons, edge cases

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

Use standard test values from [common.test-values.instructions.md](../../../.config/Code%20-%20OSS/User/prompts/instructions/common/common.test-values.instructions.md):

- Names: Alice, Bob, Carol
- Emails: alice@example.com
- Dates: 2026-01-15
- Locations: Springfield, IL