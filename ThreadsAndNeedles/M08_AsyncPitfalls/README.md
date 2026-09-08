# 8. Async valkuilen

## .Result en .Wait()

Deze code verandert async wachten in blokkerend wachten.

```csharp
var result = LoadAsync().Result;
```

```csharp
LoadAsync().Wait();
```

In gewone async code gebruik je liever:

```csharp
var result = await LoadAsync();
```

Blokkeren verspilt een thread en kan in sommige omgevingen tot deadlocks leiden.

## async void

Gebruik `async void` normaal alleen voor event handlers.

Voor gewone async methodes gebruik je:

```csharp
Task
Task<T>
```

Dan kan de caller wachten, exceptions observeren en composition gebruiken.

## Een Task vergeten te awaiten

```csharp
SaveAsync();
return Results.Ok();
```

De methode kan antwoorden voordat `SaveAsync` klaar is. Exceptions kunnen ook op een onverwachte plaats terechtkomen.

## Per ongeluk sequentieel

```csharp
var a = await LoadAAsync();
var b = await LoadBAsync();
```

Dit is correct als B afhankelijk is van A.

Als ze onafhankelijk zijn, kan het onnodig traag zijn.

## Fire and forget

Dit is verdacht:

```csharp
_ = SendEmailAsync();
```

Wie observeert de fout als de operatie faalt?

Wat gebeurt er wanneer de request eindigt of de applicatie stopt?

Voor achtergrondwerk gebruik je beter een expliciet achtergrondmechanisme met een duidelijke lifecycle.

## Opdracht

Open `AsyncPitfallsExercises`.

Voorspel vóór je iets wijzigt voor elke methode:

* of de methode haar caller kan blokkeren
* of de methode te vroeg kan voltooien
* of onafhankelijk werk sequentieel uitgevoerd wordt

Herstel de drie methodes zonder `.Result`, `.Wait()` of onnodig fire-and-forget gedrag.

Verwijder `Skip = "Not Implemented"` bij de drie tests. Controleer eerst dat elke valkuil door minstens één rode test zichtbaar wordt en maak de tests daarna groen. Laat ze vervolgens ingeschakeld.

Leg na elke aanpassing uit welk probleem je hebt verwijderd. Alleen een groen resultaat is bij concurrency-code niet voldoende bewijs dat de redenering klopt.
