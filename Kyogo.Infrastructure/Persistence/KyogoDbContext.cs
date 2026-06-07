using System.Reflection;
using Kyogo.Application.Authentication.Tokens;
using Kyogo.Application.SpacedRepetition;
using Kyogo.Application.Vocabulary.Modifications;
using Kyogo.Domain.Characters;
using Kyogo.Domain.Decks;
using Kyogo.Domain.Users;
using Kyogo.Domain.Vocabulary;
using Microsoft.EntityFrameworkCore;

namespace Kyogo.Infrastructure.Persistence;

public class KyogoDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    
    public DbSet<Term> Terms { get; set; }
    
    public DbSet<Kanji> Kanji { get; set; }
    
    public DbSet<Deck> Decks { get; set; }
    
    public DbSet<DeckSubscription>  DeckSubscriptions { get; set; }
    
    public DbSet<TermModification>  TermModifications { get; set; }
    
    public DbSet<Progress> ProgressRecords { get; set; }
    
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSnakeCaseNamingConvention();
}