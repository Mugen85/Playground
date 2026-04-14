using Playground.Domain.Entities;
using Playground.Domain.Repositories;
using System.Collections.Concurrent;
using System.Text.RegularExpressions;

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

        // Seeding dei dati storici
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
        // TryAdd è thread-safe. Se l'ID esiste già, lo ignora.
        _pets.TryAdd(pet.Id, pet);
        return Task.CompletedTask;
    }

    // ECCO IL METODO AGGIORNATO CHE RISOLVE L'ERRORE DI BUILD
    public Task<IReadOnlyCollection<Pet>> SearchPetsAsync(IEnumerable<string> searchTerms)
    {
        if (searchTerms == null || !searchTerms.Any())
        {
            return Task.FromResult<IReadOnlyCollection<Pet>>(new List<Pet>().AsReadOnly());
        }

        var terms = searchTerms.Where(t => !string.IsNullOrWhiteSpace(t)).ToList();

        var query = _pets.Values // Iteriamo su TUTTI gli animali (niente più OfType<Dog>)
            .Where(pet => 
            {
                var description = pet.GetFullDescription();
                
                // Regex in azione: \b indica il "confine" di una parola. 
                // Regex.Escape ci protegge se l'utente inserisce caratteri speciali strani.
                // In questo modo "male" matcherá solo "male" e non "female".
                return terms.Any(term => 
                    Regex.IsMatch(description, $@"\b{Regex.Escape(term)}\b", RegexOptions.IgnoreCase));
            });

        return Task.FromResult<IReadOnlyCollection<Pet>>(query.ToList().AsReadOnly());
    }
}