# 10. Eindopdracht: Dashboard Aggregator

Je bouwt de kern van een endpoint dat informatie uit meerdere bronnen samenbrengt.

Stel dat drie externe services in een echte applicatie elk ongeveer 250 ms nodig hebben:

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

Start alle calls voordat je op hun gezamenlijke voltooiing wacht. Daardoor worden ook calls na een snel gefaalde task nog gestart en worden de gestarte tasks als één geheel geobserveerd.

## Testen

Verwijder `Skip = "Not Implemented"` bij alle tests in `DashboardServiceTests`. Controleer eerst dat de starterimplementatie rood is en maak alle tests daarna groen. Laat ze vervolgens ingeschakeld.

De tests gebruiken een gedeeld vrijgavesignaal. Ze meten dus niet hoe snel je computer is, maar controleren rechtstreeks dat alle dependencies gestart zijn voordat een ervan kan voltooien.

De stresstest voor `SuccessfulLoads` kan een verloren update zichtbaar maken, maar geen enkele stresstest bewijst thread-safety. Controleer daarom ook in de code dat de increment atomair gebeurt en dat de property een geschikte thread-safe read gebruikt.

Voer daarna de volledige suite uit. Er mag alleen nog één test overgeslagen zijn: het bewuste `UnsafeCounterExperimentExpectedTotal`-experiment uit module 6.

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
