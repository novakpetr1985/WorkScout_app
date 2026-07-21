# WorkScout

WorkScout je lokální Windows aplikace pro jednoho uživatele, která má postupně
spojit vyhledávání pracovních nabídek, přípravu žádostí, e-mailovou komunikaci a
sledování výsledků na jednom místě.

Aktuální vývojová verze je **1.0.0-feature**. Neobsahuje skutečný scraper ani
automatické odesílání žádostí. Nabídky v tabulce jsou záměrně demonstrační.

## Co už funguje

- první nastavení komunikačního Gmailu a e-mailu pro upozornění,
- ověření Gmail připojení přes IMAP a SMTP,
- ochrana app passwordu pomocí Windows DPAPI,
- ukázkový režim bez připojení e-mailu,
- lokální SQLite databáze a EF Core migrace,
- zapamatování vybraných pracovních portálů,
- filtrování testovacích nabídek podle profese a portálu,
- společné rozhraní `IJobSource` pro budoucí API a scraper adaptéry,
- databázový základ pro stavy žádostí a příchozí komunikaci,
- unit a integrační testy s měřením coverage,
- CI kontrola sestavení, testů, formátování a zranitelných balíčků.

## Co zatím nefunguje

- načítání skutečných nabídek z pracovních portálů,
- automatické vyhodnocení nebo sumarizace inzerátů,
- odesílání CV a žádostí,
- pravidelné čtení příchozí pošty,
- automatické odpovědi a notifikace,
- uživatelské rozhraní pro celý stavový tok žádosti.

Podrobný plán je v [ROADMAP.md](docs/ROADMAP.md), pravidla větví a verzování v
[GIT_WORKFLOW.md](docs/GIT_WORKFLOW.md) a změny verzí v [CHANGELOG.md](CHANGELOG.md).

## Rychlé spuštění ukázky

### Z Visual Studia

1. Nainstaluj .NET 10 SDK a workload **.NET desktop development**.
2. Otevři `WorkScout.slnx`.
3. Obnov NuGet balíčky a spusť projekt `WorkScout_app`.
4. V úvodním průvodci zvol **Pokračovat v ukázkovém režimu**.

### Z příkazové řádky

```powershell
dotnet tool restore --configfile NuGet.Config
dotnet restore WorkScout.slnx --configfile NuGet.Config
dotnet run --project WorkScout_app.csproj
```

## Připojení Gmailu

Pro aplikaci použij samostatný Gmail účet, nikoliv hlavní osobní schránku.

1. Na samostatném účtu zapni dvoufázové ověření.
2. Na <https://myaccount.google.com/apppasswords> vytvoř app password.
3. Ve WorkScoutu zadej Gmail a app password a spusť ověření.
4. Doplň hlavní e-mail, na který mají chodit upozornění.

App password není běžné heslo ke Google účtu. WorkScout jej ukládá zašifrovaně
pomocí Windows DPAPI, takže je použitelný pouze pod stejným účtem Windows.

## Data a soukromí

Databáze je pouze lokální:

```text
%LocalAppData%\WorkScout\workscout.db
```

Do repozitáře se databáze ani přihlašovací údaje neukládají. Tlačítko **Změnit
nastavení** odstraní uložené e-mailové propojení a znovu otevře průvodce.

Automatické odesílání žádostí má být v budoucnu vždy dohledatelné a v prvních
verzích potvrzené uživatelem. Implementace zdrojů musí respektovat podmínky
jednotlivých pracovních portálů; před scraperem má přednost oficiální API nebo
veřejný feed.

## Struktura projektu

```text
Data/          DbContext a EF Core migrace
Models/        nastavení, portály, nabídky, žádosti a zprávy
Services/      Gmail, bezpečné uložení a zdroje pracovních nabídek
ViewModels/    stav a příkazy WPF obrazovek
Views/         hlavní panel a průvodce nastavením
docs/          produktová vize a plán dalších kroků
tests/         unit a integrační testy
.github/       CI, release workflow a šablona pull requestu
```

Hlavní knihovny: CommunityToolkit.Mvvm, Entity Framework Core SQLite a MailKit.

## Testy

```powershell
dotnet test WorkScout.slnx -c Release --collect:"XPlat Code Coverage"
```

Unit testy ověřují verzi aplikace, DPAPI úložiště, demonstrační zdroj a změny
výběru portálů. Integrační testy aplikují skutečnou EF migraci do dočasné SQLite
databáze a ověřují persistence žádostí, zpráv a unikátních URL.

## Verzování, větve a release build

`Version.props` obsahuje `VersionPrefix=1.0.0`. CI doplňuje suffix podle větve:
`feature`, `dev`, `test`, `rc`; pouze `main` a tag `v1.0.0` sestavují stabilní
`1.0.0`. Lokální override lze nastavit v ignorovaném souboru
`Version.local.props` podle dodané ukázky.

```powershell
dotnet build WorkScout.slnx -c Release -p:BuildChannel=feature
dotnet publish WorkScout_app.csproj -c Release -r win-x64 --self-contained false -p:BuildChannel=rc -p:PublishSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true
```

Framework-dependent release vyžaduje na cílovém počítači .NET 10 Desktop Runtime.

Pokud jsi spouštěl starší neveřejný prototyp vytvořený přes `EnsureCreated()` a
první migrace narazí na existující tabulky, zazálohuj a odstraň soubor
`%LocalAppData%\WorkScout\workscout.db`. Verze 1.0.0 je první podporované schéma.
