using Kyogo.Domain.Vocabulary.Glosses;

namespace Kyogo.Application.Vocabulary.Modifications.Glosses;

public sealed class GlossRemoval
{
    public required GlossId RemoveGlossId { get; init; }
}