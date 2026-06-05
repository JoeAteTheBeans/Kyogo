using Kyogo.Domain.Vocabulary.Senses;

namespace Kyogo.Application.Vocabulary.Modifications.Senses;

public sealed class TermSenseRemoval
{
    public required SenseId RemoveSenseId { get; init; }
}