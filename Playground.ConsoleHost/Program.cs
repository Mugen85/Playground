using Microsoft.Extensions.DependencyInjection;
using Playground.Application.Services;
using Playground.Domain.Repositories;
using Playground.Infrastructure.Repositories;
using Playground.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Playground.Domain.Entities;

namespace Playground.ConsoleHost;

class Program
{
    static async Task Main(string[] args)
    {
        var services = new ServiceCollection();

        services.AddDbContext<PetDbContext>(options =>
            options.UseSqlite("Data Source=pets.db"));

        services.AddScoped<IPetRepository, SqlitePetRepository>();
        services.AddTransient<PetSearchService>();

        var serviceProvider = services.BuildServiceProvider();

        // --- FIX: INIZIALIZZAZIONE DATABASE ---
        // Creiamo uno scope temporaneo per chiedere il DbContext e applicare le migrazioni all'avvio
        using (var scope = serviceProvider.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<PetDbContext>();
            Console.WriteLine("Verifica e aggiornamento del database in corso...");
            await dbContext.Database.MigrateAsync(); // Applica InitialCreate automaticamente!
        }
        // --------------------------------------

        var petRepository = serviceProvider.GetRequiredService<IPetRepository>();
        var searchService = serviceProvider.GetRequiredService<PetSearchService>();

        string? menuSelection = "";
        do
        {
            Console.Clear();
            Console.WriteLine("=== Contoso PetFriends (Clean Architecture Edition) ===");
            Console.WriteLine(" 1. Elenco di tutti gli animali");
            Console.WriteLine(" 2. Cerca animali per caratteristica");
            Console.WriteLine(" 3. Aggiungi un nuovo animale");
            Console.WriteLine("\nDigita il numero dell'opzione (o 'exit' per uscire)");

            menuSelection = Console.ReadLine()?.ToLower().Trim();

            switch (menuSelection)
            {
                case "1":
                    var allPets = await petRepository.GetAllAsync();
                    Console.WriteLine("\n--- Elenco Animali ---");
                    if (!allPets.Any()) Console.WriteLine("Il rifugio è vuoto! Usa l'opzione 3 per aggiungere ospiti.");
                    
                    foreach (var p in allPets)
                    {
                        // Formattiamo l'output su misura per la UI senza stampare l'ID (Guid)
                        string tipoAnimale = p is Dog ? "CANE" : "GATTO";
                        string badgeUrgente = p.IsUrgent ? " [🚨 URGENTE]" : "";
                        
                        Console.WriteLine($"[{tipoAnimale}] {p.Nickname} — Donazione: {p.SuggestedDonation} €{badgeUrgente}");
                        Console.WriteLine($"   Descrizione: {p.GetFullDescription()}");
                    }
                    Console.WriteLine("\nPremi Invio per continuare...");
                    Console.ReadLine();
                    break;

                case "2":
                    Console.WriteLine("\nInserisci una o più caratteristiche separate da virgola (es. 'white, friendly, male'):");
                    var input = Console.ReadLine();
                    var matchingPets = await searchService.SearchPetsByCharacteristicsAsync(input ?? "");

                    if (!matchingPets.Any())
                    {
                        Console.WriteLine("\nNessun animale trovato.");
                    }
                    else
                    {
                        Console.WriteLine($"\n--- Trovati {matchingPets.Count} animali ---");
                        foreach (var p in matchingPets)
                        {
                            // Anche qui nascondiamo l'ID e mostriamo il tipo
                            string tipoAnimale = p is Dog ? "CANE" : "GATTO";
                            Console.WriteLine($"\n- {p.Nickname} [{tipoAnimale}]");
                            Console.WriteLine($"  {p.GetFullDescription()}");
                        }
                    }
                    Console.WriteLine("\nPremi Invio per continuare...");
                    Console.ReadLine();
                    break;

                case "3":
                    Console.WriteLine("\n--- Aggiungi un nuovo Animale ---");
                    Console.Write("Che animale è? (C = Cane, G = Gatto): ");
                    var petType = Console.ReadLine()?.Trim().ToUpper();

                    if (petType != "C" && petType != "G")
                    {
                        Console.WriteLine("\nScelta non valida! Operazione annullata.");
                        Console.WriteLine("Premi Invio per continuare...");
                        Console.ReadLine();
                        break;
                    }

                    string tipoNome = petType == "C" ? "Cane" : "Gatto";
                    Console.WriteLine($"\nInserisci i dati per il nuovo {tipoNome}:");

                    Console.Write("Nome: ");
                    var name = Console.ReadLine() ?? "Sconosciuto";
                    
                    Console.Write("Razza/Aspetto fisico (es. Persiano, Labrador): ");
                    var physical = Console.ReadLine() ?? "Meticcio";
                    
                    Console.Write("Personalità (es. Giocherellone, Dormiglione): ");
                    var personality = Console.ReadLine() ?? "Tranquillo";

                    Console.Write("È un caso urgente? (S/N): ");
                    bool isUrgent = Console.ReadLine()?.Trim().ToUpper() == "S";

                    // Usiamo il polimorfismo: dichiariamo la base e istanziamo la classe derivata
                    Pet newPet;
                    string newId = Guid.NewGuid().ToString();

                    if (petType == "C")
                    {
                        newPet = new Dog(
                            id: newId,
                            nickname: name,
                            age: 1, // Età fissa per brevità nella UI
                            physicalDescription: physical,
                            personalityDescription: personality,
                            suggestedDonation: 20m
                        ) { IsUrgent = isUrgent };
                    }
                    else
                    {
                        newPet = new Cat(
                            id: newId,
                            nickname: name,
                            age: 1, 
                            physicalDescription: physical,
                            personalityDescription: personality,
                            suggestedDonation: 15m // I gatti magari hanno una donazione base diversa
                        ) { IsUrgent = isUrgent };
                    }

                    // Il repository accetta la classe base "Pet". 
                    // EF Core si occuperà di salvare il discriminatore corretto!
                    await petRepository.AddAsync(newPet);
                    Console.WriteLine($"\n{name} ({tipoNome}) aggiunto con successo al database!");
                    
                    Console.WriteLine("\nPremi Invio per continuare...");
                    Console.ReadLine();
                    break;
            }

        } while (menuSelection != "exit");
    }
}