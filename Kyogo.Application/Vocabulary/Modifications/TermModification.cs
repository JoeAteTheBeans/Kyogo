using Kyogo.Application.Vocabulary.Modifications.Senses;
using Kyogo.Domain.Users;
using Kyogo.Domain.Vocabulary;

namespace Kyogo.Application.Vocabulary.Modifications;

public sealed class TermModification
{
    public required TermId TermId { get; init; }
    
    public required UserId OwnerId { get; init; }
    
    public ICollection<SenseRemoval> SenseRemovals { get; init; } = [];

    public ICollection<SenseModification> SenseModifications { get; init; } = [];

    public ICollection<SenseAddition> SenseAdditions { get; init; } = [];
}