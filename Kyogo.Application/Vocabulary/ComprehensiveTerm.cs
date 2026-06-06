using Kyogo.Application.SpacedRepetition;
using Kyogo.Application.Vocabulary.Modifications;
using Kyogo.Application.Vocabulary.Modifications.Senses;
using Kyogo.Domain.Vocabulary;
using Kyogo.Domain.Vocabulary.Components;
using Kyogo.Domain.Vocabulary.Senses;

namespace Kyogo.Application.Vocabulary;

public sealed class ComprehensiveTerm
{ 
    public TermId Id { get; init; }
    
    public IReadOnlyList<IComponent> Components { get; init; }
    
    public IReadOnlyList<ComprehensiveTermSense> Senses { get; init; }
    
    public IReadOnlyCollection<Progress> Progress { get; init; }
    
   public ComprehensiveTerm(Term term, TermModification? modification, IReadOnlyCollection<Progress> progress)
   {
       Id = term.Id;
       Components = term.Components;
       List<ComprehensiveTermSense> senses = [];
       foreach (Sense sense in term.Senses)
           senses.Add(new ComprehensiveTermSense(sense, modification?.SenseModifications.FirstOrDefault(x => x.SenseId == sense.Id)));
       if (modification != null)
       {
           foreach(SenseRemoval removal in  modification.SenseRemovals)
               senses.RemoveAll(x => x.Id == removal.RemoveSenseId);
           foreach (SenseAddition addition in modification.SenseAdditions.OrderByDescending(x => x.InsertionIndex).ToList())
               senses.Insert(addition.InsertionIndex, new ComprehensiveTermSense(addition));
       }
       Senses = senses;
       Progress = progress;
   }
}