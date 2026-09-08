# 4. Onafhankelijk async werk concurrent uitvoeren

Bekijk deze code:

```csharp
var first = await LoadFirstAsync();
var second = await LoadSecondAsync();
```

Als beide operaties onafhankelijk zijn, gebeurt hier iets onnodig sequentieel.

De tweede operatie start pas nadat de eerste klaar is.

Je kunt beide operaties eerst starten.

```csharp
var firstTask = LoadFirstAsync();
var secondTask = LoadSecondAsync();

await Task.WhenAll(firstTask, secondTask);
```

Nu kunnen beide operaties tegelijk wachten op hun externe werk.

## Timing experiment

Twee operaties duren elk ongeveer 250 ms.

Sequentieel verwacht je ongeveer:

```text
250 ms + 250 ms = 500 ms
```

Concurrent verwacht je ongeveer:

```text
max(250 ms, 250 ms) = 250 ms
```

Timing is nooit exact. Gebruik timing daarom alleen om het grote verschil zichtbaar te maken.

## Opdracht

Implementeer `LoadBothAsync` in `ConcurrentAsyncExercises`.

De twee loaders zijn onafhankelijk en moeten gestart worden voordat je op hun resultaten wacht.

Gebruik geen `Task.Run`.

Maak de tests groen.

## Belangrijk

Concurrent betekent hier niet automatisch multithreaded.

Tijdens async I/O kunnen meerdere operaties bezig zijn zonder dat voor elke operatie een thread geblokkeerd blijft.
