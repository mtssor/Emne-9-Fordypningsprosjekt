# Systemspesifikasjon for fordypningsprosjekt
**Versjon 1.0** | **06. April - 2025**

## Innholdsfortegnelse
1. [Introduksjon](#1-introduksjon)
    - 1.1 [Formål](#11-formål)
    - 1.2 [Systemomfang](#12-systemomfang)
    - 1.3 [Teknologi](#13-teknologistakk)
2. [Systemarkitektur](#2-systemarkitektur)
    - 2.1 [Overordnet arkitektur](#21-overordnet-arkitektur)
    - 2.2 [Komponenter](#22-microservice-komponenter)


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
    - 
  
