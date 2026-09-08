# 3. async en await

`async` betekent niet "voer deze methode op een andere thread uit".

`await` betekent ook niet "blokkeer hier totdat het klaar is".

Bij een onvoltooide task kan een async methode haar uitvoering tijdelijk onderbreken en controle teruggeven aan haar caller.

Wanneer de task voltooid is, kan de rest van de methode verder uitgevoerd worden.

## Een eenvoudig voorbeeld

```csharp
public async Task<string> LoadAsync()
{
    var value = await ReadValueAsync();
    return value.ToUpperInvariant();
}
```

Je kunt dit conceptueel lezen als:

```text
Start ReadValueAsync.
Als het resultaat er al is, ga verder.
Anders kan deze methode tijdelijk stoppen.
Wanneer het resultaat beschikbaar is, voer de rest uit.
```

De compiler bouwt hiervoor intern een state machine.

Je hoeft die state machine niet zelf te schrijven, maar het helpt om te beseffen dat een async methode opgedeeld kan worden rond `await` punten.

## Opdracht

Implementeer `LoadAndTransformAsync` in `AsyncAwaitExercises`.

De methode krijgt:

```csharp
Func<Task<string>> loader
```

Ze moet:

* de loader 1 keer oproepen
* asynchroon wachten op het resultaat
* het resultaat trimmen
* het resultaat omzetten naar uppercase

Maak `AsyncAwaitExercisesTests` groen.

## Extra experiment

Zet tijdelijk logging voor en na een `await`.

Log ook:

```csharp
Environment.CurrentManagedThreadId
```

Ga er niet van uit dat hetzelfde thread id voor en na `await` gegarandeerd is.

In ASP.NET Core code is het belangrijker dat je geen thread blokkeert dan dat een continuation op exact dezelfde thread verdergaat.
