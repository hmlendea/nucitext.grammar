namespace NuciText.Grammar.Rules;

/// <summary>
/// Trims leading and trailing whitespace from Romanian text.
/// </summary>
public sealed class TrimWhitespaceRule : GrammarRule
{
    /// <inheritdoc/>
    public override string Id => "trim-whitespace";

    /// <inheritdoc/>
    public override string Description => "Trims leading and trailing whitespace.";

    /// <inheritdoc/>
    protected override string DoApply(string text)
        => text.Trim();
}