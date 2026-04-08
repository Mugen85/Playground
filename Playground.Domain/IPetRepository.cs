using Playground.Domain.Entities;

namespace Playground.Domain.Repositories;

/// <summary>
/// Contratto per l'accesso ai dati dei Pet.
/// Appartiene al layer Domain: definisce cosa serve al business, 
/// astraendo completamente la tecnologia di storage.
/// </summary>
public interface IPetRepository
{
    /// <summary>
    /// Recupera tutti gli animali presenti nel rifugio.
    /// </summary>
    Task<IReadOnlyCollection<Pet>> GetAllAsync();

    /// <summary>
    /// Recupera un animale tramite il suo ID univoco.
    /// </summary>
    Task<Pet?> GetByIdAsync(string id);

    /// <summary>
    /// Aggiunge un nuovo animale al sistema.
    /// </summary>
    Task AddAsync(Pet pet);

    /// <summary>
    /// Cerca cani le cui descrizioni corrispondono ad almeno uno dei termini di ricerca.
    /// Passiamo l'onere della ricerca all'infrastruttura (che in futuro potrebbe
    /// tradurla in una query SQL LIKE o in una ricerca full-text).
    /// </summary>
    Task<IReadOnlyCollection<Dog>> SearchDogsAsync(IEnumerable<string> searchTerms);
}