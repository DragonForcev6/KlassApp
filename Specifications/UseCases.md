# Use Cases

Use cases uttryckta, detaljerade och prioriterade utifrån [Functionality](Functionality.md) och [Application description](Application-description.md).

## Bruttolista

**Status:** Förslag, ej beslutat. Prioriteringen hänger ihop med [Q8](Questions.md#q8-prioritering-och-omfattning-av-första-versionen).

### Prioriteringsprincip

Listan är ordnad i lager efter beroenden, enligt principen för en minsta användbara kärnapplikation (MVP): varje lager förutsätter de föregående och ska ge ett fungerande helt system när det är klart.

1. **Plattform** – det måste finnas en klass att vara medlem i.
2. **Användare** – någon måste kunna logga in och höra till klassen.
3. **Poster** – det måste finnas något att dela: information på anslagstavlan.

Lager 1–3 utgör den föreslagna MVP:n: en klass där användare loggar in och där information publiceras och läses på ett ställe. Det är den minsta helhet som uppfyller applikationens kärnsyfte, att samla klassens information och förkorta kommunikationsvägarna. Senare lager bygger vidare på samma grund.

Aktörerna är preliminära tills rollerna är fastställda ([Q5](Questions.md#q5-roller-och-behörigheter)).

Kolumnen *Frågor* anger öppna frågor som måste besvaras innan use caset kan detaljeras.

### Lager 1 – Plattform (MVP)

| ID | Use case | Primär aktör | Mål | Beror på | Frågor |
|----|----------|--------------|-----|----------|--------|
| UC-01 | Skapa klass | Administratör/lärare | Upprätta en klass som användare kan höra till | – | Q3, Q5 |
| UC-02 | Bjuda in användare till klass | Administratör/lärare | Ge elever och lärare tillgång till klassen | UC-01 | Q2, Q3 |
| UC-03 | Tilldela och ändra roll | Administratör/lärare | Ge användare rätt behörigheter (elev, lärare, elevrepresentant) | UC-02 | Q5 |

### Lager 2 – Användare (MVP)

| ID | Use case | Primär aktör | Mål | Beror på | Frågor |
|----|----------|--------------|-----|----------|--------|
| UC-04 | Registrera konto / aktivera inbjudan | Elev, lärare | Få ett konto kopplat till klassen | UC-02 | Q2 |
| UC-05 | Logga in och logga ut | Alla användare | Få säker åtkomst till applikationen | UC-04 | Q2 |
| UC-06 | Hantera egen profil | Alla användare | Presentera sig för klassen | UC-05 | Q2 |
| UC-07 | Visa klasslista och profiler | Alla användare | Se vilka som ingår i klassen | UC-06 | – |

### Lager 3 – Poster / anslagstavla (MVP)

| ID | Use case | Primär aktör | Mål | Beror på | Frågor |
|----|----------|--------------|-----|----------|--------|
| UC-08 | Publicera inlägg på anslagstavlan | Lärare (ev. elevrepresentant) | Nå hela klassen med information | UC-05 | Q5, Q10 |
| UC-09 | Läsa anslagstavlan | Alla användare | Hitta aktuell information på ett ställe | UC-08 | – |
| UC-10 | Redigera och ta bort eget inlägg | Inläggets författare | Hålla informationen korrekt | UC-08 | – |
| UC-11 | Kommentera och reagera på inlägg | Alla användare | Ställa följdfrågor och visa att man tagit del | UC-09 | Q10 |
| UC-12 | Moderera inlägg och kommentarer | Lärare/administratör | Upprätthålla god ton och gemensamma regler | UC-11 | Q5 |

---

*Gräns för föreslagen MVP. Lagren nedan är ordnade efter beroende och värde men inte inbördes beslutade.*

---

### Lager 4 – Kommunikation

| ID | Use case | Primär aktör | Mål | Beror på | Frågor |
|----|----------|--------------|-----|----------|--------|
| UC-13 | Skriva i gemensam textkanal | Alla användare | Diskutera med klassen | Lager 2 | – |
| UC-14 | Föra privat chatt | Alla användare | Kommunicera en-till-en | Lager 2 | – |
| UC-15 | Skapa och dela egen kanal | Alla användare | Samla en diskussion kring ett ämne | UC-13 | Q5 |
| UC-16 | Skicka meddelande till lärare | Elev | Nå läraren direkt | UC-14 | – |
| UC-17 | Kommunicera med elevrepresentanter | Elev | Lämna frågor och synpunkter som kan föras vidare | UC-13 | Q5 |
| UC-18 | Ställa anonym fråga | Elev | Fråga utan att profilen visas | UC-13 | Q1 |

### Lager 5 – Grupper och samarbete

| ID | Use case | Primär aktör | Mål | Beror på | Frågor |
|----|----------|--------------|-----|----------|--------|
| UC-19 | Skapa studie- eller projektgrupp | Elev | Samarbeta i en mindre grupp | Lager 2 | – |
| UC-20 | Bjuda in elever och lärare till grupp | Gruppmedlem | Sätta ihop gruppen | UC-19 | – |
| UC-21 | Kommunicera i grupp | Gruppmedlem | Diskutera inom gruppen | UC-19, UC-13 | – |

### Lager 6 – Material och fildelning

| ID | Use case | Primär aktör | Mål | Beror på | Frågor |
|----|----------|--------------|-----|----------|--------|
| UC-22 | Dela dokument, kursmaterial och länkar | Lärare, elev | Samla material på ett ställe | Lager 2 | Q5 |
| UC-23 | Kategorisera material efter ämne | Lärare | Göra materialet lätt att hitta | UC-22 | – |
| UC-24 | Söka och hämta material | Alla användare | Hitta rätt material snabbt | UC-22 | – |

### Lager 7 – Kalender och planering

| ID | Use case | Primär aktör | Mål | Beror på | Frågor |
|----|----------|--------------|-----|----------|--------|
| UC-25 | Visa och hantera klasskalender | Lärare (skriva), alla (läsa) | Ha gemensam överblick över händelser | Lager 2 | Q5 |
| UC-26 | Visa och hantera gruppkalender | Gruppmedlem | Planera gruppens arbete | UC-19, UC-25 | – |
| UC-27 | Planera gemensam aktivitet | Alla användare | Samordna en aktivitet | UC-25 | – |

### Lager 8 – Notiser

| ID | Use case | Primär aktör | Mål | Beror på | Frågor |
|----|----------|--------------|-----|----------|--------|
| UC-28 | Få notis om nytt innehåll och ändringar | Alla användare | Inte missa viktig information | Lager 3–7 | Q7 |
| UC-29 | Anpassa notiser | Alla användare | Styra vad man blir störd av | UC-28 | – |
| UC-30 | Utföra snabbåtgärd från notis | Alla användare | Svara eller reagera utan att öppna appen | UC-28 | Q7 |

### Senare (ej ordnade)

| ID | Use case | Primär aktör | Frågor |
|----|----------|--------------|--------|
| UC-31 | Visa status och tillgänglighet (inkl. lärares) | Alla användare | – |
| UC-32 | Röstkanaler, röst- och videosamtal | Alla användare | Q7 |
| UC-33 | Skärmdelning | Alla användare | Q7 |
| UC-34 | Lämna in arbete i grupp | Gruppmedlem | – |
| UC-35 | Dokumentera gruppens arbete | Gruppmedlem | – |
| UC-36 | Synkronisera grupp med GitHub | Gruppmedlem | – |
| UC-37 | Boka rum för individuellt arbete eller grupparbete | Elev | – |
| UC-38 | Checka in närvaro | Elev | Q4 |
| UC-39 | Använda personlig AI-assistent | Alla användare | – |
| UC-40 | Använda gemensam AI-assistent i grupp | Gruppmedlem | – |
| UC-41 | Samverka med företag och lärosäten | Lärare, elev, extern part | Q6 |
| UC-42 | Kategorisera elever | Lärare | Q9 |
| UC-43 | Lägga till tillägg och anpassa funktionalitet | Alla användare | – |
