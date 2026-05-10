using Playground.Domain.Entities;

namespace Playground.Tests.Domain;

public class PetTests
{
    [Fact]
    public void Constructor_CreaIstanzaValida_SeDatiCorretti()
    {
        // Arrange & Act
        // Usiamo Dog perché Pet è astratto e non può essere istanziato direttamente
        var dog = new Dog("d1", "Rex", 5, "Pelo corto", "Giocherellone", 50.0m);

        // Assert
        Assert.Equal("d1", dog.Id);
        Assert.Equal("Rex", dog.Nickname);
        Assert.Equal("dog", dog.Species); // Verifichiamo che l'override della specie funzioni
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void Constructor_LanciaEccezione_SeIdVuotoONullo(string? invalidId)
    {
        // Arrange, Act & Assert
        // ThrowsAny accetta sia ArgumentException che le sue classi figlie (come ArgumentNullException)
        var exception = Assert.ThrowsAny<ArgumentException>(() => 
            new Dog(invalidId!, "Rex", 5, "Pelo corto", "Giocherellone", 50.0m));
        
        // Bonus: controlliamo che l'eccezione indichi il parametro giusto
        Assert.Equal("id", exception.ParamName);
    }

    [Fact]
    public void Constructor_LanciaEccezione_SeDonazioneNegativa()
    {
        // Arrange
        decimal donazioneNegativa = -10.50m;

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => 
            new Cat("c1", "Silvestro", 3, "Bianco e nero", "Cacciatore", donazioneNegativa));
    }

    [Fact]
    public void GetFullDescription_ConcatenaCorrettamenteLeDescrizioni()
    {
        // Arrange
        var cat = new Cat("c1", "Micia", 2, "Pelo lungo.", "Molto timida.", 40.0m);

        // Act
        var fullDesc = cat.GetFullDescription();

        // Assert
        Assert.Equal("Pelo lungo. Molto timida.", fullDesc);
    }
}