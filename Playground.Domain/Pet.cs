namespace Playground.Domain.Entities;

/// <summary>
/// Entità base per tutti gli animali del rifugio.
/// Classe astratta perché un "Pet" generico non esiste:
/// esiste sempre un Dog o un Cat concreto.
/// </summary>
public abstract class Pet
{
    public string Id { get; }
    public string Nickname { get; }
    public int? Age { get; }                    // null = età sconosciuta
    public string PhysicalDescription { get; }
    public string PersonalityDescription { get; }
    public decimal SuggestedDonation { get; }

    // Proprietà astratta: ogni sottoclasse dichiara la propria specie.
    // È una proprietà e non un campo perché in futuro potrebbe
    // derivare da logica (es. localizzazione).
    public abstract string Species { get; }

    protected Pet(
        string id,
        string nickname,
        int? age,
        string physicalDescription,
        string personalityDescription,
        decimal suggestedDonation)
    {
        // Guardie nel costruttore: il dominio non può esistere in uno stato
        // invalido. Meglio scoprirlo qui che con una NullReferenceException
        // a runtime in un punto lontano.
        ArgumentException.ThrowIfNullOrWhiteSpace(id, nameof(id));
        ArgumentException.ThrowIfNullOrWhiteSpace(nickname, nameof(nickname));

        if (suggestedDonation < 0)
            throw new ArgumentOutOfRangeException(nameof(suggestedDonation),
                "La donazione suggerita non può essere negativa.");

        Id = id;
        Nickname = nickname;
        Age = age;
        PhysicalDescription = physicalDescription ?? string.Empty;
        PersonalityDescription = personalityDescription ?? string.Empty;
        SuggestedDonation = suggestedDonation;
    }

    /// <summary>
    /// Restituisce una descrizione completa dell'animale (fisica + carattere).
    /// Usata dalla logica di ricerca per fare Contains su un'unica stringa.
    /// </summary>
    public string GetFullDescription() =>
        $"{PhysicalDescription} {PersonalityDescription}";

    public override string ToString() =>
        $"[{Species.ToUpper()}] {Nickname} (ID: {Id}) — Donazione: {SuggestedDonation:C2}";
}