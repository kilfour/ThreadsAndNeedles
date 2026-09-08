# 9. Waarom async belangrijk is in ASP.NET Core

Je hebt al code geschreven zoals:

```csharp
public async Task<IResult> GetBooks(...)
{
    var books = await query.Execute(...);
    return Results.Ok(books);
}
```

En in EF Core:

```csharp
var books = await dbContext.Books
    .ToListAsync(cancellationToken);
```

Waarom is dat nuttig op een server?

## Conceptueel verloop

```text
HTTP request komt binnen
    ↓
ASP.NET Core start de request code
    ↓
De handler start een database query
    ↓
await ToListAsync(...)
    ↓
Tijdens het wachten hoeft deze request geen thread te blokkeren
    ↓
Database antwoordt
    ↓
De continuation wordt ingepland
    ↓
De response wordt afgewerkt
```

Het belangrijke schaalbaarheidsvoordeel is niet dat 1 request plots sneller wordt.

Het voordeel is dat threads niet nutteloos bezet blijven terwijl requests op I/O wachten.

Daardoor kan dezelfde server veel wachtende requests efficiënter behandelen.

## Cancellation in Minimal APIs

ASP.NET Core kan een `CancellationToken` aan een endpoint geven.

```csharp
app.MapGet("/books", async (
    AppDbContext dbContext,
    CancellationToken cancellationToken) =>
{
    var books = await dbContext.Books
        .ToListAsync(cancellationToken);

    return Results.Ok(books);
});
```

Als de request wordt afgebroken, kan die token cancellation signaleren.

Geef hem door naar database, HTTP en andere APIs die cancellation ondersteunen.

## Opdracht

`RequestAggregator` simuleert een endpoint dat twee externe services nodig heeft.

Implementeer de methode zodat:

* beide onafhankelijke calls concurrent starten
* de cancellation token aan beide calls wordt doorgegeven
* de methode wacht tot beide resultaten beschikbaar zijn
* exceptions niet verborgen worden

Maak de tests groen.
