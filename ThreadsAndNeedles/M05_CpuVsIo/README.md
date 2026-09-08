# 5. CPU bound en I/O bound

Niet alle trage operaties zijn hetzelfde.

## I/O bound

Voorbeelden:

* HTTP request
* database query
* bestand lezen
* wachten op een netwerkresponse

De applicatie wacht vooral op iets buiten de CPU.

Async APIs zijn hier bijzonder nuttig omdat de thread niet geblokkeerd hoeft te blijven tijdens het wachten.

Voorbeelden:

```csharp
await httpClient.GetAsync(url);
await dbContext.Books.ToListAsync();
await File.ReadAllTextAsync(path);
```

## CPU bound

Voorbeelden:

* grote berekening
* beeldverwerking
* compressie
* complexe parsing

Hier is de CPU werkelijk bezig.

`async` maakt CPU werk niet sneller.

`Task.Run` kan CPU werk naar een thread-pool thread verplaatsen.

```csharp
var result = await Task.Run(() => Calculate());
```

Dat kan nuttig zijn in een desktop UI waar je de UI thread vrij wil houden.

In ASP.NET Core is zomaar CPU werk in `Task.Run` stoppen meestal geen schaalbaarheidswinst. Het werk gebruikt nog altijd een thread-pool thread en de server moet dezelfde hoeveelheid CPU werk uitvoeren.

## Opdracht

Implementeer:

```csharp
public static Task<(long Result, int ThreadId)> CalculateOnThreadPoolAsync(int iterations)
```

Gebruik `Task.Run` om `CpuWork.Calculate` uit te voeren. Geef naast het resultaat ook het managed thread id terug waarop de berekening uitgevoerd werd.

Maak de tests groen.

## Denkvragen

Classificeer deze operaties als vooral I/O bound of CPU bound:

* `HttpClient.GetAsync`
* `DbContext.SaveChangesAsync`
* SHA256 berekenen voor een groot bestand nadat het al in memory staat
* JSON van 500 MB parsen vanuit een string
* wachten op `Task.Delay`
