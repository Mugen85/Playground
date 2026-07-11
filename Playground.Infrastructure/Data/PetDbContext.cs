using Microsoft.EntityFrameworkCore;
using Playground.Domain.Entities;
using Microsoft.EntityFrameworkCore.Design;

namespace Playground.Infrastructure.Data;

// PetDbContext eredita da DbContext, che è la classe base di Entity Framework
public class PetDbContext : DbContext
{
    // DbSet rappresenta la tabella nel database. 
    // Mettendo "Pet", EF Core capisce che deve mappare anche le sottoclassi Dog e Cat.
    public DbSet<Pet> Pets => Set<Pet>();

    // Il costruttore accetta delle "opzioni" (qui passeremo la stringa di connessione a SQLite dal Program.cs)
    public PetDbContext(DbContextOptions<PetDbContext> options) : base(options)
    {
    }

    // Qui dentro "mettiamo a punto" le impostazioni specifiche delle colonne
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configurazioni specifiche per l'entità Pet
        modelBuilder.Entity<Pet>(entity =>
        {
            // Diciamo esplicitamente che 'Id' è la Chiave Primaria (Primary Key)
            entity.HasKey(e => e.Id);

            // Configuriamo esplicitamente il "Discriminator" per l'ereditarietà (TPH).
            // Verrà creata una colonna 'PetType' che conterrà la stringa "Dog" o "Cat"
            entity.HasDiscriminator<string>("PetType")
                .HasValue<Dog>("Dog")
                .HasValue<Cat>("Cat");

            // I database sono pignoli sui decimali. Gli diciamo di usare 18 cifre totali di cui 2 decimali.
            // Questo evita un warning noioso di EF Core durante la compilazione.
            entity.Property(e => e.SuggestedDonation)
                .HasColumnType("decimal(18,2)");
        });
    }
}

// NUOVO CODICE: Il "Motorino di avviamento" per la CLI di Entity Framework
// Questa classe viene ignorata quando l'app gira normalmente. 
// Viene chiamata SOLO quando usi i comandi "dotnet ef" dal terminale.
// Abbiamo inserito lo using Microsoft.EntityFrameworkCore.Design; all'inizio del file per poter implementare IDesignTimeDbContextFactory<PetDbContext>
public class PetDbContextFactory : IDesignTimeDbContextFactory<PetDbContext>
{
    public PetDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<PetDbContext>();
        
        // Diciamo esplicitamente alla CLI di usare SQLite e lo stesso file
        optionsBuilder.UseSqlite("Data Source=pets.db");

        return new PetDbContext(optionsBuilder.Options);
    }
}