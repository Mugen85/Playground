using Playground.Infrastructure.Repositories;

namespace Playground.Tests.Infrastructure;

public class InMemoryPetRepositoryTests
{
    // Dato che il nostro InMemoryPetRepository si "auto-popola" nel costruttore 
    // con 4 animali storici (2 cani e 2 gatti), sfrutteremo quei dati noti per i test.

    [Fact]
    public async Task GetAllAsync_RestituisceTuttiGliAnimaliIniziali()
    {
        // Arrange
        var sut = new InMemoryPetRepository(); // SUT = System Under Test (convenzione di naming)

        // Act
        var result = await sut.GetAllAsync();

        // Assert
        Assert.Equal(4, result.Count);
    }

    [Fact]
    public async Task SearchPetsAsync_TrovaSoloMale_IgnorandoFemale()
    {
        // Arrange
        var sut = new InMemoryPetRepository();
        var searchTerms = new[] { "male" };

        // Act
        var result = await sut.SearchPetsAsync(searchTerms);

        // Assert
        // Sappiamo che nei dati di seed c'è Gus (male) e ci sono Lola, Snow, Lion (female).
        // Il test deve restituire SOLO Gus.
        Assert.Single(result); 
        Assert.Equal("gus", result.First().Nickname.ToLower());
    }

    [Fact]
    public async Task SearchPetsAsync_RicercaCaseInsensitive_TrovaAnimale()
    {
        // Arrange
        var sut = new InMemoryPetRepository();
        // Nei seed c'è "golden retriever" tutto minuscolo. Cerchiamo con maiuscole.
        var searchTerms = new[] { "GOLDEN" }; 

        // Act
        var result = await sut.SearchPetsAsync(searchTerms);

        // Assert
        Assert.Equal(2, result.Count); // Deve trovare sia Lola che Gus
    }

    [Fact]
    public async Task SearchPetsAsync_NessunTermine_RestituisceListaVuota()
    {
        // Arrange
        var sut = new InMemoryPetRepository();

        // Act
        var result = await sut.SearchPetsAsync(new List<string>());

        // Assert
        Assert.Empty(result);
    }
}