# Behaviours

## Text correction

### Normalise whitespace

**Rule**: `TrimWhitespaceRule` (`trim-whitespace`)

**Behaviour**: Trims leading/trailing whitespace and collapses internal whitespace sequences to single spaces.

| Input | Output |
|-------|--------|
| `"  Hello    world  "` | `"Hello world"` |
| `"Hello\t\tworld"` | `"Hello world"` |
| `"Hello\n\nworld"` | `"Hello world"` |
| `"  "` | `""` |
| `""` | `""` |

### Normalise punctuation spacing

**Rule**: `SpacingRule` (`spacing`)

**Behaviour**: Normalises spacing around punctuation marks and emoticons.

| Transformation | Pattern | Example |
|----------------|---------|---------|
| Remove spaces before commas/periods | `[ ]*([,.])` → `$1` | `"Hello , world"` → `"Hello, world"` |
| Add space after punctuation before letters | `([,.:;])([a-zA-Z][a-zA-Z])` → `$1 $2` | `"Hello,world"` → `"Hello, world"` |
| Add space before emoticons | `([a-z])([Oo][7/]\|[:;=][DdOoPpXx*)]\|[Xx][Dd])` → `$1 $2` | `"Hello:)"` → `"Hello :)"` |

**Emoticons handled**: `O7`, `o7`, `O/`, `o/`, `:D`, `:d`, `:O`, `:o`, `:P`, `:p`, `:X`, `:x`, `:)`, `:*)`, `XD`, `Xd`, `xD`, `xd`

### Rule chaining

Rules are applied in the order defined by `IGrammarRuleSet.Rules`. Each rule receives the output of the previous rule.

**Example chain**: `TrimWhitespaceRule` → `SpacingRule`

| Input | After TrimWhitespaceRule | After SpacingRule |
|-------|--------------------------|-------------------|
| `"  Hello  ,  world  :)"` | `"Hello , world :)"` | `"Hello, world :)"` |

## Extensibility behaviours

### Custom rule implementation

Consumers can implement `IGrammarRule` directly or inherit from `GrammarRule`:

- **Direct implementation**: Full control over `CanApply` and `Apply`
- **Inherit from `GrammarRule`**: Default `CanApply` checks if `Apply` would change text; implement `DoApply` only

### Custom rule set implementation

Consumers implement `IGrammarRuleSet` (typically via `GrammarRuleSet`):

- Define `LanguageCode` as IETF language tag
- Return ordered `IReadOnlyList<IGrammarRule>` in `Rules` property
- Rule order determines application priority

### Pattern-based rules

For word/phrase replacement rules, inherit from `PatternReplacementRuleBase`:

- Use `RegexReplacement` records for pattern/replacement pairs
- Use `ApplyWordReplacements` for whole-word matching with terminator handling
- Use `ApplyRegexReplacements` for direct regex replacements
- Use `ReplaceWord` for single word replacements with boundary checking

## Error behaviours

### Null input handling

- `GrammarCorrector.Correct(null)` → throws `ArgumentNullException`
- `GrammarRule.CanApply(null)` → throws `ArgumentNullException`
- `GrammarRule.Apply(null)` → throws `ArgumentNullException`
- `GrammarCorrector` constructor with null `ruleSet` → throws `ArgumentNullException`

### Rule failure handling

- Rules should wrap internal failures in `GrammarRuleException` with `RuleId`
- `GrammarCorrector` propagates exceptions from rules
- Consumers should catch `GrammarRuleException` to handle rule-specific failures

### Non-matching rules

- `CanApply` returning `false` is normal behaviour (not an error)
- Rule is skipped, text passes through unchanged
- Next rule receives same text