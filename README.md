# 🧪 Playground — C# / .NET

> Esercizi pratici, snippet e appunti di studio su C# e .NET.  
> Ogni cartella corrisponde a un argomento del piano di studio mensile.

---

## Struttura

```
Playground/
├── Playground.App/          # Esercizi eseguibili (un file per argomento)
├── Playground.ContosoApp/   # Progetto guidato MS Learn — Contoso PetFriends
├── Playground.Tests/        # Test xUnit su ogni concetto studiato
└── README.md
```

---

## Come eseguire

**Prerequisiti:** .NET 10 SDK, Git

```bash
git clone https://github.com/Mugen85/Playground.git
cd Playground

# Esegui gli esercizi
dotnet run --project Playground.App

# Esegui il progetto guidato Contoso
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
| Progetto guidato Contoso PetFriends — array, loop, TryParse, ricerca | `Playground.ContosoApp/Program.cs` | — |

*(aggiornato man mano che procede lo studio)*

---

## Obiettivo

Questo repo accompagna un piano di studio strutturato su C# / .NET con l'obiettivo di costruire basi solide prima di affrontare il mercato del lavoro.  
Ogni commit corrisponde a un concetto studiato e compreso — non solo copiato.

---

## CI/CD

Questo repo usa **GitHub Actions** per eseguire build e test automatici ad ogni push su `main`.

Il workflow `.github/workflows/ci.yml` esegue in sequenza: restore delle dipendenze, build in Release e . Se un test fallisce, la pipeline si blocca e il badge diventa rosso.

---

## Stack

![CI](https://github.com/Mugen85/Playground/actions/workflows/ci.yml/badge.svg)
![.NET](https://img.shields.io/badge/.NET-10.0-512BD4?style=flat&logo=dotnet)
![C#](https://img.shields.io/badge/C%23-Latest-239120?style=flat&logo=c-sharp)
![xUnit](https://img.shields.io/badge/xUnit-Tests-blue?style=flat)
