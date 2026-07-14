using System;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Playground.Domain.Entities;
using Playground.Infrastructure.Data;
using Playground.Infrastructure.Repositories;
using Xunit;

namespace Playground.Tests.Infrastructure;

public class SqlitePetRepositoryTests : IDisposable
{
    private readonly DbConnection _connection;
    private readonly PetDbContext _context;
    private readonly SqlitePetRepository _sut; // System Under Test

    public SqlitePetRepositoryTests()
    {
        // 1. Creiamo e apriamo la connessione al DB SQLite in memoria.
        // NOTA: Deve rimanere aperta per tutta la durata del test, altrimenti il DB viene distrutto.
        _connection = new SqliteConnection("Filename=:memory:");
        _connection.Open();

        // 2. Configuriamo il DbContext per usare questa specifica connessione
        var options = new DbContextOptionsBuilder<PetDbContext>()
            .UseSqlite(_connection)
            .Options;

        _context = new PetDbContext(options);
        
        // 3. Creiamo lo schema del database (tabelle) basandoci sul nostro modello EF Core
        _context.Database.EnsureCreated();

        // 4. Istanziamo il repository da testare
        _sut = new SqlitePetRepository(_context);
    }

    [Fact]
    public async Task AddAsync_ShouldSavePetToDatabase()
    {
        // Arrange
        // Usiamo il costruttore completo: id, nickname, age, physicalDescription, personalityDescription, suggestedDonation
        var newDog = new Dog("dog-123", "Rex", 3, "German Shepherd", "Loyal and protective", 30.5m) { IsUrgent = true };

        // Act
        await _sut.AddAsync(newDog);

        // Assert
        // Invece di far tradurre "Nickname" a EF Core in SQL, cerchiamo per Id (che è la Primary Key garantita).
        var savedPet = await _context.Pets.OfType<Dog>().FirstOrDefaultAsync(p => p.Id == "dog-123");
        
        Assert.NotNull(savedPet);
        Assert.IsType<Dog>(savedPet);
        Assert.Equal("Rex", savedPet.Nickname); // Verifichiamo il Nickname direttamente in memoria
        Assert.True(savedPet.IsUrgent);
    }

    [Fact]
    public async Task SearchPetsAsync_ShouldFilterCorrectly_InMem()
    {
        // Arrange
        // Usiamo i costruttori completi per Dog e Cat
        var dog = new Dog("dog-456", "Buddy", 5, "Golden Retriever", "Very playful", 15m) { IsUrgent = false };
        var cat = new Cat("cat-789", "Luna", 2, "Persian", "Loves to sleep", 5m) { IsUrgent = true };
        
        _context.Pets.AddRange(dog, cat);
        await _context.SaveChangesAsync();

        // Act
        var results = await _sut.SearchPetsAsync(new[] { "Luna" });

        // Assert
        Assert.Single(results);
        Assert.IsType<Cat>(results.First());
        
        // Eseguiamo un cast a Cat per leggere in sicurezza la proprietà Nickname
        var foundCat = (Cat)results.First();
        Assert.Equal("Luna", foundCat.Nickname);
    }

    // Cleanup alla fine di ogni test
    public void Dispose()
    {
        _context.Dispose();
        _connection.Dispose(); // Questo distrugge effettivamente il DB in memoria
    }
}