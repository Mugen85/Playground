using Playground.Domain.Entities;
using Playground.Domain.Repositories;

namespace Playground.Infrastructure.Repositories;

/// <summary>
/// Implementazione in memoria del repository. 
/// Ottima per lo sviluppo iniziale, i test o per simulare un DB.
/// </summary>
public class InMemoryPetRepository : IPetRepository
{
    // Il nostro "database" privato. Essendo readonly, il riferimento non cambia,
    // ma la lista interna può essere modificata (tramite AddAsync).
    private readonly List<Pet> _pets;

    public InMemoryPetRepository()
    {
        // "Seeding" dei dati storici presi dal vecchio ContosoApp
        _pets = new List<Pet>
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
                45.00m) // 45.00 era il fallback nel vecchio codice se la stringa era vuota
        };
    }

    public Task<IReadOnlyCollection<Pet>> GetAllAsync()
    {
        // Task.FromResult crea un Task già completato. 
        // Simula il comportamento asincrono di un vero DB senza overhead.
        return Task.FromResult<IReadOnlyCollection<Pet>>(_pets.AsReadOnly());
    }

    public Task<Pet?> GetByIdAsync(string id)
    {
        // OrdinalIgnoreCase è sempre la scelta più performante e sicura per gli ID alfanumerici
        var pet = _pets.FirstOrDefault(p => p.Id.Equals(id, StringComparison.OrdinalIgnoreCase));
        return Task.FromResult(pet);
    }

    public Task AddAsync(Pet pet)
    {
        _pets.Add(pet);
        return Task.CompletedTask;
    }

    public Task<IReadOnlyCollection<Dog>> SearchDogsAsync(IEnumerable<string> searchTerms)
    {
        if (searchTerms == null || !searchTerms.Any())
        {
            return Task.FromResult<IReadOnlyCollection<Dog>>(new List<Dog>().AsReadOnly());
        }

        // Filtriamo eventuali stringhe vuote passate per errore
        var terms = searchTerms.Where(t => !string.IsNullOrWhiteSpace(t)).ToList();

        // LINQ in azione: niente più tripli cicli for/foreach annidati come nella vecchia app!
        var query = _pets
            .OfType<Dog>() // Estrae in automatico solo i cani (cast sicuro)
            .Where(dog => 
            {
                var description = dog.GetFullDescription();
                // Ritorna true se ALMENO UNO (Any) dei termini è contenuto nella descrizione
                return terms.Any(term => description.Contains(term, StringComparison.OrdinalIgnoreCase));
            });

        return Task.FromResult<IReadOnlyCollection<Dog>>(query.ToList().AsReadOnly());
    }
}