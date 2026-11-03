# Change guide

## Common modification patterns

### Adding a new built-in rule

1. Create new rule class in `NuciText.Grammar/Rules/`
2. Inherit from `GrammarRule` or `PatternReplacementRuleBase`
3. Implement required members: `Id`, `Description`, `DoApply`
4. Optionally override `CheckApplicability` for performance
5. Add tests in `NuciText.Grammar.UnitTests/Rules/`
6. Consider visibility: `public` for consumer use, `internal` for internal use

### Adding a new language rule set

1. Create new rule set class (typically in consumer project, not library)
2. Inherit from `GrammarRuleSet`
3. Implement `LanguageCode` (IETF tag)
4. Implement `Rules` returning ordered `IReadOnlyList<IGrammarRule>`
4. Include built-in rules and custom rules in desired order

### Modifying existing rule behavior

1. Locate rule in `NuciText.Grammar/Rules/`
2. Modify `DoApply` method
3. Update `Description` if behavior changes
4. Update/add tests to cover new behavior
5. Consider backward compatibility — rule `Id` should not change

### Changing rule ordering

1. Locate rule set implementation (consumer project)
2. Reorder rules in `Rules` property
3. Test with representative inputs
4. Document ordering rationale

## Safe modification checklist

- [ ] Run existing tests: `dotnet test`
- [ ] Add tests for new/changed behavior
- [ ] Verify formatting: `dotnet format --verify-no-changes`
- [ ] Check public API surface unchanged (unless intentional)
- [ ] Update documentation if public API changes
- [ ] Consider version bump for breaking changes

## Breaking changes

The following constitute breaking changes:
- Changing `Id` of any public rule
- Removing public types or members
- Changing method signatures in public interfaces
- Changing exception types thrown
- Changing rule behavior without version bump

## Versioning

- Follow semantic versioning
- Patch: bug fixes, internal improvements
- Minor: new rules, new public APIs (backward compatible)
- Major: breaking changes to public API