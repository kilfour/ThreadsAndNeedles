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

Run de test meerdere keren.

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

Implementeer ook `SafeLedger.Add` zodat balance en operation count samen consistent aangepast worden.

Gebruik hiervoor `lock`.
