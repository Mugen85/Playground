using Microsoft.EntityFrameworkCore;
using Playground.Domain.Entities;
using Playground.Domain.Repositories; // CORREZIONE: Namespace dell'interfaccia aggiornato
using Playground.Infrastructure.Data; 

namespace Playground.Infrastructure.Repositories;

// SqlitePetRepository firma il contratto IPetRepository completo.
public class SqlitePetRepository : IPetRepository
{
    private readonly PetDbContext _context;

    // Tramite Dependency Injection, chiediamo la mappa del DB (il DbContext)
    public SqlitePetRepository(PetDbContext context)
    {
        _context = context;
    }

    // 1. Recupera tutti
    public async Task<IReadOnlyCollection<Pet>> GetAllAsync()
    {
        return await _context.Pets.ToListAsync();
    }

    // 2. Cerca tramite ID
    public async Task<Pet?> GetByIdAsync(string id)
    {
        // FindAsync è ottimizzato per cercare tramite la Chiave Primaria
        return await _context.Pets.FindAsync(id);
    }

    // 3. Aggiunge un nuovo Pet
    public async Task AddAsync(Pet pet)
    {
        await _context.Pets.AddAsync(pet);
        // NOTA: AddAsync mette l'entità "in attesa". 
        // Per salvarla fisicamente su disco, dobbiamo chiamare SaveChangesAsync()
        await _context.SaveChangesAsync(); 
    }

    // 4. Motore di ricerca per caratteristiche
    public async Task<IReadOnlyCollection<Pet>> SearchPetsAsync(IEnumerable<string> searchTerms)
    {
        var termsList = searchTerms.ToList();
        
        if (!termsList.Any()) 
            return new List<Pet>();

        // Siccome stiamo facendo un test locale (DB piccolo), 
        // peschiamo prima tutto e filtriamo in memoria con LINQ-to-Objects.
        // In un database vero con milioni di righe, dovremmo costruire la query 
        // in modo dinamico prima di chiamare ToListAsync().
        var allPets = await _context.Pets.ToListAsync();

        var result = allPets.Where(pet => 
        {
            var fullDesc = $"{pet.Species} {pet.Nickname} {pet.PhysicalDescription} {pet.PersonalityDescription}".ToLower();
            return termsList.Any(term => fullDesc.Contains(term.ToLower()));
        }).ToList();

        return result;
    }
}