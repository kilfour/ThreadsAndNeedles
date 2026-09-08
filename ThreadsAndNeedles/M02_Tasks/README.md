# 2. Task is niet hetzelfde als Thread

Een `Task` stelt werk voor dat bezig is, later kan eindigen of al klaar kan zijn.

Een `Task<T>` stelt ook een resultaat voor dat later beschikbaar kan worden.

```csharp
Task<int> answer = GetAnswerAsync();
```

Dat betekent niet automatisch dat er een aparte thread bestaat voor die task.

## Task.Delay

Bekijk:

```csharp
await Task.Delay(1000);
```

Een verkeerde voorstelling is:

```text
Maak een thread.
Laat die thread 1 seconde slapen.
```

`Task.Delay` gebruikt een timer. Tijdens het wachten hoeft geen thread 1 seconde geblokkeerd te blijven.

Vergelijk dit met:

```csharp
Thread.Sleep(1000);
```

`Thread.Sleep` blokkeert de thread waarop de code draait.

## Opdracht

Implementeer in `TaskExperiments`:

```csharp
public static async Task<string> CompleteLater(
    string value,
    TimeSpan delay)
```

De methode wacht asynchroon en geeft daarna `value` terug.

Gebruik `Task.Delay`, niet `Thread.Sleep`.

Maak de tests in `TaskExperimentsTests` groen.

## Denkvragen

* Kan een `Task` al voltooid zijn wanneer je hem ontvangt?
* Kan een `Task` bestaan zonder dat er op dat moment een thread voor werkt?
* Waarom is `Task` een nuttiger abstractie dan overal zelf threads beheren?
