# NuciText.Grammar Documentation Index

## Repository summary

NuciText.Grammar is a lightweight, dependency-free .NET library for rule-based grammar and text normalisation. It provides a minimal core API (`IGrammarRule`, `IGrammarRuleSet`, `IGrammarCorrector`) with a concrete `GrammarCorrector` implementation that applies rules sequentially. Built-in rules include whitespace trimming/collapsing and punctuation/emoticon spacing. The library targets .NET 10, has zero external dependencies, and is licensed under GPL-3.0-or-later.

## Root document map

| Document | Purpose |
|----------|---------|
| [ARCHITECTURE.md](../ARCHITECTURE.md) | High-level architecture, layers, runtime flow, components |
| [SECURITY.md](../SECURITY.md) | Security policy, vulnerability reporting, threat model |
| [PRIVACY.md](../PRIVACY.md) | Data handling, privacy practices |
| [LICENSE](../LICENSE) | GPL-3.0-or-later license text |
| [CONTRIBUTING.md](../CONTRIBUTING.md) | Contribution guidelines, code style, PR process |
| [README.md](../README.md) | Project overview, installation, basic usage |

## Documentation catalogue

### Getting started

| File | Description |
|------|-------------|
| [quick-start.md](quick-start.md) | Prerequisites, installation, first correction example |
| [api-usage-examples.md](api-usage-examples.md) | Basic usage, multiple rules, custom rules, patterns, DI integration |

### Architecture & design

| File | Description |
|------|-------------|
| [architecture.md](architecture.md) | Elaborated architecture: layers, runtime flow, components, dependencies |
| [data-model.md](data-model.md) | Stateless string model, rule data flow, built-in rule data, exceptions |
| [design-decisions.md](design-decisions.md) | 10 architecture decisions, 3 testing decisions, 5 build/packaging decisions |
| [invariants.md](invariants.md) | System-wide invariants: null safety, rule order, statelessness, determinism |

### Components

| File | Description |
|------|-------------|
| [components/core.md](components/core.md) | All core interfaces, base classes, concrete implementations, rule components |

### Flows

| File | Description |
|------|-------------|
| [flows/text-correction.md](flows/text-correction.md) | Sequence diagram, step-by-step, error paths, rule ordering, custom rule/set creation |

### Behaviour

| File | Description |
|------|-------------|
| [behaviour/text-correction.md](behaviour/text-correction.md) | Whitespace normalisation, punctuation spacing, rule chaining, extensibility, errors |

### API reference

| File | Description |
|------|-------------|
| [api-reference/INDEX.md](api-reference/INDEX.md) | Public API surface table, internal API, package info, usage example |

### Operations

| File | Description |
|------|-------------|
| [build-and-deployment.md](build-and-deployment.md) | CI workflow, GitHub Release, NuGet.org, local dev commands, versioning |
| [configuration.md](configuration.md) | No config system; compile-time settings; consumer-controlled configuration |
| [dependencies.md](dependencies.md) | Zero external deps; test packages; internal dependencies; dependency graph |
| [logging.md](logging.md) | No built-in logging; 12-section analysis; consumer logging guidance |
| [concurrency-and-scheduling.md](concurrency-and-scheduling.md) | Thread-safe, no async, no scheduling, embarrassingly parallel |
| [state-and-persistence.md](state-and-persistence.md) | Stateless, no persistence, no migrations, consumer state external |
| [security.md](security.md) | Minimal attack surface, threat model, data handling, compliance |
| [testing.md](testing.md) | NUnit 4.x + Moq strategy, test structure, naming, patterns, coverage |

### Maintenance

| File | Description |
|------|-------------|
| [documentation-maintenance.md](documentation-maintenance.md) | Structure, update triggers, process, standards, ownership, coverage |
| [change-guide.md](change-guide.md) | Modification patterns, safe checklist, breaking changes, versioning |
| [ambiguities-and-open-questions.md](ambiguities-and-open-questions.md) | 8 known gaps with TODO checklist |

### Reference

| File | Description |
|------|-------------|
| [faq.md](faq.md) | General, usage, built-in rules, extensibility, performance, licensing, contributing |
| [troubleshooting.md](troubleshooting.md) | Common issues: rule not applied, exceptions, SpacingRule internal, performance |

## Navigation aids

### Start here (newcomers)

1. [quick-start.md](quick-start.md) — Get running in 5 minutes
2. [api-usage-examples.md](api-usage-examples.md) — See common patterns
3. [components/core.md](components/core.md) — Understand the core API
4. [faq.md](faq.md) — Answers to common questions

### Deep dive (component owners)

1. [architecture.md](architecture.md) — Full architecture context
2. [design-decisions.md](design-decisions.md) — Why decisions were made
3. [invariants.md](invariants.md) — Contracts that must never break
4. [components/core.md](components/core.md) — Complete API reference
5. [change-guide.md](change-guide.md) — How to modify safely

### Flows (debuggers)

1. [flows/text-correction.md](flows/text-correction.md) — End-to-end correction flow
2. [behaviour/text-correction.md](behaviour/text-correction.md) — Observable behaviors
3. [troubleshooting.md](troubleshooting.md) — Diagnose issues
4. [logging.md](logging.md) — Add observability

## Maintenance metadata

| Field | Value |
|-------|-------|
| Last reviewed | 2026-11-02 |
| Owner | NuciText maintainers |
| Coverage status | Complete — all planned files created |
| Next review | On next release or major change |

---

*This index is the master navigation document for the `docs/` directory. All cross-references use relative links. Headings use sentence case. Filenames use kebab-case.*