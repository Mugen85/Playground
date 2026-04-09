# 🧪 Playground — C# / .NET

> Esercizi pratici, snippet e appunti di studio su C# e .NET.  
> Ogni cartella corrisponde a un argomento del piano di studio mensile.

---

## Struttura

```
Playground/  
├── Playground.App/            \# Esercizi eseguibili (un file per argomento)  
├── Playground.Application/    \# Casi d'uso e orchestrazione (es. PetSearchService)  
├── Playground.ContosoApp/     \# Progetto guidato MS Learn — Contoso PetFriends (versione originale, archivio)  
├── Playground.Domain/         \# Entità del dominio — Pet, Dog, Cat (refactoring di ContosoApp)  
├── Playground.Infrastructure/ \# Implementazione dei contratti (es. InMemoryPetRepository)  
├── Playground.Tests/          \# Test xUnit su ogni concetto studiato  
└── README.md
```

> **Nota:** il refactoring di `ContosoApp` verso una clean architecture è in corso.  
> `Playground.ContosoApp` rimane nella solution come riferimento storico — rappresenta il punto di partenza prima dell'introduzione dei layer.

---

## Come eseguire

**Prerequisiti:** .NET 10 SDK, Git

> ⚠️ **Posizione consigliata:** clona la repo in una cartella dedicata ai progetti (es. `C:\PROGETTI CSHARP\`) ed evita Desktop, Documenti o cartelle sincronizzate con OneDrive. Windows applica policy di sicurezza su queste posizioni che possono bloccare l'esecuzione delle DLL compilate.

```bash
git clone https://github.com/Mugen85/Playground.git
cd Playground

# Esegui gli esercizi
dotnet run --project Playground.App

# Esegui il progetto guidato Contoso (versione originale)
dotnet run --project Playground.ContosoApp

# Esegui i test
dotnet test
```

---

## Argomenti coperti

| Argomento | File | Test |
|-----------|------|------|
| `IndexOf` e `Substring` — trovare e estrarre sottostringhe | `StringExercises.cs` | `StringExercisesTests.cs` |
| `LastIndexOf` — ultima occorrenza di un carattere | `StringExercises.cs` | `StringExercisesTests.cs` |
| `while` loop + `Substring` — estrarre tutte le occorrenze | `StringExercises.cs` | `StringExercisesTests.cs` |
| `IndexOfAny` — cercare simboli multipli contemporaneamente | `StringExercises.cs` | `StringExercisesTests.cs` |
| `Remove` — rimuovere caratteri per posizione fissa o dinamica | `StringExercises.cs` | `StringExercisesTests.cs` |
| `Replace` — sostituire tutte le occorrenze di una sottostringa | `StringExercises.cs` | `StringExercisesTests.cs` |
| Challenge HTML — estrarre, rimuovere e sostituire da stringa HTML | `StringExercises.cs` | `StringExercisesTests.cs` |
| Progetto guidato Contoso PetFriends — array, TryParse, valuta `:C2` | `Playground.ContosoApp/Program.cs` | — |
| Contoso PetFriends — ricerca cani per caratteristica con `while` + `Contains` | `Playground.ContosoApp/Program.cs` | — |
| Contoso PetFriends — ricerca multi-termine con `Split`, `Sort` e `foreach` annidato | `Playground.ContosoApp/Program.cs` | — |
| Contoso PetFriends — animazione terminale con `\r`, `Thread.Sleep` e conto alla rovescia | `Playground.ContosoApp/Program.cs` | — |
| Domain entities — Pet (abstract), Dog, Cat con guardie e nullable | Playground.Domain/Entities/ | *(in arrivo)* |
| Repository Pattern (Contratto) — IPetRepository con asincronia (Task) e IReadOnlyCollection | Playground.Domain/Repositories/ | *(in arrivo)* |
| Data Structures: ConcurrentDictionary per lookup ![][image1] e Thread-Safety | InMemoryPetRepository.cs | *(in arrivo)* |

*(aggiornato man mano che procede lo studio)*

---

## Roadmap refactoring ContosoApp → Clean Architecture

Il progetto `ContosoApp` viene progressivamente smontato e ricostruito seguendo i principi della clean architecture. Ogni step è un commit separato.

| Step | Layer | Stato |
|------|-------|-------|
| Entities: Pet, Dog, Cat | Playground.Domain | ✅ |
| Interfaccia IPetRepository | Playground.Domain | ✅ |
| InMemoryPetRepository | Playground.Infrastructure | ✅ |
| PetSearchService | Playground.Application | ✅ |
| Rewire console con dependency injection | Playground.ConsoleHost | ⏳ |

---

## CI/CD

Questo repo usa **GitHub Actions** per eseguire build e test automatici ad ogni push su `main`.

Il workflow `.github/workflows/ci.yml` esegue in sequenza: restore delle dipendenze, build in Release e test. Se un test fallisce, la pipeline si blocca e il badge diventa rosso.

---

## Stack

![CI](https://github.com/Mugen85/Playground/actions/workflows/ci.yml/badge.svg)
![.NET](https://img.shields.io/badge/.NET-10.0-512BD4?style=flat&logo=dotnet)
![C#](https://img.shields.io/badge/C%23-Latest-239120?style=flat&logo=c-sharp)
![xUnit](https://img.shields.io/badge/xUnit-Tests-blue?style=flat)
