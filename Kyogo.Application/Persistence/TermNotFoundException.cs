using Kyogo.Domain.Vocabulary;

namespace Kyogo.Application.Persistence;

public sealed class TermNotFoundException(TermId termId) : Exception($"Term with id {termId} not found")
{
    public TermId TermId { get; } = termId;
}