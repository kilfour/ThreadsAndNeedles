# 10. Eindopdracht: Dashboard Aggregator

Je bouwt de kern van een endpoint dat informatie uit meerdere bronnen samenbrengt.

De starter bevat drie fake externe services.

```text
Profile service       ongeveer 250 ms
Orders service        ongeveer 250 ms
Recommendations       ongeveer 250 ms
```

Een eerste naïeve implementatie die elke call meteen `await` doet, duurt ongeveer 750 ms.

De calls zijn onafhankelijk.

## Doel

Implementeer `DashboardService.LoadAsync`.

De methode moet:

* de drie externe calls zo snel mogelijk starten
* ze concurrent laten wachten
* geen `Task.Run` gebruiken voor de fake I/O calls
* cancellation doorgeven
* exceptions laten doorstromen
* een thread-safe teller bijhouden van volledig geslaagde dashboard loads
* alleen een succesvolle load tellen wanneer alle data beschikbaar is

## Verwacht gedrag

Bij succes krijg je:

```csharp
new Dashboard(
    profile,
    orders,
    recommendations);
```

Bij cancellation moet cancellation zichtbaar blijven voor de caller.

Als 1 dependency faalt, mag de methode geen gedeeltelijk dashboard als succes tellen.

## Testen

Maak alle tests in `DashboardServiceTests` groen.

Voer daarna de volledige suite uit.

```bash
dotnet test
```

## Reflectie

Beantwoord voor jezelf:

* Waar in deze opdracht is concurrency aanwezig?
* Waar is echte multithreading relevant?
* Waarom gebruiken we geen `Task.Run` voor de fake externe calls?
* Welke state moest thread-safe zijn?
* Waarom geven we cancellation door in plaats van zelf een boolean `cancelled` te maken?
* Wat zou er in een echte ASP.NET Core applicatie gebeuren als de client de request afbreekt?
