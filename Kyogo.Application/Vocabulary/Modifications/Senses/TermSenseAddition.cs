namespace Kyogo.Application.Vocabulary.Modifications.Senses;

public sealed class TermSenseAddition
{
    public required int InsertionIndex { get; set; }
    
    public required AdditionalTermSense AdditionalTermSense { get; init; }
}