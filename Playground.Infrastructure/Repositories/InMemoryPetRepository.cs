using System.Collections.Concurrent;
using Playground.Domain.Entities;
using Playground.Domain.Repositories;

namespace Playground.Infrastructure.Repositories;

/// <summary>
/// Implementazione in memoria del repository. 
/// Ottima per lo sviluppo iniziale, i test o per simulare un DB.
/// </summary>
public class InMemoryPetRepository : IPetRepository
{
    // Il nostro "database" privato diventa un ConcurrentDictionary.
    // Thread-safe e velocissimo per i lookup. La chiave (string) sarà l'ID del Pet.
    private readonly ConcurrentDictionary<string, Pet> _pets;

    public InMemoryPetRepository()
    {
        // Inizializziamo il dizionario. Passiamo StringComparer.OrdinalIgnoreCase 
        // così la ricerca per chiave (ID) se ne frega delle maiuscole/minuscole in modo nativo.
        _pets = new ConcurrentDictionary<string, Pet>(StringComparer.OrdinalIgnoreCase);

        // Seeding dei dati
        var seedPets = new List<Pet>
        {
            new Dog("d1", "lola", 2, 
                "medium sized cream colored female golden retriever weighing about 45 pounds. housebroken.", 
                "loves to have her belly rubbed and likes to chase her tail. gives lots of kisses.", 
                85.00m),
                
            new Dog("d2", "gus", 9, 
                "large reddish-brown male golden retriever weighing about 85 pounds. housebroken.", 
                "loves to have his ears rubbed when he greets you at the door, or at any time! loves to lean-in and give doggy hugs.", 
                49.99m),
                
            new Cat("c3", "snow", 1, 
                "small white female weighing about 8 pounds. litter box trained.", 
                "friendly", 
                40.00m),
                
            new Cat("c4", "Lion", 3, 
                "Medium sized, long hair, yellow, female, about 10 pounds. Uses litter box.", 
                "A people loving cat that likes to sit on your lap.", 
                45.00m)
        };

        foreach (var pet in seedPets)
        {
            _pets.TryAdd(pet.Id, pet);
        }
    }

    public Task<IReadOnlyCollection<Pet>> GetAllAsync()
    {
        // .Values estrae solo gli oggetti Pet dal dizionario.
        // Chiamiamo ToList() per materializzare la collezione prima di fare AsReadOnly()
        return Task.FromResult<IReadOnlyCollection<Pet>>(_pets.Values.ToList().AsReadOnly());
    }

    public Task<Pet?> GetByIdAsync(string id)
    {
        // Prestazioni O(1): non scorre più tutta la lista. Va dritto al bersaglio.
        // TryGetValue è sicuro: se non trova nulla, out pet è null.
        _pets.TryGetValue(id, out var pet);
        return Task.FromResult(pet);
    }

    public Task AddAsync(Pet pet)
    {
        // TryAdd è thread-safe. Se l'ID esiste già, lo ignora (o potresti lanciare un'eccezione di dominio).
        _pets.TryAdd(pet.Id, pet);
        return Task.CompletedTask;
    }

    public Task<IReadOnlyCollection<Dog>> SearchDogsAsync(IEnumerable<string> searchTerms)
    {
        if (searchTerms == null || !searchTerms.Any())
        {
            return Task.FromResult<IReadOnlyCollection<Dog>>(new List<Dog>().AsReadOnly());
        }

        var terms = searchTerms.Where(t => !string.IsNullOrWhiteSpace(t)).ToList();

        var query = _pets.Values // Iteriamo solo sui valori
            .OfType<Dog>()
            .Where(dog => 
            {
                var description = dog.GetFullDescription();
                return terms.Any(term => description.Contains(term, StringComparison.OrdinalIgnoreCase));
            });

        return Task.FromResult<IReadOnlyCollection<Dog>>(query.ToList().AsReadOnly());
    }
}