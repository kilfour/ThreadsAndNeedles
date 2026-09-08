# 6. Race conditions en gedeelde state

Concurrent werk wordt gevaarlijk wanneer meerdere uitvoeringspaden dezelfde mutable state aanpassen.

Bekijk:

```csharp
counter++;
```

Dit lijkt 1 operatie, maar conceptueel gebeurt er meer:

```text
Lees counter.
Bereken counter + 1.
Schrijf het nieuwe resultaat terug.
```

Als twee threads dit tegelijk doen, kunnen updates verloren gaan.

Dat is een race condition.

## Experiment

`UnsafeCounter` verhoogt een gedeelde integer vanuit veel parallel werk.

De test `UnsafeCounterExperimentExpectedTotal` heeft bewust een andere `Skip`-reden dan de opdrachten. Verwijder die `Skip` alleen tijdelijk en run de test meerdere keren. Wanneer updates verloren gaan, wordt de test rood en zie je het kleinere resultaat. Een toevallige groene run bewijst niet dat de implementatie veilig is. Zet de demonstratie daarna opnieuw op `Skip`, zodat de volledige suite niet onbetrouwbaar wordt.

Een race condition is timing afhankelijk. Dat betekent dat buggy code soms toch het juiste resultaat kan geven.

## Interlocked

Voor eenvoudige atomaire operaties biedt .NET `Interlocked`.

```csharp
Interlocked.Increment(ref counter);
```

## lock

Voor een grotere critical section kun je `lock` gebruiken.

```csharp
lock (gate)
{
    // gedeelde state veilig aanpassen
}
```

Hou een lock zo klein mogelijk. Doe geen trage I/O terwijl je een gewone `lock` vasthoudt.

## Opdracht

Maak `SafeCounter` thread-safe.

Maak daarna `ThreadSafeCounterNeverLosesUpdates` groen.

Gebruik `Interlocked.Increment` voor de update. Gebruik voor `Value` een geschikte atomaire of volatile read, zodat de property ook veilig gelezen kan worden terwijl andere threads schrijven.

Implementeer ook `SafeLedger.Add` zodat balance en operation count samen consistent aangepast worden. Een snapshot mag nooit een nieuwe balance met een oude operation count combineren, of omgekeerd.

Gebruik daarom in zowel `Add` als `Snapshot` dezelfde private lock. De lock beschermt de invariant tussen de twee velden; alleen de schrijfmethode locken is niet voldoende wanneer snapshots tegelijk met updates kunnen gebeuren.

Verwijder bij de drie opdracht-tests `Skip = "Not Implemented"`, controleer dat de startercode rood is en maak de tests groen. Laat deze drie tests daarna ingeschakeld. Alleen de niet-deterministische `UnsafeCounterExperimentExpectedTotal` blijft overgeslagen.
