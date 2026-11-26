# Öppna frågor

Frågor som måste besvaras innan vi kan specificera eller designa de berörda delarna. När en fråga är besvarad dokumenteras beslutet i relevant specifikation och frågan markeras som besvarad här.

Status: **Öppen** | **Besvarad**

## Q1. Anonyma frågor – anonyma för vem?

**Status:** Öppen
**Berör:** Kommunikation, Integritet och behörighet, Social miljö

Ska anonyma frågor vara anonyma för alla, inklusive lärare och administratörer, eller enbart för andra elever?

**Varför det spelar roll:** Påverkar om avsändaren lagras alls, och därmed möjligheten att moderera och hantera kränkningar. Fullständig anonymitet kan stå i konflikt med kravet på god ton.

## Q2. Användarnas ålder och inloggning

**Status:** Öppen
**Berör:** Användare, profiler och roller, Integritet

Kan användarna vara minderåriga? Ska inloggning ske via skolans befintliga identitetslösning (SSO) eller via egna konton?

**Varför det spelar roll:** Påverkar GDPR-hantering, samtycke, vilka personuppgifter vi får lagra och val av autentiseringslösning.

## Q3. Omfattning – en klass, flera klasser eller flera skolor?

**Status:** Öppen
**Berör:** Datamodell, Användare och roller

Ska en installation hantera en klass, flera klasser eller flera skolor?

**Varför det spelar roll:** Avgör om datamodellen behöver stödja flera separata organisationer (multi-tenancy) och hur behörigheter avgränsas.

## Q4. Närvarokontroll – hur verifieras platsen?

**Status:** Öppen
**Berör:** Närvaro, Integritet

Hur ska vi kontrollera att eleven befinner sig på skolan vid incheckning (t.ex. GPS, skolans nätverk, QR-kod)?

**Varför det spelar roll:** Platsdata är integritetskänslig. Valet påverkar både teknisk lösning och vilka personuppgifter som behandlas.

## Q5. Roller och behörigheter

**Status:** Öppen
**Berör:** Användare, profiler och roller

Vilka behörigheter har en elevrepresentant utöver en vanlig elev? Vem skapar klasser och tilldelar roller? Finns en administratörsroll?

**Varför det spelar roll:** Grunden för behörighetsmodellen.

## Q6. Externa parter

**Status:** Öppen
**Berör:** Extern kommunikation och samverkan, Integritet och behörighet

Ska företag och lärosäten ha egna konton i applikationen? Vad ska de i så fall få se och göra?

**Varför det spelar roll:** Externa användare innebär en separat behörighetsgräns och påverkar integritetsskyddet för elever.

## Q7. Webb och mobil

**Status:** Öppen
**Berör:** Tillgänglighet och plattformar, Teknik

Räcker en responsiv webbapplikation eller PWA initialt, eller krävs en mobilapp från start? Mobilnotiser och snabbåtgärder från låsskärmen kan kräva en mobilapp.

**Varför det spelar roll:** Påverkar teknikval och arkitektur tidigt.

## Q8. Prioritering och omfattning av första versionen

**Status:** Öppen
**Berör:** Samtliga funktionsområden

Vilka funktioner ska ingå i första versionen?

**Förslag (ej beslutat):** Konton och roller, en klass, anslagstavla, textkanaler och privat chatt, grupper och gemensam kalender.

**Varför det spelar roll:** Krävs för att prioritera och detaljera use cases.
