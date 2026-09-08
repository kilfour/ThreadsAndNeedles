# 7. Cancellation

Async werk kan lang duren.

Soms is het resultaat niet meer nodig.

Voorbeelden:

* gebruiker navigeert weg
* HTTP client verbreekt de verbinding
* applicatie sluit af
* timeout is bereikt

.NET gebruikt hiervoor meestal `CancellationToken`.

## Cancellation doorgeven

```csharp
public async Task<Book?> LoadBook(
    int id,
    CancellationToken cancellationToken)
{
    return await dbContext.Books
        .FirstOrDefaultAsync(
            book => book.Id == id,
            cancellationToken);
}
```

Een methode die zelf andere cancellable APIs aanroept, geeft dezelfde token meestal verder door.

## Cancellation is cooperative

Een `CancellationToken` stopt code niet met geweld.

De code of gebruikte API moet cancellation observeren.

Bijvoorbeeld:

```csharp
cancellationToken.ThrowIfCancellationRequested();
```

of:

```csharp
await Task.Delay(delay, cancellationToken);
```

## Opdracht

Implementeer `RepeatUntilCancelledAsync`.

De methode moet:

* vóór de eerste iteratie controleren of cancellation al aangevraagd is
* `action` herhaald uitvoeren
* de cancellation token aan `action` doorgeven
* tussen iteraties asynchroon wachten
* de meegegeven cancellation token respecteren
* `OperationCanceledException` laten doorstromen

De action heeft daarom deze vorm:

```csharp
Func<CancellationToken, Task> action
```

Cancellation beëindigt de methode dus met een cancelled task; de methode keert niet succesvol terug alsof het werk voltooid was.

Verwijder `Skip = "Not Implemented"` bij beide tests. Controleer eerst dat de startercode rood is en maak de tests daarna groen. Laat ze vervolgens ingeschakeld.
