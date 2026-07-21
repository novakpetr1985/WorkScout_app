# Kritické manuální scénáře WorkScout 1.0.0-feature

`P0` blokuje vydání, `P1` blokuje dotčenou funkci a `P2` lze řešit následně.

| ID | Scénář | Priorita | Kroky | Očekávaný výsledek | Stav |
|---|---|---:|---|---|---|
| APP-01 | První start bez DB | P0 | Zazálohovat/odebrat AppData DB a spustit | Migrace vytvoří DB, zobrazí se průvodce | Neprovedeno |
| APP-02 | Opakovaný start | P0 | Dokončit demo setup a restartovat | Otevře se hlavní panel, volby zůstanou | Neprovedeno |
| APP-03 | Demo režim | P0 | Zvolit ukázkový režim | Bez síťového volání se zobrazí testovací nabídky | Neprovedeno |
| MAIL-01 | Platný app password | P0 | Připojit samostatný Gmail | IMAP i SMTP projdou a přijde testovací e-mail | Neprovedeno |
| MAIL-02 | Neplatný app password | P0 | Zadat chybný údaj | Čitelná chyba, konfiguraci nelze dokončit | Neprovedeno |
| MAIL-03 | Změna ověřeného účtu | P0 | Ověřit a poté změnit e-mail/heslo | Ověření se zruší a musí se zopakovat | Neprovedeno |
| MAIL-04 | Reset propojení | P0 | Změnit nastavení a potvrdit | Credential se smaže a otevře se průvodce | Neprovedeno |
| JOB-01 | Výchozí nabídky | P0 | Otevřít demo hlavní panel | Zobrazí se testovací řádky označené zdrojem | Neprovedeno |
| JOB-02 | Filtr profese | P1 | Vybrat jednotlivé profese a aktualizovat | Tabulka obsahuje pouze odpovídající nabídky | Neprovedeno |
| JOB-03 | Výběr portálů | P1 | Zrušit/vybrat portály a restartovat | Filtr se aplikuje a výběr se zachová | Neprovedeno |
| JOB-04 | Žádný portál | P1 | Zrušit celý výběr | Prázdná tabulka a srozumitelná stavová zpráva | Neprovedeno |
| UI-01 | Změna velikosti | P2 | Zmenšit/zvětšit hlavní okno | Ovládací prvky zůstávají použitelné | Neprovedeno |
| REL-01 | Feature metadata | P0 | Otevřít vlastnosti EXE | ProductVersion obsahuje `1.0.0-feature` | Neprovedeno |
| REL-02 | Čistý počítač | P0 | Rozbalit publish na Windows x64 s runtime | Aplikace startuje bez zdrojového kódu a DB | Neprovedeno |
