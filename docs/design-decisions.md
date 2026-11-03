# Design decisions

## Core architecture decisions

### 1. Interface-first design

**Decision**: Define public contracts as interfaces (`IGrammarRule`, `IGrammarRuleSet`, `IGrammarCorrector`)

**Rationale**:
- Enables consumer implementations without library coupling
- Supports testing with mocks/stubs
- Allows multiple implementations for same contract
- Stable versioning surface

**Alternatives considered**:
- Abstract classes only — rejected: less flexible for consumers
- Concrete classes only — rejected: no extensibility

### 2. Base classes with default implementations

**Decision**: Provide `GrammarRule` and `GrammarRuleSet` abstract base classes

**Rationale**:
- Reduces boilerplate for common cases
- `GrammarRule` provides default `CanApply` (compares `Apply` result to input)
- `GrammarRuleSet` enforces structure without mandating implementation details
- Consumers can still implement interfaces directly if needed

### 3. Sealed concrete implementations

**Decision**: `GrammarCorrector`, `TrimWhitespaceRule` are `sealed`

**Rationale**:
- Prevents unintended inheritance that could break invariants
- `GrammarCorrector` has simple, fixed algorithm — no extension point needed
- `TrimWhitespaceRule` is a leaf implementation
- Consumers extend via interfaces, not inheritance

### 4. Stateless, pure string transformations

**Decision**: All operations are pure functions: `string → string`

**Rationale**:
- Thread-safe by default
- No hidden state or side effects
- Easy to test and reason about
- Composable and cacheable
- No resource management needed

### 5. Sequential rule application

**Decision**: Rules applied in order, each receiving previous output

**Rationale**:
- Deterministic behavior
- Simple mental model
- Rule ordering is explicit in rule set
- Enables pipeline patterns (trim → spacing → custom)

**Alternatives considered**:
- Parallel application — rejected: non-deterministic, complex conflict resolution
- Dependency graph — rejected: over-engineered for use case

### 6. CanApply/Apply separation

**Decision**: Two-phase rule application: `CanApply` check, then `Apply`

**Rationale**:
- Allows efficient pre-check without full transformation
- Default implementation compares result to input
- Consumers can override for performance
- Clear separation of concerns

### 7. Exception for rule failures

**Decision**: `GrammarRuleException` with `RuleId` for rule execution failures

**Rationale**:
- Distinguishes rule failures from argument validation
- Carries rule identity for diagnostics
- Standard exception hierarchy
- Three constructor overloads for flexibility

### 8. No external dependencies

**Decision**: Zero NuGet dependencies in main library

**Rationale**:
- Minimal attack surface
- No version conflicts for consumers
- Easy to audit
- Fast restore/build
- Suitable for constrained environments

### 9. Internal SpacingRule

**Decision**: `SpacingRule` is `internal`

**Rationale**:
- Implementation detail of potential future language packs
- Not yet stabilized for public API
- Can be made public later without breaking changes

**Open question**: Should be public? See [ambiguities](../ambiguities-and-open-questions.md)

### 10. PatternReplacementRuleBase for word replacements

**Decision**: Specialized base class for whole-word regex replacements

**Rationale**:
- Common pattern in grammar correction
- Handles word boundaries and terminators correctly
- Reduces error-prone regex in consumer code
- `RegexReplacement` record for clean data representation

## Testing decisions

### 1. NUnit 4.x + Moq

**Decision**: Use NUnit 4.x and Moq for unit tests

**Rationale**:
- Project standard (per instructions)
- Mature, well-supported
- Good integration with dotnet test

### 2. Test project structure

**Decision**: Separate test project (`NuciText.Grammar.UnitTests`)

**Rationale**:
- Clear separation of concerns
- No `InternalsVisibleTo` needed for public API tests
- `InternalsVisibleTo` only for testing internal `SpacingRule`

### 3. Test naming convention

**Decision**: `GivenX_WhenY_ThenZ` pattern

**Rationale**:
- Readable, behavior-focused
- Consistent with project standards
- Clear arrangement of test scenarios

## Build and packaging decisions

### 1. Target net10.0

**Decision**: Target .NET 10.0

**Rationale**:
- Current LTS at time of writing
- Modern language features
- Long support window

### 2. Documentation file generation

**Decision**: `<GenerateDocumentationFile>true</GenerateDocumentationFile>`

**Rationale**:
- Enables IntelliSense for consumers
- Required for NuGet best practices
- XML docs on all public APIs

### 3. ImplicitUsings disabled

**Decision**: `<ImplicitUsings>disable</ImplicitUsings>`

**Rationale**:
- Explicit control over namespace imports
- Consistent with project coding standards
- Avoids hidden dependencies

### 4. Nullable enabled

**Decision**: `<Nullable>enable</Nullable>`

**Rationale**:
- Compile-time null safety
- Consistent with modern .NET practices
- Library code written as if NRT enabled

### 5. GPL-3.0-or-later license

**Decision**: Copyleft license

**Rationale**:
- Ensures derivative works remain open source
- Aligns with project philosophy
- Clear obligations for consumers