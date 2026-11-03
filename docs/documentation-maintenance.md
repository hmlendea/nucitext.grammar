# Documentation maintenance

## Documentation structure

All generated documentation lives in `docs/` directory:

```
docs/
├── api-reference/           # Public API reference
├── behaviour/               # User-facing behaviours
├── components/              # Per-component deep dives
├── flows/                   # End-to-end execution flows
├── api-usage-examples.md    # Example requests/responses
├── ambiguities-and-open-questions.md  # Unresolved items
├── architecture.md          # High-level architecture
├── build-and-deployment.md  # Build pipeline, deployment
├── change-guide.md          # How to modify common areas
├── configuration.md         # Configuration schema
├── concurrency-and-scheduling.md    # Threading, async
├── data-model.md            # Domain entities, relationships
├── dependencies.md          # External and internal dependencies
├── design-decisions.md      # Key architectural choices
├── error-handling.md        # Error taxonomy, handling
├── faq.md                   # Frequently asked questions
├── INDEX.md                 # Master index and navigation
├── invariants.md            # System-wide invariants
├── integrations.md          # External system integrations
├── logging.md               # Logging framework
├── quick-start.md           # Getting started guide
├── repository-overview.md   # Repository purpose, scope
├── repository-structure.md  # Source tree layout
├── security.md              # Security model
├── state-and-persistence.md # State management
├── testing.md               # Test strategy
└── troubleshooting.md       # Common issues
```

## Maintenance responsibilities

### When to update documentation

| Trigger | Files to review |
|---------|-----------------|
| New public API | `api-reference/`, `api-usage-examples.md`, `architecture.md` |
| New rule/rule set | `components/`, `behaviour/`, `api-usage-examples.md` |
| Behavior change | `behaviour/`, `flows/`, `api-usage-examples.md` |
| Build/deploy change | `build-and-deployment.md` |
| New dependency | `dependencies.md` |
| Architectural decision | `design-decisions.md`, `architecture.md` |
| Bug fix | `troubleshooting.md`, `testing.md` |
| Configuration change | `configuration.md` |

### Update process

1. Make code changes
2. Update relevant documentation files
3. Run `dotnet format` to ensure code style
4. Run `dotnet test` to verify behavior
5. Commit documentation with code changes

## Documentation standards

### File naming
- kebab-case: `state-and-persistence.md`
- Directories: plural kebab-case: `components/`, `flows/`

### Headings
- Sentence case: `## Repository purpose`
- Use `##` for top-level sections in each file

### Cross-references
- Relative links from `docs/` root: `[Architecture](./architecture.md)`
- Code symbols: backticks with full namespace: `` `NuciText.Grammar.GrammarCorrector` ``

### Code examples
- Complete, compilable snippets
- Include necessary `using` statements
- Show realistic usage

## Ownership

- **Primary**: Repository maintainers
- **Contributors**: Update docs with their changes
- **Review**: Documentation changes reviewed in PRs

## Coverage status

| Area | Status | Last reviewed |
|------|--------|---------------|
| Architecture | ✅ Complete | 2026-11-02 |
| Components | ✅ Complete | 2026-11-02 |
| Flows | ✅ Complete | 2026-11-02 |
| Behaviours | ✅ Complete | 2026-11-02 |
| API Reference | ✅ Complete | 2026-11-02 |
| Data Model | ✅ Complete | 2026-11-02 |
| Dependencies | ✅ Complete | 2026-11-02 |
| Build/Deploy | ✅ Complete | 2026-11-02 |
| Configuration | ✅ Complete | 2026-11-02 |
| Design Decisions | ✅ Complete | 2026-11-02 |
| Testing | ⏳ Pending | — |
| Security | ⏳ Pending | — |
| Error Handling | ⏳ Pending | — |
| Logging | ⏳ Pending | — |
| Concurrency | ⏳ Pending | — |
| State/Persistence | ⏳ Pending | — |
| Integrations | ⏳ Pending | — |
| Invariants | ⏳ Pending | — |
| FAQ | ⏳ Pending | — |
| Troubleshooting | ⏳ Pending | — |
| Quick Start | ⏳ Pending | — |
| Change Guide | ✅ Complete | 2026-11-02 |
| Ambiguities | ✅ Complete | 2026-11-02 |