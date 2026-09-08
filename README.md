# ThreadsAndNeedles

## Werkwijze

Werk de modules in volgorde af. Elke module bevat:

* een korte uitleg in `README.md`
* startercode met een of meer `TODO`-commentaren
* gerichte tests
* eventueel denk- of reflectievragen

Tests die bij nog niet geïmplementeerde code horen, zijn gemarkeerd met:

```csharp
[Fact(Skip = "Not Implemented")]
```

Werk per module als volgt:

1. verwijder `Skip = "Not Implemented"` bij de relevante tests;
2. voer de tests uit en controleer dat ze met de startercode rood zijn;
3. implementeer de opdracht totdat de tests groen zijn;
4. laat afgewerkte tests ingeschakeld.

Voer tijdens een module alleen de relevante tests uit, bijvoorbeeld:

```bash
dotnet test --filter FullyQualifiedName~TaskExperimentsTests
```

Een volledig groene suite aan het begin betekent alleen dat onafgewerkte tests zijn overgeslagen. Voer na de eindopdracht de volledige suite uit en controleer dat er nergens meer `Skip = "Not Implemented"` staat:

```bash
dotnet test
git grep 'Skip = "Not Implemented"' -- ThreadsAndNeedles
```

Het `git grep`-commando hoort bij de eindcontrole geen resultaten meer te tonen.

Alleen `UnsafeCounterExperimentExpectedTotal` blijft normaal overgeslagen. Dat is een bewust niet-deterministisch experiment uit module 6.

Voor voorbereiding, observatiepunten en een nakijkrubric: zie [TEACHING_NOTES.md](TEACHING_NOTES.md).

## Modules

1. Processes en threads
2. `Task` is niet hetzelfde als `Thread`
3. `async` en `await`
4. Onafhankelijk async werk concurrent uitvoeren
5. CPU-bound en I/O-bound werk
6. Race conditions en gedeelde state
7. Cancellation
8. Veelvoorkomende async valkuilen
9. Async in ASP.NET Core
10. Eindopdracht: Dashboard Aggregator

## Belangrijk bij concurrency-tests

Tests kunnen aantonen dat een implementatie fout is, maar bewijzen niet in hun eentje dat code thread-safe is. Bespreek daarom ook de gekozen synchronisatie en redeneer over mogelijke interleavings.
