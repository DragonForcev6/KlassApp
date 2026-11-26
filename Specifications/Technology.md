# Technology

Praktiska teknologier, mjukvaruarkitektur och teknisk metodik.

Status per val: **Beslutat** | **Förslag** (ej beslutat) | **Att verifiera**

## Översikt

| Del | Val | Status |
|-----|-----|--------|
| Backend | C# / .NET (LTS-version), ASP.NET Core Minimal API | Beslutat |
| Arkitektur | Tunn, imperativ, organiserad per funktion (se nedan) | Beslutat |
| Databas | MySQL 8 (LTS-version) | Beslutat |
| Dataåtkomst | Dapper + MySqlConnector, ingen EF Core (se nedan) | Beslutat |
| Migreringar | Versionerade SQL-skript, t.ex. med DbUp | Förslag |
| Realtid | SignalR | Förslag – behövs från lager 4 (kommunikation) |
| Kontrakt mot klient | OpenAPI, genererad TypeScript-klient | Förslag |
| Klient | Expo (React Native) för iOS, Android och webb | Förslag – beror på [Q7](Questions.md#q7-webb-och-mobil) |
| Autentisering | Etablerad lösning med OIDC-stöd (t.ex. ASP.NET Core Identity, OpenIddict, Keycloak) | Förslag – beror på [Q2](Questions.md#q2-användarnas-ålder-och-inloggning) |
| Filer | S3-kompatibel objektlagring | Förslag |
| Video/röst | LiveKit | Förslag – senare |
| Drift | EU-baserad hosting | Förslag |
| Test | xUnit, Testcontainers (riktig MySQL), `WebApplicationFactory` | Förslag |

## Arkitektur

**Beslutat:** En tunn, imperativ arkitektur med minimum av ceremoni.

Principer:

- **Organisera per funktion**, inte per teknisk typ. Varje funktionsområde (t.ex. `Classes`, `Users`, `Posts`) samlar sina endpoints, logik och datatyper på ett ställe.
- **Endpoints anropar logik direkt.** Minimal API-endpoints anropar vanliga metoder som läser och skriver med explicit SQL via Dapper. Ingen mediator, inga kommandobussar.
- **Inga abstraktioner i förväg.** Inga generiska repositories, inga interfaces med en enda implementation och inga separata lagerprojekt (Domain/Application/Infrastructure) förrän ett konkret behov uppstår.
- **Affärsregler i vanlig kod där de hör hemma.** Regler som rör roller, anonymitet, moderering och klassavgränsning ska vara samlade, läsbara och enhetstestbara.
- **Behörighet explicit i backend.** ASP.NET Core:s behörighetspolicies plus kontroller i koden.

Förslag på grundstruktur (ej beslutat):

```
src/
  Klassologi.Api/
    Program.cs
    Data/            anslutning, migreringsskript
    Features/
      Classes/
      Users/
      Posts/
    Auth/
tests/
  Klassologi.Api.Tests/
```

## Dataåtkomst: Dapper utan EF Core

**Beslutat (2026-11-26):** Dapper med MySqlConnector som databasdrivrutin. Ingen EF Core.

**Bakgrund:** Pomelo, den mognaste MySQL-providern för EF Core, saknade stöd för EF Core 10 (LTS). Den senaste versionen, 9.0.0, är låst till EF Core 9, vars support har upphört. Oracles provider stöder EF Core 10, men vi valde i stället att avstå från EF Core helt. Det ger tunnare dataåtkomst och tar bort beroendet av en ORM-provider. Paketversioner kontrollerade på NuGet 2026-11-26: Dapper 2.1.x och MySqlConnector 3.0.0 (med `net10.0`).

**Konsekvenser:**

- SQL skrivs explicit och syns i koden. Inget dolt beteende från en ORM.
- Migreringar hanteras med versionerade SQL-skript i stället för EF Core-migreringar.
- Det finns inga globala query-filter. Klassavgränsningen måste därför upprätthållas i koden (se nedan).
- Användarhantering kan inte bygga på ASP.NET Core Identitys färdiga EF Core-lagring. Det påverkar valet av autentiseringslösning ([Q2](Questions.md#q2-användarnas-ålder-och-inloggning)): antingen en extern identitetstjänst (t.ex. Keycloak) eller Identity med egen lagring via Dapper.

**Klassavgränsning utan query-filter:**

- Varje fråga mot klassbunden data tar klassens id som uttrycklig parameter, och id:t hämtas från den inloggade användarens verifierade medlemskap, aldrig enbart från klientens anrop.
- Medlemskapskontrollen samlas på ett ställe och anropas av varje klassbunden endpoint.
- Automatiska tester verifierar att en användare inte kan läsa eller ändra data i en klass som användaren inte tillhör.

## Databas: MySQL – säkerhetsåtgärder

MySQL saknar radnivåskydd (RLS) och har några standardbeteenden som påverkar säkerhet och dataintegritet. Följande gäller:

- **Teckenkodning:** `utf8mb4` överallt.
- **Collation:** uttrycklig collation på identitetskolumner; jämför på normaliserade fält (t.ex. `NormalizedEmail`, `NormalizedUserName`) så att skillnader i skiftläge och accenter inte ger dubbletter eller kollisioner.
- **Strikt läge:** `STRICT_TRANS_TABLES` påslaget, så att för långa värden ger fel i stället för att avkortas.
- **Tid:** `DATETIME(6)` och alltid UTC.
- **Klassavgränsning:** upprätthålls i koden och verifieras med automatiska tester (se Dataåtkomst ovan).
- **Migreringar:** små migreringar och backup före driftsättning, eftersom MySQL inte kan rulla tillbaka schemaändringar i en transaktion.

## Att verifiera

- **SAML-stöd** om inloggning via skolans federation krävs ([Q2](Questions.md#q2-användarnas-ålder-och-inloggning)).
