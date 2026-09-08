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

* `action` herhaald uitvoeren
* tussen iteraties asynchroon wachten
* de meegegeven cancellation token respecteren
* `OperationCanceledException` laten doorstromen

Maak de tests groen.
