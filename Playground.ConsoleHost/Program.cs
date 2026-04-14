using Microsoft.Extensions.DependencyInjection;
using Playground.Application.Services;
using Playground.Domain.Repositories;
using Playground.Infrastructure.Repositories;

namespace Playground.ConsoleHost;

class Program
{
    static async Task Main(string[] args)
    {
        // 1. SETUP DELLA CENTRALINA (Dependency Injection)
        var services = new ServiceCollection();

        // Registriamo il repository come SINGLETON.
        // Perché? Dato che usa un dizionario in memoria, vogliamo che la stessa istanza 
        // "viva" per tutta la durata dell'app, altrimenti a ogni richiesta ripartiremmo da zero.
        // Se domani mettessimo un DB vero (es. SQL Server), lo cambieremmo in AddScoped o AddTransient.
        services.AddSingleton<IPetRepository, InMemoryPetRepository>();

        // Registriamo il servizio applicativo. AddTransient va benissimo: ogni volta che
        // ce ne serve uno, il sistema ce ne dà un'istanza nuova, iniettandogli dentro il repository.
        services.AddTransient<PetSearchService>();

        // Accendiamo il quadro elettrico: il provider è pronto a fornirci le classi
        var serviceProvider = services.BuildServiceProvider();

        // 2. RISOLUZIONE DEI SERVIZI
        // Chiediamo al magazziniere (serviceProvider) di darci i pezzi che ci servono per l'interfaccia
        var petRepository = serviceProvider.GetRequiredService<IPetRepository>();
        var searchService = serviceProvider.GetRequiredService<PetSearchService>();


        // 3. IL CRUSCOTTO (User Interface)
        string? menuSelection = "";
        do
        {
            Console.Clear();
            Console.WriteLine("=== Contoso PetFriends (Clean Architecture Edition) ===");
            Console.WriteLine(" 1. Elenco di tutti gli animali");
            Console.WriteLine(" 2. Cerca animali per caratteristica");
            Console.WriteLine("\nDigita il numero dell'opzione (o 'exit' per uscire)");

            menuSelection = Console.ReadLine()?.ToLower().Trim();

            switch (menuSelection)
            {
                case "1":
                    // Chiamata asincrona pulita al nostro Domain/Infrastructure
                    var allPets = await petRepository.GetAllAsync();
                    
                    Console.WriteLine("\n--- Elenco Animali ---");
                    foreach (var pet in allPets)
                    {
                        // Sfruttiamo il ToString() che avevi magistralmente overridato in Pet.cs
                        Console.WriteLine(pet.ToString());
                        Console.WriteLine($"   Descrizione: {pet.GetFullDescription()}");
                    }
                    
                    Console.WriteLine("\nPremi Invio per continuare...");
                    Console.ReadLine();
                    break;

                case "2":
                    Console.WriteLine("\nInserisci una o più caratteristiche separate da virgola (es. 'white, friendly, male'):");
                    var input = Console.ReadLine();
                    
                    // Deleghiamo tutta la fatica (split, validazione, ricerca) al layer Application
                    // Usiamo il nuovo metodo generico per i Pet!
                    var matchingPets = await searchService.SearchPetsByCharacteristicsAsync(input ?? "");
                    
                    if (!matchingPets.Any())
                    {
                        Console.WriteLine("\nNessun animale trovato con queste caratteristiche.");
                    }
                    else
                    {
                        Console.WriteLine($"\n--- Trovati {matchingPets.Count} animali ---");
                        foreach (var pet in matchingPets)
                        {
                            Console.WriteLine($"\n- {pet.Nickname} (ID: {pet.Id})");
                            Console.WriteLine($"  {pet.GetFullDescription()}");
                        }
                    }
                    
                    Console.WriteLine("\nPremi Invio per continuare...");
                    Console.ReadLine();
                    break;
            }

        } while (menuSelection != "exit");
    }
}