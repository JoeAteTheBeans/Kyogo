using Kyogo.Domain.Vocabulary.Senses;

namespace Kyogo.Application.Vocabulary.Modifications.Senses;

public sealed class SenseRemoval
{
    public required SenseId RemoveSenseId { get; init; }
}