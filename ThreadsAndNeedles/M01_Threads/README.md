# 1. Processes en threads

Een draaiende applicatie is een process.

Een process bevat onder andere geheugen en 1 of meer threads.

Een thread is een uitvoeringspad. Code wordt uiteindelijk door een thread uitgevoerd.

## Experiment

Open `ThreadExperiments.cs`.

Bekijk deze property:

```csharp
Environment.CurrentManagedThreadId
```

Die geeft het id van de managed thread waarop de code op dat moment draait.

Run de test `CurrentThreadIdReturnsAValidThreadId`.

## Zelf een thread starten

.NET laat toe om rechtstreeks een thread te maken.

```csharp
var thread = new Thread(() =>
{
    Console.WriteLine(Environment.CurrentManagedThreadId);
});

thread.Start();
thread.Join();
```

Dit kan nuttig zijn in gespecialiseerde code, maar gewone applicatiecode maakt zelden zelf threads aan.

.NET heeft een thread pool met herbruikbare worker threads. Veel hogere niveau APIs bouwen daarop verder.

## Opdracht

Implementeer `RunOnDedicatedThread`.

De methode moet:

* een nieuwe `Thread` maken
* de meegegeven `Action` op die thread uitvoeren
* wachten tot de thread klaar is
* het managed thread id van die thread teruggeven

Maak daarna deze test groen:

```text
RunOnDedicatedThreadUsesAnotherThread
```

## Denkvragen

* Waarom zou voor elke kleine actie een nieuwe thread maken duur zijn?
* Wat moet een applicatie bijhouden voor elke thread?
* Waarom zou een pool van herbruikbare threads nuttig zijn?
