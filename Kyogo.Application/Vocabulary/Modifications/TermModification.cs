using Kyogo.Application.Vocabulary.Modifications.Senses;
using Kyogo.Domain.Users;
using Kyogo.Domain.Vocabulary;

namespace Kyogo.Application.Vocabulary.Modifications;

public sealed class TermModification
{
    public required UserId OwnerId { get; init; }
    
    public required TermId TermId { get; init; }
    
    public ICollection<TermSenseRemoval> SenseRemovals { get; init; } = [];

    public ICollection<TermSenseModification> SenseModifications { get; init; } = [];

    public ICollection<TermSenseAddition> SenseAdditions { get; init; } = [];
}