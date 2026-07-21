# Changelog

Změny projektu jsou vedené podle vydávaných verzí. Verze zobrazená v dokumentaci
se při postupu release větve aktualizuje spolu s `WorkScout_app.csproj`.

## [1.0.0-feature] - připravováno

### Přidáno

- Základní WPF aplikace pro Windows s MVVM strukturou.
- Průvodce nastavením Gmailu a e-mailu pro upozornění.
- Ověření IMAP/SMTP připojení a odeslání testovací zprávy.
- Ukázkový režim bez Gmail připojení.
- SQLite databáze, počáteční EF Core migrace a výchozí pracovní portály.
- Filtrování demonstračních nabídek podle profese a portálu.
- Rozhraní `IJobSource` pro budoucí API a scraper zdroje.
- Modely životního cyklu žádosti a příchozích zpráv.
- Unit a integrační testy včetně skutečné migrace do dočasné SQLite databáze.
- Jednotná GitHub Actions pipeline pro testy, coverage, diagnostiku a tag publish.
- Publikační PowerShell skript, česká dokumentace a release checklist.

### Změněno

- Verze aplikace se spravuje pouze v `WorkScout_app.csproj`.
- Release větev se postupně označuje `1.0.0-DEV`, `1.0.0-TEST` a `1.0.0`.
- Databáze se ukládá do `%LocalAppData%\WorkScout` a používá EF migrace.
- Výběr portálů se ukládá mezi spuštěními.

### Zabezpečení

- Gmail app password není ukládán jako otevřený text, ale chráněn Windows DPAPI.
- NuGet audit kontroluje přímé i transitivní závislosti.
- Databáze, credentials, certifikáty a lokální build artefakty jsou ignorované Gitem.
- Automatické testy nekontaktují reálný Gmail ani pracovní portály.

### Omezení

- Nabídky jsou demonstrační a nepocházejí ze skutečných portálů.
- Odesílání CV, pravidelné čtení zpráv a automatické odpovědi nejsou implementované.

## Povýšení verze 1.0.0

Před jednotlivými PR se současně upraví `Version` v `.csproj`, první řádek README
a nadpis této sekce:

1. `1.0.0-feature` → `1.0.0-DEV` před PR do `dev`.
2. `1.0.0-DEV` → `1.0.0-TEST` před PR do `test`.
3. `1.0.0-TEST` → `1.0.0` před PR do `main`; `připravováno` se nahradí datem vydání.
