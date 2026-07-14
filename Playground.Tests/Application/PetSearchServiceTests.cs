using System;
using System.Linq;
using System.Threading.Tasks;
using System.Data.Common;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Playground.Application.Services;
using Playground.Domain.Entities;
using Playground.Infrastructure.Data;
using Playground.Infrastructure.Repositories;
using Xunit;

namespace Playground.Tests.Application;

// Implementiamo IDisposable per pulire il DB in memoria alla fine di ogni test
public class PetSearchServiceTests : IDisposable
{
    private readonly DbConnection _connection;
    private readonly PetDbContext _context;
    private readonly PetSearchService _sut; // System Under Test

    public PetSearchServiceTests()
    {
        // 1. Creiamo il DB in memoria
        _connection = new SqliteConnection("Filename=:memory:");
        _connection.Open();

        var options = new DbContextOptionsBuilder<PetDbContext>()
            .UseSqlite(_connection)
            .Options;

        _context = new PetDbContext(options);
        _context.Database.EnsureCreated();

        // 2. Prepariamo i dati (Seed) che i test si aspettano di trovare
        // Creiamo Gus e Lola con le caratteristiche cercate nel test
        var gus = new Dog("d-1", "Gus", 3, "Golden Retriever, Male", "Playful", 10m);
        var lola = new Dog("d-2", "Lola", 2, "Golden Retriever, Female", "Sweet", 10m);
        
        _context.Pets.AddRange(gus, lola);
        _context.SaveChanges();

        // 3. Inizializziamo Repo e Service reali (Integration Test)
        var repo = new SqlitePetRepository(_context);
        _sut = new PetSearchService(repo);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public async Task SearchPets_RestituisceListaVuota_SeInputEInvalido(string? badInput)
    {
        // Act
        var result = await _sut.SearchPetsByCharacteristicsAsync(badInput!);

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public async Task SearchPets_PulisceInputSporco_ETrovaRisultatiCorretti()
    {
        // Arrange
        // Un utente disordinato digita questo scempio:
        string dirtyInput = " , golden ,,,   MaLe , ";

        // Act
        var result = await _sut.SearchPetsByCharacteristicsAsync(dirtyInput);

        // Assert
        // Il servizio pulisce l'input in ["golden", "male"].
        // Troverà Gus (golden + male) e Lola (golden + female).
        Assert.Equal(2, result.Count);
        
        var nicks = result.Select(p => p.Nickname.ToLower()).ToList();
        Assert.Contains("gus", nicks);
        Assert.Contains("lola", nicks);
    }

    public void Dispose()
    {
        _context.Dispose();
        _connection.Dispose();
    }
}