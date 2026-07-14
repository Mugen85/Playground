# **🧪 Playground — C\# / .NET**

Il mio banco di lavoro personale: esercizi pratici, snippet e refactoring architetturali su C\# e .NET.

Qui smonto codice procedurale e lo rimonto seguendo i principi SOLID, in preparazione a sfide di livello enterprise.

## **🏗️ Case Study: Refactoring verso la Clean Architecture**

Il progetto originale ContosoApp (fornito da MS Learn) era un classico monolite procedurale: tutto il codice, i dati e l'interfaccia utente sbrodolati in un unico file Program.cs. L'abbiamo preso, smontato pezzo per pezzo e reingegnerizzato in una vera **Clean Architecture** modulare, passando da finti dati in memoria a un database relazionale reale:

* **Domain (Il blocco motore):** Il cuore del sistema. Entità ricche e validate (Pet astratta, Dog, Cat con proprietà init e costruttori sicuri Fail-Fast) e i Contratti (Interfacce come IPetRepository) che dettano le regole, ignorando totalmente la tecnologia esterna.  
* **Infrastructure (La trasmissione):** L'implementazione pratica dell'accesso ai dati. Abbiamo integrato **Entity Framework Core 10** mappando il dominio polimorfico su un database **SQLite** (pets.db) sfruttando il pattern **Table-Per-Hierarchy (TPH)**. Il SqlitePetRepository isola le query LINQ dal resto dell'app.  
* **Application (La centralina):** I servizi (es. PetSearchService) che orchestrano i casi d'uso. Puliscono l'input utente e coordinano il Dominio, rimanendo completamente agnostici rispetto al database o all'interfaccia.  
* **ConsoleHost (Il cruscotto):** Il livello di Presentazione e *Composition Root*. Gestisce l'interfaccia utente, applica le migrazioni del DB all'avvio e, tramite il container di **Dependency Injection** di .NET, inietta e cabla a runtime i vari servizi (AddScoped, AddTransient).

**Nota:** La versione originale (Playground.ContosoApp) rimane nella solution esclusivamente come riferimento storico per apprezzare il "Prima e Dopo".

## **Struttura della Solution**

Playground/    
├── Playground.App/            \# Esercizi base eseguibili (un file per argomento)    
├── Playground.Application/    \# Casi d'uso e orchestrazione (Clean Architecture)    
├── Playground.ContosoApp/     \# Il monolite di partenza (Archivio)    
├── Playground.Domain/         \# Entità del dominio e interfacce    
├── Playground.Infrastructure/ \# Implementazioni tecniche (EF Core, SqlitePetRepository)    
├── Playground.ConsoleHost/    \# UI, Migrazioni EF Core e Dependency Injection    
├── Playground.Tests/          \# Suite Test: Unitari e Integration (su SQLite in-memory)    
└── README.md

## **Come eseguire**

**Prerequisiti:** .NET 10 SDK, Git

⚠️ **Posizione consigliata:** clona la repo in una cartella dedicata ai progetti (es. C:\\PROGETTI CSHARP\\) ed evita Desktop, Documenti o cartelle sincronizzate con OneDrive. Windows applica policy di sicurezza su queste posizioni che possono bloccare l'esecuzione delle DLL compilate.

git clone https://github.com/Mugen85/Playground.git    
cd Playground

\# Esegui l'app refattorizzata (crea il DB in automatico)  
dotnet run \--project Playground.ConsoleHost

\# Esegui gli esercizi base sulle stringhe    
dotnet run \--project Playground.App

\# Esegui la suite completa di test (26/26 Passed)  
dotnet test

## **Argomenti coperti**

| Argomento | File | Test |
| :---- | :---- | :---- |
| IndexOf e Substring — trovare e estrarre sottostringhe | StringExercises.cs | ✅ |
| LastIndexOf — ultima occorrenza di un carattere | StringExercises.cs | ✅ |
| while loop \+ Substring — estrarre tutte le occorrenze | StringExercises.cs | ✅ |
| IndexOfAny — cercare simboli multipli contemporaneamente | StringExercises.cs | ✅ |
| Remove — rimuovere caratteri per posizione fissa o dinamica | StringExercises.cs | ✅ |
| Replace — sostituire tutte le occorrenze di una sottostringa | StringExercises.cs | ✅ |
| Challenge HTML — estrarre, rimuovere e sostituire da stringa HTML | StringExercises.cs | ✅ |
| **Domain-Driven Design** — Entità astratte, Polimorfismo, Invarianza (Fail-Fast) | Domain/Entities/Pet.cs | ✅ Unit Tests |
| **EF Core: Table-Per-Hierarchy (TPH)** — Mappare derivati su singola tabella | Infrastructure/Data/PetDbContext.cs | ✅ |
| **EF Core: Incapsulamento** — Costruttori privati/protetti per materializzazione DB | Domain/Entities/ | ✅ |
| **Integration Testing Anti-Fragile** — SQLite in-memory (DataSource=:memory:) vs Mocking | Tests/Infrastructure/ | ✅ Integration Tests |
| **Dependency Injection** — AddDbContext, AddScoped (Repo), AddTransient (Service) | ConsoleHost/Program.cs | — |

*(aggiornato man mano che procede lo studio)*

## **Roadmap refactoring ContosoApp → Clean Architecture**

| Step | Layer | Stato |
| :---- | :---- | :---- |
| Modellazione Entities base e polimorfiche | Playground.Domain | ✅ |
| Astrazione con Interfaccia IPetRepository | Playground.Domain | ✅ |
| Implementazione Service (ricerca e validazione input) | Playground.Application | ✅ |
| Setup EF Core e SqlitePetRepository | Playground.Infrastructure | ✅ |
| Generazione DB fisico via Migrations | Playground.ConsoleHost | ✅ |
| Cablaggio Dependency Injection e UI | Playground.ConsoleHost | ✅ |
| Refactoring Test per usare DB SQLite In-Memory | Playground.Tests | ✅ |

## **CI/CD**

Questo repo usa **GitHub Actions** per eseguire build e test automatici ad ogni push su main.

Il workflow .github/workflows/ci.yml esegue in sequenza: restore delle dipendenze, build in Release e test. Se un test fallisce, la pipeline si blocca e il badge diventa rosso.

## **Stack**

![CI](https://github.com/Mugen85/Playground/actions/workflows/ci.yml/badge.svg)  ![.NET](https://img.shields.io/badge/.NET-10.0-512BD4?style=flat&logo=dotnet) ![C#](https://img.shields.io/badge/C%23-Latest-239120?style=flat&logo=c-sharp) ![EF Core](https://img.shields.io/badge/EF%20Core-10.0-5C2D91?style=flat&logo=nuget) ![SQLite](https://img.shields.io/badge/SQLite-DB-003B57?style=flat&logo=sqlite) ![xUnit](https://img.shields.io/badge/xUnit-Tests-blue?style=flat)