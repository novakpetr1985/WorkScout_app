# Produktová vize a roadmapa

## Cíl

WorkScout má být osobní pracovní asistent pro jednoho uživatele. Má soustředit
nabídky z povolených zdrojů, pomoci s jejich tříděním, připravovat žádosti a
udržovat dohledatelnou historii komunikace. Automatizace nesmí skrývat, co bylo
jménem uživatele odesláno.

## Zásady

- Přednost má veřejné API nebo feed; scraper je až další možnost.
- Každý portál má samostatný adaptér implementující `IJobSource`.
- Odeslání CV nebo odpovědi v prvních verzích vždy schvaluje uživatel.
- Automatická zpráva se označí jako automatická a použije hlavní adresu v
  `Reply-To`.
- Odpovědi automatů se filtrují, aby nevznikly e-mailové smyčky.
- Citlivá data zůstávají lokální, dokud uživatel výslovně nezvolí jinak.

## Etapa 1 – základ (1.0.x)

- [x] WPF, MVVM, SQLite a migrace.
- [x] Jednorázové nastavení dvou e-mailových adres.
- [x] Demo režim a testovací nabídky.
- [x] Výběr portálů a filtr profesí.
- [x] Databázový model žádosti a příchozí zprávy.
- [ ] Detail nabídky a ruční vytvoření konceptu žádosti.
- [ ] Správa cest k CV bez ukládání příloh do databáze.

## Etapa 2 – první skutečný zdroj (1.1.x)

- Prověřit podmínky a veřejné možnosti vybraného českého pracovního portálu.
- Implementovat jeden `IJobSource` adaptér.
- Deduplikovat nabídky podle URL, externího ID a hashe obsahu.
- Ukládat první a poslední čas nalezení nabídky.
- Zobrazit chybu jednotlivého zdroje bez pádu celé aktualizace.

## Etapa 3 – řízení žádostí (1.2.x)

- Stavy: koncept, čeká na schválení, odesláno, odpověď, pohovor, zamítnuto,
  nabídka a staženo.
- Výběr CV a šablony průvodního textu.
- Náhled celé zprávy před odesláním.
- Auditní záznam odeslání a ruční změny stavu.

## Etapa 4 – příchozí komunikace (1.3.x)

- Pravidelné načítání zpráv z aplikační schránky.
- Přiřazování odpovědí k žádostem podle vláken a identifikátorů zpráv.
- Přeposlání upozornění na hlavní e-mail.
- Bezpečná pravidla automatické odpovědi s ochranou proti smyčkám.

## Pozdější možnosti

- Lokální nebo volitelná AI sumarizace požadavků inzerátu.
- Bodové porovnání nabídky s profilem a CV.
- Report nových nabídek a změn stavů.
- Export historie žádostí do CSV nebo XLSX.
- OAuth připojení Gmailu pro případ použití více uživateli.
