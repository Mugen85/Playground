using Playground.Domain.Entities;
using Playground.Domain.Repositories;

namespace Playground.Application.Services;

public class PetSearchService
{
    private readonly IPetRepository _petRepository;

    // Dependency Injection pura: il servizio richiede il manuale (l'interfaccia), 
    // non gli interessa quale officina (classe concreta) fa il lavoro.
    public PetSearchService(IPetRepository petRepository)
    {
        _petRepository = petRepository;
    }

    /// <summary>
    /// Orchestra la ricerca dei cani partendo dall'input grezzo dell'utente.
    /// </summary>
    public async Task<IReadOnlyCollection<Dog>> SearchDogsByCharacteristicsAsync(string rawInput)
    {
        if (string.IsNullOrWhiteSpace(rawInput))
        {
            return new List<Dog>().AsReadOnly();
        }

        // Pulizia dei condotti: gestiamo l'input dell'utente qui nell'Application layer.
        // StringSplitOptions fa il lavoro sporco di Trim e rimozione dei vuoti in un colpo solo.
        var searchTerms = rawInput
            .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Select(t => t.ToLowerInvariant())
            .ToList();

        // Passiamo la palla al repository
        return await _petRepository.SearchDogsAsync(searchTerms);
    }
}