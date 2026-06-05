using Kyogo.Domain.Vocabulary.Components;
using Kyogo.Domain.Vocabulary.Senses;

namespace Kyogo.Domain.Vocabulary;

public sealed class Term
{
    public required TermId Id { get; init; }

    public required IReadOnlyList<IComponent> Components { get; init; }

    public required IReadOnlyList<Sense> Senses { get; init; }
}