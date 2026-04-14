# **🧪 Playground — C\# / .NET**

Il mio banco di lavoro personale: esercizi pratici, snippet e refactoring architetturali su C\# e .NET.

Qui smonto codice procedurale e lo rimonto seguendo i principi SOLID, in preparazione a sfide di livello enterprise.

## **🏗️ Case Study: Refactoring verso la Clean Architecture**

Il progetto originale ContosoApp (fornito da MS Learn) era un classico monolite procedurale: tutto il codice, i dati e l'interfaccia utente sbrodolati in un unico file Program.cs. L'abbiamo preso, smontato pezzo per pezzo e reingegnerizzato in una vera **Clean Architecture** modulare:

* **Domain (Il blocco motore):** Il cuore del sistema. Entità ricche e validate (Pet, Dog, Cat con costruttori sicuri) e i Contratti (Interfacce come IPetRepository) che dettano le regole, ignorando totalmente la tecnologia esterna.  
* **Infrastructure (La trasmissione):** L'implementazione pratica dell'accesso ai dati. Abbiamo sostituito l'array multidimensionale originale con un InMemoryPetRepository basato su ConcurrentDictionary per garantire thread-safety e tempi di lookup ![][image1], implementando ricerche avanzate tramite **Regex** (es. *Word Boundaries*).  
* **Application (La centralina):** I servizi (es. PetSearchService) che orchestrano i casi d'uso. Puliscono l'input utente e coordinano il Dominio, rimanendo completamente agnostici rispetto al database o all'interfaccia.  
* **ConsoleHost (Il cruscotto):** Il livello di Presentazione e *Composition Root*. Gestisce l'interfaccia utente e, tramite il container di **Dependency Injection** di .NET, inietta e cabla a runtime i vari servizi e repository.

**Nota:** La versione originale (Playground.ContosoApp) rimane nella solution esclusivamente come riferimento storico per apprezzare il "Prima e Dopo".

## **Struttura della Solution**

Playground/  
├── Playground.App/            \# Esercizi base eseguibili (un file per argomento)  
├── Playground.Application/    \# Casi d'uso e orchestrazione (Clean Architecture)  
├── Playground.ContosoApp/     \# Il monolite di partenza (Archivio)  
├── Playground.Domain/         \# Entità del dominio e interfacce  
├── Playground.Infrastructure/ \# Implementazioni tecniche (Repository, DB)  
├── Playground.ConsoleHost/    \# UI e Composition Root (Dependency Injection)  
├── Playground.Tests/          \# Test unitari (xUnit)  
└── README.md

## **Come eseguire**

**Prerequisiti:** .NET 10 SDK, Git

⚠️ **Posizione consigliata:** clona la repo in una cartella dedicata ai progetti (es. C:\\PROGETTI CSHARP\\) ed evita Desktop, Documenti o cartelle sincronizzate con OneDrive. Windows applica policy di sicurezza su queste posizioni che possono bloccare l'esecuzione delle DLL compilate.

git clone \[https://github.com/Mugen85/Playground.git\](https://github.com/Mugen85/Playground.git)  
cd Playground

\# Esegui l'app refattorizzata con Clean Architecture  
dotnet run \--project Playground.ConsoleHost

\# Esegui gli esercizi base sulle stringhe  
dotnet run \--project Playground.App

\# Esegui i test unitari  
dotnet test

## **Argomenti coperti**

| Argomento | File | Test |
| :---- | :---- | :---- |
| IndexOf e Substring — trovare e estrarre sottostringhe | StringExercises.cs | StringExercisesTests.cs |
| LastIndexOf — ultima occorrenza di un carattere | StringExercises.cs | StringExercisesTests.cs |
| while loop \+ Substring — estrarre tutte le occorrenze | StringExercises.cs | StringExercisesTests.cs |
| IndexOfAny — cercare simboli multipli contemporaneamente | StringExercises.cs | StringExercisesTests.cs |
| Remove — rimuovere caratteri per posizione fissa o dinamica | StringExercises.cs | StringExercisesTests.cs |
| Replace — sostituire tutte le occorrenze di una sottostringa | StringExercises.cs | StringExercisesTests.cs |
| Challenge HTML — estrarre, rimuovere e sostituire da stringa HTML | StringExercises.cs | StringExercisesTests.cs |
| Domain entities — Pet (abstract), Dog, Cat con guardie e DDD | Playground.Domain/Entities/ | *(in arrivo)* |
| Repository Pattern (Contratto) — IPetRepository con asincronia (Task) e IReadOnlyCollection | Playground.Domain/Repositories/ | *(in arrivo)* |
| Data Structures: ConcurrentDictionary per lookup ![][image1] e Thread-Safety | InMemoryPetRepository.cs | *(in arrivo)* |
| RegEx: Utilizzo di \\b (Word Boundaries) per ricerche esatte e insensibili alle maiuscole | InMemoryPetRepository.cs | *(in arrivo)* |
| Dependency Injection: Registrazione Singleton e Transient, risoluzione tramite ServiceProvider | ConsoleHost/Program.cs | — |

*(aggiornato man mano che procede lo studio)*

## **Roadmap refactoring ContosoApp → Clean Architecture**

| Step | Layer | Stato |
| :---- | :---- | :---- |
| Entities: Pet, Dog, Cat | Playground.Domain | ✅ |
| Interfaccia IPetRepository | Playground.Domain | ✅ |
| InMemoryPetRepository | Playground.Infrastructure | ✅ |
| PetSearchService | Playground.Application | ✅ |
| Rewire console con dependency injection | Playground.ConsoleHost | ✅ |

## **CI/CD**

Questo repo usa **GitHub Actions** per eseguire build e test automatici ad ogni push su main.

Il workflow .github/workflows/ci.yml esegue in sequenza: restore delle dipendenze, build in Release e test. Se un test fallisce, la pipeline si blocca e il badge diventa rosso.

---

## Stack

![CI](https://github.com/Mugen85/Playground/actions/workflows/ci.yml/badge.svg)
![.NET](https://img.shields.io/badge/.NET-10.0-512BD4?style=flat&logo=dotnet)
![C#](https://img.shields.io/badge/C%23-Latest-239120?style=flat&logo=c-sharp)
![xUnit](https://img.shields.io/badge/xUnit-Tests-blue?style=flat)
