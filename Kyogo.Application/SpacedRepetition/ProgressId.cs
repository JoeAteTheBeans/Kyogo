using Kyogo.Domain.Vocabulary;

namespace Kyogo.Application.SpacedRepetition;

public readonly record struct ProgressId(string Id)
{
    public static ProgressId Create(TermId termId, CardType cardType)
        => new ProgressId($"{cardType}:{termId.Value}");
    
    public static TermId DeriveTermId(ProgressId progressId)
        => new TermId(Guid.Parse(progressId.Id.Split(':')[1]));

    public static CardType DeriveCardType(ProgressId progressId)
        => Enum.TryParse(progressId.Id.Split(':')[0], true, out CardType cardType) ? cardType : throw new InvalidOperationException($"ProgressId {progressId} has an invalid card type");

    public override string ToString() 
        => Id;
}