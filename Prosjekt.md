# Systemspesifikasjon for fordypningsprosjekt
**Versjon 1.0** | **06. April - 2025**

## Innholdsfortegnelse
1. [Introduksjon](#1-introduksjon)
    - 1.1 [Formål](#11-formål)
    - 1.2 [Systemomfang](#12-systemomfang)
    - 1.3 [Teknologi](#13-teknologi)
2. [Systemarkitektur](#2-systemarkitektur)
    - 2.1 [Overordnet arkitektur](#21-overordnet-arkitektur)
    - 2.2 [Komponenter](#22-komponenter)
3. [Gjennomføring og prinsipper](#3-gjennomføring-og-prinsipper)
   - 3.1 [Prinsipper](#31-prinsipper)
   - 3.2 [Skalerbarhet og brukervennlighet](#32-skalerbarhet-og-brukervennlighet)
   - 3.3 [Optimalisering](#33-optimalisering)
4. [Fremtidige utvidelser](#4-fremtidige-utvidelser)
   - 4.1 [Utvidet innhold](#41-utvidet-innhold)
   - 4.2 [Utvidet platform tilgjengelighet](#42-utvidet-platform-tilgjengelighet)
   - 4.3 [Instillinger](#43-instillinger)
5. [Utfordringer og risikoer](#5-utfordringer-og-risikoer)
   - 5.1 [Tekniske utfordringer](#51-tekniske-utfordringer)
6. [Misc](#6-misc)
   - 6.1 [CD/CD](#61-cicd)


## 1. Introduksjon

### 1.1 Formål
Oppgaven er designet for å være et videospill av sjangeren Roguelike. Det er flere måter å lage et slikt spill på, men våres går ut på å være et 2D top-down combat basert Roguelike. Brukeren vil kontrollere en spiller som kan bevege seg rundt gjennom dungeons og slåss mot fiender. 

### 1.2 Systemomfang
Oppgaven inkluderer:
- Applikasjon for Windows og Mac
- Variert combat system med upgrades
- Randomly generated dungeons for ny å spennende opplevelse hver gang
- Varierte fiender
- Bruk av forskjellige open source assets for visuelt og lyd design for å gjøre spillet mere spennende

### 1.3 Teknologi
- **Godot**: En cross-platform open-source game engine 
- C# koding 
- GDScript koding
- Diverse packages, både for koding og design

## 2. Systemarkitektur

### 2.1 Overordnet arkitektur
Systemarkitekturen er hovedsakelig en stor service laget med Godot. De forskjellige tjenestene i spillet er implementert som scripts

### 2.2 Komponenter

**Komponentbeskrivelser:**
1. **Player**
    - Karakter som styres av spilleren
    - WASD/Piltaster for bevegelse, og venste + høyre museklikk for combat
    - Bruk av forskjellig pixel-art sprites, sydd sammen for å lage forskjellige animasjoner som spiller når man står stille, beveger seg, eller angriper osv.
    - Combat med sverd eller pil og bue som gjør skade til fiender
    - Health bar. Spilleren kan ta en viss mengde skade før de død. Spillet starter da på nytt

2. **Enemy**
    - Fiender som beveger seg rundt og prøver å skade spilleren
    - Bruk av forskjellig pixel-art sprites, sydd sammen for å lage forskjellige animasjoner for fiender. Når de står stille, beveger seg, eller angriper osv.
    - Combat noe likt som player.
    - Health bar. Også noe likt som player. Fiender dør etter en viss mengde damage.

3. **Hitboxes**
    - Usynlige soner som bestemmer hvordan spiller, fiender, og omgivelser interacter med hverandre
    - Hitboxes burde være relativt nøyaktige, men også litt tilgivende for å skape en tilfredsstillende opplevelse
    - Spiller skal ikke kunne gå gjennom vegger eller fiender
    - Når Player hitbox kommer i kontakt med en fiender eller fiende angrep så skal skade registreres 

4**Map**
   - Forskjellige sammensatte rom som spilleren beveger seg rundt i
   - Fiender plassert i alle rom utenom starting rommet. 
   - Bruk av gratis tiles for å lage designet til de forskjellige elementene i rommene
   - Random generation skal brukes for å lage variasjon for hver gang en bruker starter spillet. Genereringen må ha regler slik at rom alltid blir lagd med en viss størrelse, form, fiender osv.

5**Combat**
   - Det vil være tilfeldige våpen tilgjengelig for spilleren. Sverd, crossbow, hammer osv.
   - Forskjellige animasjoner som passer våpentypen i bruk
   - Forskjellig mengder skade fra våpen avhengig av hva som gir mening for typen. For eksempel: 
     - 5 damage fra hammer siden den er stor og slår tregt
     - 4 damage fra sverd siden man slår raskere
     - 2 damage fra crossbow skudd siden skade fra avstand er mindre farlig enn bruk av melee våpen

Sydd sammen så skal komponenten lage en spill opplevelse som er spennende og tilfredsstillende for brukeren. Variasjon skal gi økt varighet av spillet. Bra flyt laget gjennom bra hitboxes, combat og map design skal føre til at bruker får lyst til å spille mer. 

## 3. Gjennomføring og prinsipper

### 3.1 Prinsipper
1. **Organisert koding**
   - Alle komponentene skal være laget seperat. 
     - Dette hjelper med å holde koden organisert, og det gjør det lettere å gjøre endringer på en spesifik ting i fremtiden skulle det trengs. 
     - Hvis alt ligger hulter til bulter så er det både vanskelig å finne ting og det kan lettere ha uønsket påvirkning på urelaterte deler av koden som kan føre til bugs
   - Alle komponenter skal ha klare navn og dokumentering 
   - Generel ryddighet 
2. **Visuell klarhet**
   - Alle de visuelle elementene i spillet skal være klare. Dette betyr:
     - Det skal være tydelig hva som er hva. Spilleren skal lett kunne se hva som er en vegg, hva som er den dør, hva som er en fiende, hvor man kan gå og ikke gå osv.
     - Visuel bevegelse av karakter, fiender, og våpen skal samsvare ganske nøyaktig med hva som skjer i backenden. Hvis man venstreklikker for å svinge sverdet sitt, så skal skade skje når sverdet visuelt treffer en fiende, og ikke bare første frame etter klikk
   - Bruk av passende assets. Art style på vegg tiles, Player model, og crossbow osv. burde se ut som de er fra samme verden.

### 3.2 Skalerbarhet og brukervennlighet
1. **Clutter**
   - Spillet skal kunne klare å håndtere at mange ting skjer samtidig.
   - Økt mengde med fiender, tiles, projectiles, andre animasjoner osv. skal ikke føre til bugs eller crashes.
2. **Cross-platform**
   - Spillet skal fungere på både Windows og Mac uten problemer.
3. **Tilpasning**
   - Spillet bør tilpasse seg til forskjellige systemspesifikasjoner:
     - Oppløsning skal tilpasse seg til brukerens skjerm
     - Veldig viktig slik at spillet ikke ser veldig stort ut i 720p og veldig liten ut i 4k

### 3.3 Optimalisering
   - Optimalisering av koden for både ytelse og tidssparing 
   - Sy sammen pixel art assets for å skape forskjellige animasjoner
     - Dette hjelper med å holde spillet lightweight. 
     - Ser bra ut, tar mindre plass, krever mindre, og er lettere å lage en større "high quality" animasjoner
   - Gjennbruk av kode der det passer
     - Hvis man først har lagd en hitbox for Player, så kan denne brukes videre for fiender istedenfor å lage fiende hitbox fra scratch
     - Samme konsept for andre ting
     - Dette vil hjelpe med å spare tid


## 4. Fremtidige utvidelser

### 4.1 Utvidet innhold
   - Nye fiender med nytt design og angrep
   - Nye våpen for mer variasjon i spillet
   - Tid basert mode så man kan se hvor raskt man kan komme seg gjennom spillet
     - Leaderboard for å sammenligne seg selv med andre
   - Powerup system for nye spennende muligheter

### 4.2 Utvidet platform tilgjengelighet
   - Utvikling av støtte for og tilgjengelighet på Android og iOS

### 4.3 Instillinger
   - Utvikling av en instilling meny slik at brukere har mer kontroll og kan:
     - Endre til ønsket oppløsning 
     - Velge mellom fullscreen, windowed fullscreen, og windowed
     - Endre volum individuelt på bakgrunnsmusikk og sfx


## 5. Utfordringer og risikoer

### 5.1 Tekniske utfordringer

## 6. Misc

### 6.1 CI/CD
