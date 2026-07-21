# Changelog

Významné změny projektu jsou zaznamenávány v tomto souboru. Projekt používá
[Semantic Versioning](https://semver.org/).

## [Unreleased]

### Added

- Základní WPF aplikace pro Windows s MVVM strukturou.
- Průvodce nastavením Gmailu a e-mailu pro upozornění.
- Ověření IMAP/SMTP připojení a testovací e-mail.
- Ukázkový režim bez Gmail připojení.
- SQLite databáze, počáteční EF Core migrace a výchozí pracovní portály.
- Filtrování demonstračních nabídek podle profese a portálu.
- Rozhraní `IJobSource` připravené pro budoucí API a scraper zdroje.
- Modely životního cyklu žádosti a příchozích zpráv.
- Lokální ochrana app passwordu pomocí Windows DPAPI.
- Centralizované sémantické verzování podle vývojového kanálu.
- Unit a integrační testy včetně skutečné migrace do dočasné SQLite databáze.
- Pull request CI, release workflow, šablona PR a dokumentovaný branch flow.
- Základní dokumentace, roadmapa a poznámky k vydání `v1.0.0`.

### Security

- Přihlašovací údaj Gmailu není ukládán jako otevřený text.
- NuGet audit kontroluje přímé i transitivní závislosti v každém pull requestu.
- Databáze, credentials, certifikáty, lokální konfigurace a build artefakty jsou
  vyloučené pomocí `.gitignore`.

### Planned

- První skutečný adaptér pracovního portálu.
- Obrazovka detailu nabídky a založení žádosti.
- Import a výběr CV s potvrzením před odesláním.

Při finálním PR `release/1.0.0` → `main` se sekce přejmenuje na
`[1.0.0] - YYYY-MM-DD` a doplní se odkazy na tag.
