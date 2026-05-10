using Playground.Application.Services;
using Playground.Infrastructure.Repositories;

namespace Playground.Tests.Application;

public class PetSearchServiceTests
{
    // NOTA DA TECH LEAD: 
    // In un test unitario "puro", qui non useremmo l'InMemoryPetRepository vero, 
    // ma creeremmo un "Mock" (una finta) dell'IPetRepository per isolare il test.
    // Essendo però il nostro repo attuale già in memoria e istantaneo, lo usiamo 
    // direttamente. Questo tecnicamente lo rende un "Integration Test" leggero.

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public async Task SearchPets_RestituisceListaVuota_SeInputEInvalido(string? badInput)
    {
        // Arrange
        var repo = new InMemoryPetRepository();
        var sut = new PetSearchService(repo);

        // Act
        // Usiamo badInput! perché sappiamo cosa stiamo facendo (null forgiving)
        var result = await sut.SearchPetsByCharacteristicsAsync(badInput!);

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public async Task SearchPets_PulisceInputSporco_ETrovaRisultatiCorretti()
    {
        // Arrange
        var repo = new InMemoryPetRepository();
        var sut = new PetSearchService(repo);
        
        // Un utente disordinato digita questo scempio:
        string dirtyInput = " , golden  ,,,   MaLe , ";

        // Act
        var result = await sut.SearchPetsByCharacteristicsAsync(dirtyInput);

        // Assert
        // Il servizio pulisce l'input in ["golden", "male"].
        // Poiché il nostro motore usa una logica OR (Any), troverà sia Gus (che è "golden" e "male") 
        // sia Lola (che è "golden" ma female).
        Assert.Equal(2, result.Count);
        
        // Verifichiamo che ci siano esattamente Gus e Lola
        var nicks = result.Select(p => p.Nickname.ToLower()).ToList();
        Assert.Contains("gus", nicks);
        Assert.Contains("lola", nicks);
    }
}